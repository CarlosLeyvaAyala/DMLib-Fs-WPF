namespace DMLib_WPF

open System.ComponentModel

///<summary>A class that has already enabled all plumbing to just tell WPF a property has changed.</summary>
///<remarks>Usage: inherit from this class. When a property has changed,
///call <c>OnPropertyChanged</c>.</remarks>
///<example id="wpfbindable"><code lang="fsharp">
/// type NavItem(uId: string, d: Raw) =
///     inherit WPFBindable()
///
///     member t.Img
///         with get () = img
///         and set (v) =
///             img <- v
///             t.OnPropertyChanged("Img")
///</code>
type WPFBindable() =
    let propertyChanged = Event<PropertyChangedEventHandler, PropertyChangedEventArgs>()

    /////////////////////////////////////////////////////////////////////////////////////////////
    // WPF binding
    // https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/members/events
    [<CLIEvent>]
    member _.PropertyChanged = propertyChanged.Publish

    interface INotifyPropertyChanged with
        member _.add_PropertyChanged(handler) =
            propertyChanged.Publish.AddHandler(handler)

        member _.remove_PropertyChanged(handler) =
            propertyChanged.Publish.RemoveHandler(handler)

    member t.OnPropertyChanged(e: PropertyChangedEventArgs) = propertyChanged.Trigger(t, e)

    member t.OnPropertyChanged(property: string) =
        t.OnPropertyChanged(PropertyChangedEventArgs(property))

    /// Sends a message telling all properties were updated.
    member t.OnPropertyChanged() = t.OnPropertyChanged("")

    member t.OnPropertyChanged a =
        a
        |> List.iter (fun (s: string) -> s |> t.OnPropertyChanged)
