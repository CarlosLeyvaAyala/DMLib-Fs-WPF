// Usage:
// * Copy whole F# static command definition.
// * Set static class namespace.
// * Run script.
//
// This script will detect al commands and will generate XAML bindings for them.

#r "nuget: TextCopy"

// DMLib includes must be deleted once nuget works again
#I "..\..\DMLib-FSharp"
#load "Combinators.fs"
#load "MathL.fs"
#load "Result.fs"
#load "Option.fs"
#load "Tuples.fs"
#load "Array.fs"
#load "String.fs"
#load "List.fs"
#load "Map.fs"
#load "Dictionary.fs"
#load "Objects.fs"
#load "Collections.fs"
#load "Files.fs"
#load "IO\IO.Path.fs"
#load "IO\File.fs"
#load "IO\Directory.fs"
#load "Json.fs"
#load "Misc.fs"
#load "Types\NonEmptyString.fs"
#load "Types\RecordId.fs"
#load "Types\MemoryAddress.fs"
#load "Types\CanvasPoint.fs"
#load "Types\Chance.fs"
#load "Types\Skyrim\EDID.fs"
#load "Types\Skyrim\Weight.fs"
#load "Types\Skyrim\EspFileName.fs"
#load "Types\Skyrim\UniqueId.fs"

open System.Text.RegularExpressions
open DMLib.String

let getCommands () =
    let commands =
        TextCopy.Clipboard().GetText() |> Regex("static member val (\w+) =").Matches

    [ for cmd in commands do
          cmd.Groups[1].Value ]

let classFromClipboard defaultName =
    "c:"
    + match TextCopy.Clipboard().GetText() with
      | Regex "type\s+(\w+)" [ className ] -> className
      | _ -> defaultName
    + "."

let genBindings staticClass =
    let toBindings (cmdName, cmd) =
        let outStr =
            """
            <CommandBinding
            CanExecute="OnCan%s"
            Command="%s"
            Executed="On%s" />
            """

        sprintf (Printf.StringFormat<string -> string -> string -> string>(outStr)) cmdName cmd cmdName

    let toDecls s =
        let d =
            """
              private void OnCan%s(object sender, CanExecuteRoutedEventArgs e) {
                e.CanExecute = ;
              }
              private void On%s(object sender, ExecutedRoutedEventArgs e) {

              }
            """

        sprintf (Printf.StringFormat<string -> string -> string>(d)) s s

    let toMenus cmd =
        $"""
<MenuItem Command="{staticClass}{cmd}"
    Header="{cmd}"
    ToolTip="{{Binding RelativeSource={{RelativeSource Self}}, Path=Command.Text}}" />
"""

    let toCommands cmd = $"Command=\"{staticClass}{cmd}\""

    let fold acc s = acc + s
    let c = getCommands ()

    let b =
        c
        |> List.map (fun s -> s, staticClass + s)
        |> List.map toBindings
        |> List.fold fold ""

    let d = c |> List.map toDecls |> List.fold fold ""
    let m = c |> List.map toMenus |> List.fold fold ""

    let cmd = c |> List.map toCommands |> List.fold smartNl "" |> encloseSame "\n"

    {| bindings = b
       declarations = d
       menus = m
       commandList = cmd |}

let copy = TextCopy.ClipboardService.SetText

let r = "WriteMonologueCmds" |> classFromClipboard |> genBindings

r.bindings |> copy
r.declarations |> copy
r.menus |> copy

r.bindings
r.declarations
r.menus
r.commandList

//////////////////////////////////////////////////////////////////////////////
// Declare command class
open System

[<Literal>]
let cmdDef =
    """
    static member val Cmd =
        { name = "Cmd"
          text = "_Command description"
          gestures =
            [ { keyDisplay = "Key.None"
                key = Key.None
                modifiers = ModifierKeys.None } ] }
        |> create
    """

let getClassDeclaration () =
    let cmdDecl =
        """open DMLib_WPF.Commands
open System.Windows.Input

[<Sealed>]
type %sCmds private () =
    static let create = createCmd<%sCmds>
    static let basic = basicCmd create
        """
        + cmdDef

    printfn "Write the name of the class"
    let cn = Console.ReadLine()

    (cn, cn)
    ||> sprintf (Printf.StringFormat<string -> string -> string>(cmdDecl))
    |> TextCopy.ClipboardService.SetText

getClassDeclaration ()






// https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.modifierkeys?view=windowsdesktop-9.0
"ALT"
"SHIFT"
"CTRL"

let t =
    """
[<Sealed>]
type WriteMonologueCmds private () =
    static let create = createCmd<WriteMonologueCmds>
    static let basic = basicCmd create

    static member val CmdBold = basic "Bold" "Bold"
    static member val CmdItalics = basic "Italics" "Italics"
    static member val CmdTopic = basic "Topic" "Add to the Known Topics List"
    static member val CmdPreview = basic "Preview" "Show preview"
    static member val CmdSpeed = basic "Speed" "Set text speed"

"""

let (|Regexs|_|) pattern input =
    match
        [ for m in System.Text.RegularExpressions.Regex.Matches(input, pattern) ->
              List.tail [ for g in m.Groups -> g.Value ] ]
    with
    | [] -> None
    | l -> Some l

match t with
| Regex @"type (\w+)Cmds private" g -> g[0]
| _ -> "Error: Cmds type not found"

let pattern = @"static member val Cmd(\w+) ="
let input = t


match t with
| Regexs @"static member val Cmd(\w+) =" g -> g |> List.collect id |> List.fold smartNl ""
| _ -> ""
