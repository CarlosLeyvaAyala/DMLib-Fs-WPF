namespace DMLib_WPF.Converters

open System.Windows.Data
open DMLib.Objects

/// Checks that an object is some value.
/// Can be used for binding RadioButtons.
///
/// https://stackoverflow.com/questions/1317891/simple-wpf-radiobutton-binding
/// https://stackoverflow.com/questions/397556/how-to-bind-radiobuttons-to-an-enum/406798#406798
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
