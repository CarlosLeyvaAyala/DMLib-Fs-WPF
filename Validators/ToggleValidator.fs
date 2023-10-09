namespace DMLib_WPF.Validators

open System.Windows.Controls
open System.ComponentModel
open DMLib.TupleCommon

/// Validates that a regular expression is valid.
///
/// Activation can only be done by code.
[<AbstractClass>]
type ToggleRule() =
    inherit ValidationRule()

    member val IsActive = true with get, set

    abstract member DoValidate: obj -> System.Globalization.CultureInfo -> ValidationResult

    override t.Validate(value, culture) =
        if not t.IsActive then
            ValidationResult.ValidResult
        else
            t.DoValidate value culture
