namespace DMLib_WPF.Converters

open System.Windows.Data

[<ValueConversion(typeof<float>, typeof<float>)>]
type MathMultiplyConverter() =
    interface IValueConverter with
        member _.Convert(v, _, p, _) = (v :?> float) * (p :?> float) :> obj
        member _.ConvertBack(_, _, _, _) = 0.0 :> obj
