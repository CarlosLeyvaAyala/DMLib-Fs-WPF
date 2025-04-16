namespace DMLib_WPF.Converters

open System.Windows.Data
open System.Windows
open System.ComponentModel
open System

[<ValueConversion(typeof<Enum>, typeof<string>)>]
type EnumDescriptionConverter() =
    let getDescription v =
        let fi = v.GetType().GetField(v.ToString())

        fi.GetCustomAttributes(typeof<DescriptionAttribute>, false)
        |> Array.tryExactlyOne
        |> Option.map (fun i -> (i :?> DescriptionAttribute).Description)
        |> Option.defaultValue String.Empty

    interface IValueConverter with
        member _.Convert(value, _, _, _) =
            match value with
            | :? Enum as v -> getDescription v
            | _ -> String.Empty

        member _.ConvertBack(_, _, _, _) = String.Empty
