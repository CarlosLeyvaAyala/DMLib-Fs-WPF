namespace DMLib_WPF.Converters

open System.Windows.Data
open DMLib.Objects

/// Checks that an object is some value.
/// Can be used for binding RadioButtons.
[<ValueConversion(typeof<obj>, typeof<bool>)>]
type ComparisonConverter() =
    interface IValueConverter with
        member _.Convert(value, _, param, _) =
            match value with
            | IsNull -> false
            | o -> o.Equals param

        member _.ConvertBack(value, _, param, _) =
            match value with
            | IsNull -> Binding.DoNothing
            | o when o.Equals true -> param
            | _ -> Binding.DoNothing
