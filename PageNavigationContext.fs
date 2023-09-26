namespace DMLib_WPF.Contexts

open DMLib_WPF.Controls
open DMLib
open System
open System.Windows.Controls
open DMLib_WPF.Controls.ListBox
open System.Windows

/// Members expected to have, but can not be added due to type constraints:
///         t.Nav, t.SelectedItem, t.NavSelectedItem.
/// Will always need to be overriden:
///         t.SelectCurrentItem
[<AbstractClass>]
type PageNavigationContext() =
    inherit WPFBindable()

    let mutable isFinishedLoading = false

    member val OwnerWindow: Window = null with get, set

    member t.IsFinishedLoading
        with get () = isFinishedLoading
        and set v =
            isFinishedLoading <- v
            t.OnFinishedLoadingChange()

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

    member t.LoadNav() = t.OnPropertyChanged("Nav")

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
        t.OnceFinishedLoading
            (fun () ->
                t.NavControl.Items.Count > 0
                || t.EnabledControlsConditions.Invoke())
            (fun () -> false)

    abstract member SelectCurrentItem: unit -> unit

    member t.HasItemsSelected = t.NavControl.SelectedItems.Count > 0

    /// Select item when the ListBox selection changes
    default t.SelectCurrentItem() =
        nameof t.CanItemBeSelected |> t.OnPropertyChanged
        nameof t.HasItemsSelected |> t.OnPropertyChanged
        t.OnPropertyChanged("SelectedItem")
