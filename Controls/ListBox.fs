module DMLib_WPF.Controls.ListBox

open System.Windows.Controls
open DMLib

/// Scrolls the ListBox into the currently selected item
let scrollToSelected (lst: ListBox) =
    match lst.SelectedItem with
    | null -> ()
    | s -> lst.ScrollIntoView s

/// Selects the first item in a ListBox
let selectFirst (lst: ListBox) =
    lst.SelectedIndex <- if lst.Items.Count > 0 then 0 else -1
    scrollToSelected lst

/// Selects the first item that fulfills some condition
let selectBy (lst: ListBox) chooser =
    let id =
        [| for i in lst.Items -> i |]
        |> Array.Parallel.mapi (fun i v -> i, v)
        |> Array.Parallel.choose chooser

    lst.SelectedIndex <-
        match id with
        | EmptyArray -> -1
        | OneElemArray x -> x
        | ManyElemArray (y, _) -> y

    scrollToSelected lst
