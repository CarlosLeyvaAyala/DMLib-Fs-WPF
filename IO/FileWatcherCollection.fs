namespace DMLib_WPF.IO

open System.IO

type FileChangeWatcherCollection() =
    let mutable watchers: FileChangeWatcher array = [||]
    let mutable _path = ""

    member private _.iterFileWatchers f = watchers |> Array.iter f

    [<CompiledName "Add">]
    member _.add watcher =
        watchers <- watchers |> Array.insertAt 0 watcher

    /// Sets the same dispatcher to all watchers.
    [<CompiledName "Dispatcher">]
    member t.dispatcher
        with set v = t.iterFileWatchers (fun w -> w.Dispatcher <- v)

    /// Sets the same path to all watchers.
    [<CompiledName "Path">]
    member t.path
        with get () = _path
        and set v =
            if Directory.Exists v then
                t.iterFileWatchers (fun w -> w.Path <- v)

    /// Initial delay before reading when using tryRead.
    [<CompiledName "InitialDelayBeforeReading">]
    member val initialDelayBeforeReading = 5 with get, set

    /// Delay between reads when using tryRead.
    [<CompiledName "DelayBetweenReadings">]
    member val delayBetweenReadings = 200 with get, set

    /// Maximum number of retries when using tryRead.
    [<CompiledName "ReadingRetries">]
    member val readingRetries = 5 with get, set

    /// Tries to execute a reading function a number of times. Fails if it didn't have success.
    [<CompiledName "TryRead">]
    member t.tryRead func arg =
        let wait (secs: int) = System.Threading.Thread.Sleep secs
        let mutable success = false

        wait t.initialDelayBeforeReading

        for retry in [ 1 .. t.readingRetries ] do
            try
                func arg
                success <- true
            with
            | :? IOException -> wait t.delayBetweenReadings

        if not success then
            failwith $"Failed to open \"{arg}\" after {t.readingRetries} tries"
