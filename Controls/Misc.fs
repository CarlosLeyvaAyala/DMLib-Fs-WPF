namespace DMLib_WPF.Controls

open System.Windows.Controls
open System
open DMLib
open DMLib.Objects

module ContextMenu =
    [<CompiledName("GetCaller")>]
    let getCaller<'a when 'a: not struct and 'a: null and 'a :> Windows.UIElement> (sender: obj) =
        let nr = nullToResult

        result {
            let! menu = sender :?> MenuItem |> nr
            let! ctxMenu = menu.Parent :?> ContextMenu |> nr
            let! caller = ctxMenu.PlacementTarget :?> 'a |> nr
            return caller
        }
        |> Result.defaultValue null
