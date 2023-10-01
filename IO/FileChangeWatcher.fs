namespace DMLib_WPF.IO

open System
open System.IO
open System.Windows
open System.Windows.Threading

type FileChangeWatcher(path, filter, onFileChange: Action<string>, dispatcher: Dispatcher) =
    let avoidRapidFire () =
        let mutable lastCalled = DateTime.Now

        fun doSomethig ->
            let td =
                DateTime
                    .Now
                    .Subtract(
                        lastCalled
                    )
                    .TotalMilliseconds

            if td > 500 then
                doSomethig ()
                lastCalled <- DateTime.Now

    let noRapidFire = avoidRapidFire ()
    let mutable _dispatcher = dispatcher
    let mutable _guiAction: Action<string> = null
    let watcher = new FileSystemWatcher(path, filter)

    /// Avoids thread error due to this function running in a non UI thread.
    let wrapCallback (e: FileSystemEventArgs) (callback: Action<string>) =
        noRapidFire (fun () ->
            let act =
                Action (fun () ->
                    callback.Invoke e.FullPath

                    if _guiAction <> null then
                        _guiAction.Invoke e.FullPath)

            // Watcher is not checked for nullity so it raises an excepcion in case the app doesn't setup the dispatcher
            _dispatcher.Invoke(act))

    let onFileChanged (source: obj) (e: FileSystemEventArgs) = wrapCallback e onFileChange

    do
        watcher.NotifyFilter <- NotifyFilters.LastWrite
        watcher.Changed.AddHandler onFileChanged
        watcher.Created.AddHandler onFileChanged

        // Begin watching
        watcher.EnableRaisingEvents <- true

    ///<summary>Gets or sets the path of the directory to watch.</summary>
    ///<exception cref="T:System.ArgumentException">The specified path does not exist or could not be found.
    ///
    /// -or-
    ///
    /// The specified path contains wildcard characters.
    ///
    /// -or-
    ///
    /// The specified path contains invalid path characters.</exception>
    ///<returns>The path to monitor. The default is an empty string ("").</returns>
    member _.Path
        with get () = watcher.Path
        and set v = watcher.Path <- v

    member _.Dispatcher
        with get () = _dispatcher
        and set v = _dispatcher <- v

    /// Execute an aditional GUI action once the main action was done.
    ///
    /// This is used only if this object was created in F#, but it's called from C#.
    /// There's no need to use this if the object was created in C#, since it will call an
    /// action most likely defined with the purpose of using its GUI capabilities.
    member _.GUIAction
        with get () = _guiAction
        and set v = _guiAction <- v

    /// Intended for use in F#.
    ///
    /// Dispatcher will be null, so it will raise an exception to remind the programmer to set it up.
    new(path, filter, onFileChange) = FileChangeWatcher(path, filter, Action<string>(onFileChange), null)

    /// Intended for use in F#.
    ///
    /// Path will point towards a dummy directory so it can be setup later by the host app.
    ///
    /// Dispatcher will be null, so it will raise an exception to remind the programmer to set it up.
    new(filter, onFileChange) =
        FileChangeWatcher(Directory.GetCurrentDirectory(), filter, Action<string>(onFileChange), null)
