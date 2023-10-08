module DMLib_WPF.Controls.DataGrid

open System.Windows.Controls
open System

/// Converts a DataGrid items to an array of objects
[<CompiledName("ItemsArray")>]
let itemsArray<'a> (lst: DataGrid) = [| for i in lst.Items -> i :?> 'a |]

/// Returns the first possible index of a DataGrid.
[<CompiledName("FirstIndex")>]
let firstIndex (lst: DataGrid) = if lst.Items.Count = 0 then -1 else 0

/// <summary>Returns the last possible index of a DataGrid.</summary>
/// <param name="lb">DataGrid to check.</param>
/// <returns>Last possible index.</returns>
[<CompiledName("LastIndex")>]
let lastIndex (lst: DataGrid) = lst.Items.Count - 1

/// Scrolls the DataGrid into the currently selected item
[<CompiledName("ScrollToSelected")>]
let scrollToSelected (lst: DataGrid) =
    match lst.SelectedItem with
    | null -> ()
    | s -> lst.ScrollIntoView s

/// Selects the first item in a DataGrid
[<CompiledName("SelectFirst")>]
let selectFirst (lst: DataGrid) =
    lst.SelectedIndex <- firstIndex lst
    scrollToSelected lst

/// <summary>Selects the last item of a DataGrid.</summary>
/// <param name="lb">DataGrid to work on.</param>
[<CompiledName("SelectLast")>]
let selectLast (lst: DataGrid) =
    lst.SelectedIndex <- lastIndex lst
    scrollToSelected lst

/// Finds the index of the first item that fulfills some condition.
[<CompiledName("IndexOf")>]
let indexOf<'a> predicate (lst: DataGrid) =
    match lst
          |> itemsArray<'a>
          |> Array.tryFindIndex predicate
        with
    | Some i -> i
    | None -> -1

/// Selects the first item that fulfills some condition.
[<CompiledName("SelectBy")>]
let selectBy<'a> (lst: DataGrid) predicate =
    lst.SelectedIndex <- lst |> indexOf<'a> predicate
    scrollToSelected lst

/// <summary>Returns the new index on a DataGrid after deleting one item.</summary>
/// <param name="lst">DataGrid to check.</param>
/// <returns>The new expected SelectedItemIndex.</returns>
[<CompiledName("IndexAfterDeleting")>]
let indexAfterDeleting (lst: DataGrid) =
    let curridx = lst.SelectedIndex
    let last = lastIndex lst

    if curridx >= last then
        last - 1
    else
        curridx

/// <summary>Selects the new index on a DataGrid after deleting one item.</summary>
[<CompiledName("SelectIndexAfterDeleting")>]
let selectIndexAfterDeleting (doSomething: Action) (lst: DataGrid) =
    let newI = indexAfterDeleting lst
    doSomething.Invoke()
    lst.SelectedIndex <- newI

/// Makes sure an index is not outside the possible indexes.
[<CompiledName("EnsureValidIndex")>]
let ensureValidIndex idx (lst: DataGrid) =
    Math.Clamp(idx, firstIndex lst, lastIndex lst)
