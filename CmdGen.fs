namespace DMLib_WPF.CmdGen

open System

type Generator() =
    let isNullOrEmpty = String.IsNullOrEmpty

    let smartFold separator acc s =
        if isNullOrEmpty acc then s else acc + separator + s

    let (|Regex|_|) pattern input =
        let m = System.Text.RegularExpressions.Regex.Match(input, pattern)

        if m.Success then
            Some(List.tail [ for g in m.Groups -> g.Value ])
        else
            None

    let (|Regexs|_|) pattern input =
        match
            [ for m in System.Text.RegularExpressions.Regex.Matches(input, pattern) ->
                  List.tail [ for g in m.Groups -> g.Value ] ]
        with
        | [] -> None
        | l -> Some l

    let nl = "\r\n"
    let enclose left right (s: string) = left + s + right
    let smartNl = smartFold nl
    let normalize (cmd: string) = cmd[0].ToString().ToUpper() + cmd[1..]

    let cmdIf fsClass cmd =
        sprintf
            """    <CommandBinding CanExecute="CanCmd%s" Command="c:%sCmds.Cmd%s" Executed="OnCmd%s" />"""
            cmd
            fsClass
            cmd
            cmd

    let cmdSimple fsClass cmd =
        sprintf """    <CommandBinding Command="c:%sCmds.Cmd%s" Executed="OnCmd%s" />""" fsClass cmd cmd

    let gesture fsClass cmd =
        sprintf """    <KeyBinding Command="c:%sCmds.Cmd%s" Gesture="ALT+SHIFT+CTRL+%c" />""" fsClass cmd cmd[0]

    let genXAML wpfClass xmlContainer cmdType a =
        a
        |> Array.map cmdType
        |> Array.fold smartNl ""
        |> enclose (sprintf "<%s.%s>\r\n" wpfClass xmlContainer) (sprintf "\r\n</%s.%s>" wpfClass xmlContainer)

    let getCs cmds =
        cmds
        |> Array.map (fun c ->
            sprintf
                """    private void OnCmd%s(object sender, ExecutedRoutedEventArgs e) => ctx.OnCmd%s();
    private void CanCmd%s(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;"""
                c
                c
                c)
        |> Array.fold smartNl ""


    let fsClassDecl fsClass cmds =
        let d =
            sprintf
                """[<Sealed>]
type %s() =
    inherit WPFBindable()

"""
                fsClass

        cmds
        |> Array.map (sprintf "    member t.OnCmd%s() = ()")
        |> Array.fold smartNl ""
        |> fun fs -> d + fs

    let fsCmdClass fsClass cmds =
        let d =
            sprintf
                """open DMLib_WPF.Commands
open System.Windows.Input

[<Sealed>]
type %sCmds private () =
    static let create = createCmd<%sCmds>
    static let basic = basicCmd create

"""
                fsClass
                fsClass

        cmds
        |> Array.map (fun c -> sprintf """    static member val Cmd%s = basic "%s" "%s description" """ c c c)
        |> Array.fold smartNl ""
        |> fun c -> d + c

    member val XAML = "" with get, set
    member val Fs = "" with get, set
    member val Cs = "" with get, set

    member t.Generate wpfClass fsClass (commands: string) =
        let fsClass = normalize fsClass
        let xaml = genXAML wpfClass
        let cmds = commands.Trim().Split(nl) |> Array.map normalize
        let cmdsB = xaml "CommandBindings"
        let inputB = xaml "InputBindings" (gesture fsClass)

        let ctx =
            sprintf
                """<Window.DataContext>
    <c:%s x:Name="ctx" />
</Window.DataContext>"""
                fsClass

        let sep = nl + nl + "".PadRight(60, '/') + nl + nl

        [ """xmlns:c="clr-namespace:XXX;assembly=YYY" """
          ctx
          inputB cmds
          cmdsB (cmdSimple fsClass) cmds
          cmdsB (cmdIf fsClass) cmds ]
        |> List.fold (smartFold sep) ""
        |> fun x -> t.XAML <- x + nl

        [ fsClassDecl fsClass cmds; fsCmdClass fsClass cmds ]
        |> List.fold (smartFold sep) ""
        |> fun fs -> t.Fs <- fs + nl

        t.Cs <- cmds |> getCs |> (+) nl

    member _.GetFsClass text =
        match text with
        | Regex @"type (\w+)Cmds private" g -> g[0]
        | _ -> "Error: Cmds type not found"

    member _.GetFsCommands text =
        match text with
        | Regexs @"static member val Cmd(\w+) =" g -> g |> List.collect id |> List.fold smartNl ""
        | _ -> ""
