module DMLib_WPF.Commands

open System.Windows.Input

type Gesture =
    { keyDisplay: string
      key: Key
      modifiers: ModifierKeys }

type Cmd =
    { name: string
      text: string
      gestures: Gesture list }

let createCmd<'a> v =
    let inputs = InputGestureCollection()

    v.gestures
    |> List.iter (fun g ->
        inputs.Add(KeyGesture(g.key, g.modifiers, g.keyDisplay))
        |> ignore)

    RoutedUICommand(v.text, v.name, typeof<'a>, inputs)

let blankCmd = { name = ""; text = ""; gestures = [] }

let basicCmd (create: Cmd -> RoutedUICommand) name text =
    { blankCmd with
        name = name
        text = text }
    |> create
