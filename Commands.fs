module DMLib_WPF.Commands

open System.Windows.Input

type Cmd =
    { name: string
      text: string
      keyDisplay: string
      key: Key
      modifiers: ModifierKeys }

let createCmd typeFunc v =
    let inputs = InputGestureCollection()

    inputs.Add(KeyGesture(v.key, v.modifiers, v.keyDisplay))
    |> ignore

    RoutedUICommand(v.text, v.name, typeFunc (), inputs)

let blankCmd =
    { name = ""
      text = ""
      keyDisplay = ""
      key = Key.None
      modifiers = ModifierKeys.None }

let basicCmd (create: Cmd -> RoutedUICommand) name text =
    { blankCmd with
        name = name
        text = text }
    |> create
