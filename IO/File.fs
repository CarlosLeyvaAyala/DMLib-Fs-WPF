module DMLib_WPF.IO.File

open System.Windows
open DMLib

let getDroppedFiles (e: DragEventArgs) =
    match e.Data.GetDataPresent DataFormats.FileDrop with
    | false -> [||]
    | true -> e.Data.GetData(DataFormats.FileDrop) :?> (string array)

let getDroppedFile (e: DragEventArgs) =
    e |> getDroppedFiles |> Array.firstOr ""
