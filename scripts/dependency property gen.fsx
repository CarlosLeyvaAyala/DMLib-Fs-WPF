#r "nuget: TextCopy"

let getDP (s: string) propertyClass propertyType propertyName =
    s
        .Replace("<propertyName>", propertyName)
        .Replace("<propertyClass>", propertyClass)
        .Replace("<propertyType>", propertyType)
        .Trim()

//#####################################################################################
/// A simple property that doesn't need a complex setup.
/// Can be used for binding internal components, like ItemsSource in the comment example.
let getSimpleCs =
    // Example:
    //<UserControl>
    //  <RadioButton>
    //    <StackPanel>
    //      <TextBlock {Binding ItemsSource} />
    //    </StackPanel>
    //  </RadioButton>
    //</UserControl>
    """
      #region DependencyProperty : <propertyName>
      public <propertyType> <propertyName> {
        get => (<propertyType>)GetValue(<propertyName>Property);
        set => SetValue(<propertyName>Property, value);
      }

      [Category()]
      public static readonly DependencyProperty <propertyName>Property
          = DependencyProperty.Register(
            nameof(<propertyName>),
            typeof(<propertyType>),
            typeof(<propertyClass>),
            new PropertyMetadata(default(<propertyType>)));
      #endregion
    """
    |> getDP

//#####################################################################################
/// Use this to expose a property from a nested control.
/// Using this can expose the IsChecked property of the RadioButton in the comment example.
let getCsOfInternalControls =
    // Example:
    //<UserControl>
    //  <RadioButton>
    //    <StackPanel>
    //      <TextBlock />
    //    </StackPanel>
    //  </RadioButton>
    //</UserControl>
    """
      #region DependencyProperty : <propertyName>
      public <propertyType> <propertyName> {
        get => (<propertyType>)GetValue(<propertyName>Property);
        set {
          <propertyName>PropertyChanged(value);
          SetValue(<propertyName>Property, value);
        }
      }

      [Category()]
      public static readonly DependencyProperty <propertyName>Property
          = DependencyProperty.Register(
            nameof(<propertyName>),
            typeof(<propertyType>),
            typeof(<propertyClass>),
            new PropertyMetadata(default(<propertyType>), propertyChangedCallback: <propertyName>PropertyChanged));

      static void <propertyName>PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
        ((<propertyClass>)d).<propertyName>PropertyChanged((<propertyType>)e.NewValue);
      void <propertyName>PropertyChanged(<propertyType> v){}
      #endregion
    """
    |> getDP

//#####################################################################################
/// Use this to expose an event from a nested control.
let getCsEvent eventClass eventType eventName eventAppendix =
    """  
  #region Event : <eventName>Event
  public event <eventType>Handler <eventName><eventAppendix> {
    add => AddHandler(<eventName>Event, value);
    remove => RemoveHandler(<eventName>Event, value);
  }

  public static readonly <eventType> <eventName>Event
      = EventManager.Register<eventType>(
        nameof(<eventName><eventAppendix>),
        RoutingStrategy.Bubble,
        typeof(<eventType>Handler),
        typeof(<eventClass>));

  protected virtual void Internal<eventName><eventAppendix>(args) => RaiseEvent(new <eventType>Args(<eventName>Event, this));

  private void On<eventName><eventAppendix>(object sender, <eventType>Args e) {
    Internal<eventName><eventAppendix>(args);
    e.Handled = true;
  }
  #endregion
"""
        .Replace("<eventName>", eventName)
        .Replace("<eventType>", eventType)
        .Replace("<eventClass>", eventClass)
        .Replace("<eventAppendix>", eventAppendix)

getCsEvent "IconRadioButton" "RoutedEvent" "Click" ""
|> TextCopy.ClipboardService.SetText

getSimpleCs "IconRadioButton" "PackIconKind" "IconKind"
|> TextCopy.ClipboardService.SetText
