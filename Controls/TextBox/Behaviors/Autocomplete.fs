namespace DMLib_WPF.Controls.TextBox.Behaviors

open System.Windows
open System.Windows.Controls
open System.Diagnostics
open System.Collections.Generic
open DMLib.Objects
open DMLib.String
open DMLib.Combinators
open System
open DMLib
open System.Windows.Data

[<AutoOpen>]
module private AutoCompletePrivateOps =
    let getOriginalSource (e: TextChangedEventArgs) =
        match e.OriginalSource with
        | :? TextBox -> Ok()
        | _ -> Error()

    let getChanges (e: TextChangedEventArgs) =
        if e.Changes
           |> Seq.filter (fun c -> c.AddedLength > 0)
           |> Seq.isEmpty then
            Error()
        else
            Ok()

    let getTextbox (sender: obj) =
        match sender with
        | null -> Error()
        | :? TextBox as tb -> Ok tb
        | _ -> Error()

    let requireStr =
        function
        | IsEmptyStr -> Error()
        | s -> Ok s

    let getRequiredSeq a =
        if a |> Seq.isEmpty then
            Error()
        else
            Ok a

    let getStartingPoint indicator multiIndicator alwaysStart txt =
        let indicators =
            match multiIndicator with
            | true ->
                indicator
                |> Seq.map (fun c -> c.ToString())
                |> Seq.toList
            | false when isNullOrEmpty indicator -> []
            | false -> [ indicator ]

        match indicators with
        | [] -> Ok(0, txt) // Predict only from the start
        | a ->
            match indicators
                  |> List.map (swap lastIndexOf txt)
                  |> List.max,
                  alwaysStart
                with
            | -1, false -> Error() // Indicators are required, but none was found
            | -1, true -> Ok(0, txt) // Predict also from the start
            | start, _ -> // Indicator was expected and found
                Ok(start + 1, txt[start + 1 ..])

[<Sealed>]
type Autocomplete private () =
    inherit DependencyObject()
    static let mutable canChange = Ok()

    static let OnTextChanged (sender: obj) (e: TextChangedEventArgs) =
        result {
            let! _ = canChange
            let! _ = getOriginalSource e // No original source. Get out.
            let! _ = getChanges e // No changes. Get out.
            let! tb = getTextbox sender
            let! txt = requireStr tb.Text

            let! values =
                tb
                |> Autocomplete.GetItemsSource
                |> getRequiredSeq

            let! (textToMatchStart, textToMatch') =
                getStartingPoint
                    (Autocomplete.GetIndicator tb)
                    (Autocomplete.GetMultipleIndicators tb)
                    (Autocomplete.GetAlwaysAutocompleteStart tb)
                    //txt
                    (txt[.. tb.CaretIndex])

            let! textToMatch = requireStr textToMatch'
            let txtLen = textToMatch.Length
            let comparison = Autocomplete.GetStringComparison tb

            let! match' =
                values
                |> Seq.toArray
                |> Array.Parallel.choose (fun (s: string) ->
                    if s.Length >= txtLen
                       && s[ .. txtLen - 1 ].Equals(textToMatch, comparison) then
                        Some(s)
                    //Some(s, s[txtLen..])
                    else
                        None)
                |> Array.first
                |> Result.ofOption ()

            let matchStart = textToMatchStart + textToMatch.Length
            canChange <- Error()
            let newTxt = replaceAt txt textToMatchStart match'

            match BindingOperations.GetBinding(tb, TextBox.TextProperty) with
            | null -> tb.Text <- newTxt
            | _ -> tb.SetCurrentValue(TextBox.TextProperty, newTxt)

            tb.CaretIndex <- matchStart
            tb.SelectionStart <- matchStart
            tb.SelectionLength <- tb.Text.Length - textToMatchStart
            canChange <- Ok()

            return ()
        }
        |> ignore

    static let onTextChanged = TextChangedEventHandler(OnTextChanged)

    static let onAutoCompleteItemsSource (sender: obj) (e: DependencyPropertyChangedEventArgs) =
        match sender with
        | :? TextBox as tb ->
            tb.TextChanged.RemoveHandler onTextChanged

            if isNotNull e.NewValue then
                tb.TextChanged.AddHandler onTextChanged
        | _ -> ()

    // -------------------------------
    // Properties
    static let ItemsSourceProperty =
        DependencyProperty.RegisterAttached(
            "ItemsSource",
            typeof<IEnumerable<string>>,
            typeof<Autocomplete>,
            UIPropertyMetadata(null, onAutoCompleteItemsSource)
        )

    static let IndicatorProperty =
        DependencyProperty.RegisterAttached("Indicator", typeof<string>, typeof<Autocomplete>, UIPropertyMetadata(""))

    static let MultipleIndicatorsProperty =
        DependencyProperty.RegisterAttached(
            "MultipleIndicators",
            typeof<bool>,
            typeof<Autocomplete>,
            UIPropertyMetadata(true)
        )

    static let AlwaysAutocompleteStartProperty =
        DependencyProperty.RegisterAttached(
            "AlwaysAutocompleteStart",
            typeof<bool>,
            typeof<Autocomplete>,
            UIPropertyMetadata(true)
        )

    static let StringComparisonProperty =
        DependencyProperty.RegisterAttached(
            "StringComparison",
            typeof<StringComparison>,
            typeof<Autocomplete>,
            UIPropertyMetadata(StringComparison.Ordinal)
        )

    // -------------------------------
    // Properties setters/getters
    static member SetItemsSource (a: DependencyObject) (v: IEnumerable<string>) = a.SetValue(ItemsSourceProperty, v)

    static member GetItemsSource(a: DependencyObject) =
        a.GetValue ItemsSourceProperty :?> IEnumerable<string>

    static member SetIndicator (a: DependencyObject) (v: string) = a.SetValue(IndicatorProperty, v)
    static member GetIndicator(a: DependencyObject) = a.GetValue IndicatorProperty :?> string

    static member SetMultipleIndicators (a: DependencyObject) (v: bool) =
        a.SetValue(MultipleIndicatorsProperty, v)

    static member GetMultipleIndicators(a: DependencyObject) =
        a.GetValue MultipleIndicatorsProperty :?> bool

    static member SetAlwaysAutocompleteStart (a: DependencyObject) (v: bool) =
        a.SetValue(MultipleIndicatorsProperty, v)

    static member GetAlwaysAutocompleteStart(a: DependencyObject) =
        a.GetValue MultipleIndicatorsProperty :?> bool

    static member SetStringComparison (a: DependencyObject) (v: StringComparison) =
        a.SetValue(StringComparisonProperty, v)

    static member GetStringComparison(a: DependencyObject) =
        a.GetValue StringComparisonProperty :?> StringComparison
