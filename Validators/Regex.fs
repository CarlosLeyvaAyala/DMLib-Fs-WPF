namespace DMLib_WPF.Validators

open System.Windows.Controls
open System.Text.RegularExpressions
open DMLib.Objects

/// Validates that a regular expression is valid
type RegexRule() =
    inherit ValidationRule()

    override _.Validate(value: obj, _) =
        try
            Regex(
                match value with
                | IsNotNull v -> v :?> string
                | _ -> ""
            )
            |> ignore

            ValidationResult.ValidResult
        with
        | _ -> ValidationResult(false, "Not a valid regular expression")
