namespace DMLib_WPF.Converters

open System.Windows.Data
open System.Windows

[<ValueConversion(typeof<bool>, typeof<Visibility>)>]
type BoolToVisibilityConverter() =
    interface IValueConverter with
        member _.Convert(value, _, _, _) =
            match value with
            | :? bool as o when o = true -> Visibility.Visible
            | _ -> Visibility.Collapsed

        member _.ConvertBack(value, _, _, _) =
            match value with
            | :? Visibility as o when o = Visibility.Visible -> true
            | _ -> false

[<ValueConversion(typeof<bool>, typeof<Visibility>)>]
type InverseBoolToVisibilityConverter() =
    interface IValueConverter with
        member _.Convert(value, _, _, _) =
            match value with
            | :? bool as o when o = false -> Visibility.Visible
            | _ -> Visibility.Collapsed

        member _.ConvertBack(value, _, _, _) =
            match value with
            | :? Visibility as o when o = Visibility.Visible -> false
            | _ -> true
