module DMLib_WPF.Controls.ListBox

open System.Windows.Controls
open DMLib
open System
open DMLib.Objects

/// Converts a ListBox items to an array of objects
[<CompiledName("ItemsArray")>]
let itemsArray<'a> (lst: ListBox) = [| for i in lst.Items -> i :?> 'a |]

/// Returns the first possible index of a ListBox.
[<CompiledName("FirstIndex")>]
let firstIndex (lst: ListBox) = if lst.Items.Count = 0 then -1 else 0

/// <summary>Returns the last possible index of a ListBox.</summary>
/// <param name="lb">ListBox to check.</param>
/// <returns>Last possible index.</returns>
[<CompiledName("LastIndex")>]
let lastIndex (lst: ListBox) = lst.Items.Count - 1

/// Scrolls the ListBox into the currently selected item
[<CompiledName("ScrollToSelected")>]
let scrollToSelected (lst: ListBox) =
    match lst.SelectedItem with
    | null -> ()
    | s -> lst.ScrollIntoView s

/// Selects the first item in a ListBox
[<CompiledName("SelectFirst")>]
let selectFirst (lst: ListBox) =
    lst.SelectedIndex <- firstIndex lst
    scrollToSelected lst

/// <summary>Selects the last item of a ListBox.</summary>
/// <param name="lb">ListBox to work on.</param>
[<CompiledName("SelectLast")>]
let selectLast (lst: ListBox) =
    lst.SelectedIndex <- lastIndex lst
    scrollToSelected lst

/// Finds the index of the first item that fulfills some condition.
[<CompiledName("IndexOf")>]
let indexOf<'a> predicate (lst: ListBox) =
    match lst
          |> itemsArray<'a>
          |> Array.tryFindIndex predicate
        with
    | Some i -> i
    | None -> -1

/// Selects the first item that fulfills some condition.
[<CompiledName("SelectBy")>]
let selectBy<'a> (lst: ListBox) predicate =
    lst.SelectedIndex <- lst |> indexOf<'a> predicate
    scrollToSelected lst

/// <summary>Returns the new index on a ListBox after deleting one item.</summary>
/// <param name="lst">ListBox to check.</param>
/// <returns>The new expected SelectedItemIndex.</returns>
[<CompiledName("IndexAfterDeleting")>]
let indexAfterDeleting (lst: ListBox) =
    let curridx = lst.SelectedIndex
    let last = lastIndex lst

    if curridx >= last then
        last - 1
    else
        curridx

/// <summary>Selects the new index on a ListBox after deleting one item.</summary>
[<CompiledName("SelectIndexAfterDeleting")>]
let selectIndexAfterDeleting (doSomething: Action) (lst: ListBox) =
    let newI = indexAfterDeleting lst
    doSomething.Invoke()
    lst.SelectedIndex <- newI

/// Makes sure an index is not outside the possible indexes.
[<CompiledName("EnsureValidIndex")>]
let ensureValidIndex idx (lst: ListBox) =
    Math.Clamp(idx, firstIndex lst, lastIndex lst)

/// Makes sure to return an item only if the ListBox is assigned
let getSelectedItem (lst: ListBox) =
    match lst with
    | IsNull -> null
    | l -> l.SelectedItem
