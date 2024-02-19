using CheckBox = Microsoft.Maui.Controls.CheckBox;

namespace TipCalculator;

using System.Globalization;

public partial class MainPage : ContentPage
{
    private int _split = 1;
    private bool _roundUp;
    
    public MainPage()
    {
        InitializeComponent();
    }
    
    private void CalculateAndDisplay()
    {
        var billAmountString = AmountEntry.Text;
        if (!double.TryParse(billAmountString, NumberStyles.Any, CultureInfo.InvariantCulture, out var billAmount))
        {
            billAmount = 0.0;
        }
        
        var tipPercentString = PercentEntry.Text;
        if (!double.TryParse(tipPercentString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tipPercent))
        {
            tipPercent = 0.0;
        }

        var tipAmount = tipPercent / 100 * billAmount;
        if (_roundUp)
        {
            tipAmount = Math.Ceiling(tipAmount);
        }
        var totalAmount = tipPercent / 100 + billAmount;

        var splitAmount = 0.0;
        if (_split == 1)
        {
            PersonLabel.IsVisible = false;
            PersonCurrencyLabel.IsVisible = false;
        }
        else
        {
            splitAmount = totalAmount / _split;
            PersonLabel.IsVisible = true;
            PersonCurrencyLabel.IsVisible = true;
        }

        TipLabel.Text = tipAmount.ToString("C");
        TotalLabel.Text = totalAmount.ToString("C");
        PersonCurrencyLabel.Text = splitAmount.ToString("C");
    }

    private void PercentEntry_OnCompleted(object? sender, EventArgs e)
    {
        SemanticScreenReader.Announce(PercentEntry.Text);
        CalculateAndDisplay();
    }

    private void AmountEntry_OnCompleted(object? sender, EventArgs e)
    {
        SemanticScreenReader.Announce(AmountEntry.Text);
        CalculateAndDisplay();
    }

    private void Picker_OnSelectedIndexChanged(object? sender, EventArgs e)
    {
        var picker = sender as Picker;
        var selectedIndex = picker?.SelectedIndex;
        _split = selectedIndex + 1 ?? 1;
        CalculateAndDisplay();
    }

    private void CheckBox_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        var checkbox = sender as CheckBox;
        _roundUp = checkbox?.IsChecked ?? false;
        CalculateAndDisplay();
    }
}