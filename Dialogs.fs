module DMLib_WPF.Dialogs

open System.Windows
open Microsoft.WindowsAPICodePack.Dialogs
open DMLib.String
open System
open System.Runtime.InteropServices

[<AutoOpen>]
module private DlgOps =
    let executeDlg args (onYes: Action<string>) (onNo: Action) dlgFunc =
        match dlgFunc args with
        | IsWhiteSpaceStr ->
            match onNo with
            | null -> ()
            | no -> no.Invoke()
        | fn -> onYes.Invoke fn

[<Sealed>]
type File private () =
    static let Dlg (dlgClass: unit -> System.Windows.Forms.FileDialog) filter guid title fileName =
        use mutable dlg = dlgClass ()
        dlg.Filter <- filter

        match title with
        | IsEmptyStr -> ()
        | t -> dlg.Title <- t

        match fileName with
        | IsEmptyStr -> ()
        | f -> dlg.FileName <- f

        match guid with
        | IsEmptyStr -> ()
        | g -> dlg.ClientGuid <- new System.Guid(g)

        match dlg.ShowDialog() with
        | System.Windows.Forms.DialogResult.OK -> dlg.FileName
        | _ -> null

    /// Save file
    static member Save
        (
            filter,
            guid,
            [<Optional; DefaultParameterValue("")>] title,
            [<Optional; DefaultParameterValue("")>] fileName
        ) =
        Dlg (fun () -> new System.Windows.Forms.SaveFileDialog()) filter guid title fileName

    /// Open file
    static member Open
        (
            filter,
            guid,
            [<Optional; DefaultParameterValue("")>] title,
            [<Optional; DefaultParameterValue("")>] fileName
        ) =
        Dlg (fun () -> new System.Windows.Forms.OpenFileDialog()) filter guid title fileName

    /// Execute save file
    static member Save
        (
            filter: string,
            onYes: Action<string>,
            [<Optional; DefaultParameterValue("")>] guid: string,
            [<Optional; DefaultParameterValue("")>] fileName: string,
            [<Optional; DefaultParameterValue("")>] title: string,
            [<Optional; DefaultParameterValue(null: Action)>] onNo: Action
        ) =
        executeDlg (filter, guid, title, fileName) onYes onNo File.Save

    /// Execute open file
    static member Open
        (
            filter: string,
            onYes: Action<string>,
            [<Optional; DefaultParameterValue("")>] guid: string,
            [<Optional; DefaultParameterValue("")>] fileName: string,
            [<Optional; DefaultParameterValue("")>] title: string,
            [<Optional; DefaultParameterValue(null: Action)>] onNo: Action
        ) =
        executeDlg (filter, guid, title, fileName) onYes onNo File.Open

[<Obsolete("Use Directory.Select")>]
let SelectDir startingDir =
    use mutable dlg = new CommonOpenFileDialog()

    if not (isNullOrWhiteSpace startingDir) then
        dlg.InitialDirectory <- startingDir

    dlg.IsFolderPicker <- true

    if dlg.ShowDialog() = CommonFileDialogResult.Ok then
        dlg.FileName
    else
        ""

[<Sealed>]
type Directory private () =
    static member Select startingDir =
        use mutable dlg = new CommonOpenFileDialog()

        if not (isNullOrWhiteSpace startingDir) then
            dlg.InitialDirectory <- startingDir

        dlg.IsFolderPicker <- true

        if dlg.ShowDialog() = CommonFileDialogResult.Ok then
            dlg.FileName
        else
            ""

    static member Select
        (
            startingDir,
            onYes: Action<string>,
            ([<Optional; DefaultParameterValue(null: Action)>] onNo: Action)
        ) =
        executeDlg startingDir onYes onNo Directory.Select

let private okMessageBox icon (owner: Window) text caption =
    MessageBox.Show(owner, text, caption, MessageBoxButton.OK, icon)

let private yesNoMessageBox icon (owner: Window) text caption =
    MessageBox.Show(owner, text, caption, MessageBoxButton.YesNo, icon, MessageBoxResult.No)

[<Obsolete("Use MessageBox")>]
let AsteriskMessageBox owner text caption =
    okMessageBox MessageBoxImage.Asterisk owner text caption

[<Obsolete("Use MessageBox")>]
let AsteriskYesNoMessageBox owner text caption =
    yesNoMessageBox MessageBoxImage.Asterisk owner text caption

[<Obsolete("Use MessageBox")>]
let WarningMessageBox owner text caption =
    okMessageBox MessageBoxImage.Warning owner text caption

[<Obsolete("Use MessageBox")>]
let WarningYesNoMessageBox owner text caption =
    yesNoMessageBox MessageBoxImage.Warning owner text caption

[<Obsolete("Use MessageBox")>]
let ErrorMessageBox owner text caption =
    okMessageBox MessageBoxImage.Error owner text caption

[<Obsolete("Use MessageBox")>]
let ErrorYesNoMessageBox owner text caption =
    yesNoMessageBox MessageBoxImage.Error owner text caption

[<Obsolete("Use MessageBox")>]
let ExceptionMessageBox owner text =
    ErrorMessageBox owner text "Unexpected error"

[<AutoOpen>]
module private MBOps =
    let executeDlg icon owner text caption (onYes: Action) (onNo: Action) =
        match yesNoMessageBox icon (owner: Window) text caption with
        | MessageBoxResult.No ->
            match onNo with
            | null -> ()
            | no -> no.Invoke()
        | _ -> onYes.Invoke()

[<Sealed>]
type MessageBox private () =
    [<Literal>]
    static let A = MessageBoxImage.Asterisk

    [<Literal>]
    static let W = MessageBoxImage.Warning

    [<Literal>]
    static let E = MessageBoxImage.Error

    static member Asterisk(owner, text, caption) = okMessageBox A owner text caption

    static member Asterisk
        (
            owner,
            text,
            caption,
            onYes: Action,
            ([<Optional; DefaultParameterValue(null: Action)>] onNo: Action)
        ) =
        executeDlg A owner text caption onYes onNo

    static member Warning(owner, text, caption) = okMessageBox W owner text caption

    static member Warning
        (
            owner,
            text,
            caption,
            onYes: Action,
            ([<Optional; DefaultParameterValue(null: Action)>] onNo: Action)
        ) =
        executeDlg W owner text caption onYes onNo

    static member Error(owner, text, caption) = okMessageBox E owner text caption

    static member Error
        (
            owner,
            text,
            caption,
            onYes: Action,
            ([<Optional; DefaultParameterValue(null: Action)>] onNo: Action)
        ) =
        executeDlg E owner text caption onYes onNo

    static member Exception(owner, text) =
        MessageBox.Error(owner, text, "Unexpected error")
