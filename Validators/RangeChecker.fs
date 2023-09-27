namespace DMLib_WPF.Validators

open DMLib.MathL
open System
open System.ComponentModel
open DMLib_WPF

[<AbstractClass>]
type RangeChecker<'a when 'a: comparison>(min: 'a, max: 'a) =
    inherit WPFBindable()
    let mutable isValid = false

    member val Min = min with get, set
    member val Max = max with get, set

    member internal t.setValid v =
        isValid <- v
        t.OnPropertyChanged(PropertyChangedEventArgs("IsValid"))

    member internal t.invalidArg msg =
        t.setValid false
        raise (new ArgumentException(msg))

    member _.IsValid = isValid
    abstract NumericValue: 'a

    member internal t.convert converter (convertErrorMsg: string) setValue (v: string) =
        match converter v with
        | false, _ -> t.invalidArg $"Only {convertErrorMsg} values are allowed"
        | true, x when not (x |> isInRange t.Min t.Max) -> t.invalidArg $"Value must be between {t.Min} and {t.Max}"
        | true, x ->
            t.setValid true
            setValue x

[<Sealed>]
type IntRangeChecker(min, max) =
    inherit RangeChecker<int>(min, max)
    let mutable value = 0
    let setValue x = value <- x

    override _.NumericValue = value

    member t.Value
        with get () = string value
        and set v = t.convert Int32.TryParse "integer" setValue v

[<Sealed>]
type RealRangeChecker(min, max) =
    inherit RangeChecker<float>(min, max)
    let mutable value = 0.0
    let setValue x = value <- x

    override _.NumericValue = value

    member t.Value
        with get () = string value
        and set v = t.convert Double.TryParse "numeric" setValue v
