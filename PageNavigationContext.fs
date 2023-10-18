namespace DMLib_WPF.Contexts

open DMLib_WPF.Controls
open System
open System.Windows.Controls
open DMLib_WPF.Controls.ListBox
open DMLib
open DMLib_WPF
open System.Windows.Threading
open DMLib.Objects

/// Members expected to have, but can not be added due to type constraints:
///         t.Nav, t.SelectedItem, t.NavSelectedItem.
/// Will always need to be overriden:
///         t.SelectCurrentItem
///
/// A property to navigate by using ListBox.SelectedItem was attempted, but it
/// slow and moody.
[<AbstractClass>]
type PageNavigationContext() =
    inherit WPFBindable()

    let mutable isFinishedLoading = false

    member val GuiDispatcher: Dispatcher = null with get, set

    member t.ExecuteInGUI f =
        makeAsync (fun () -> t.GuiDispatcher.Invoke(Action f)) ()

    member t.IsFinishedLoading
        with get () = isFinishedLoading
        and set v =
            isFinishedLoading <- v
            t.OnFinishedLoadingChange()

    /// The WPF app says this page was activated.
    abstract member Activate: unit -> unit
    default t.Activate() = ()

    abstract member OnFinishedLoadingChange: unit -> unit

    default t.OnFinishedLoadingChange() =
        t.OnPropertyChanged(nameof t.CanItemBeSelected)

    member t.OnceFinishedLoading whenLoaded whenNotLoaded =
        if not t.IsFinishedLoading then
            whenNotLoaded ()
        else
            whenLoaded ()

    member val EnabledControlsConditions: Func<bool> = null with get, set
    member val NavControl: ListBox = null with get, set

    abstract member RebuildNav: unit -> unit

    /// Sends the signal to build the nav.
    /// Nav won't be built until IsFinishedLoading is set to true.
    member t.LoadNav() =
        t.RebuildNav()
        t.OnPropertyChanged("Nav")

    member t.ReloadNavAndGoToFirst() =
        t.LoadNav()
        ListBox.selectFirst t.NavControl

    member t.ReloadNavAndGoToIndex idx =
        t.LoadNav()
        t.NavControl.SelectedIndex <- t.NavControl |> ListBox.ensureValidIndex idx

    member t.DeleteSelected doDelete =
        let i = indexAfterDeleting t.NavControl
        doDelete ()
        t.NavControl.SelectedIndex <- i
        t.ReloadNavAndGoToIndex i

    abstract member ReloadNavAndGoToCurrent: unit -> unit

    /// Disables UI if an item can not be selected
    member t.CanItemBeSelected =
        let conditions () =
            match t.EnabledControlsConditions with
            | IsNull -> false
            | action -> action.Invoke()

        t.OnceFinishedLoading (fun () -> t.NavControl.Items.Count > 0 || conditions ()) (fun () -> false)

    abstract member SelectCurrentItem: unit -> unit

    member t.HasItemsSelected = t.NavControl.SelectedItems.Count > 0

    /// Select item when the ListBox selection changes
    default t.SelectCurrentItem() =
        nameof t.CanItemBeSelected |> t.OnPropertyChanged
        nameof t.HasItemsSelected |> t.OnPropertyChanged
        t.OnPropertyChanged("SelectedItem")

    member t.ReloadSelectedItem() = t.SelectCurrentItem()
