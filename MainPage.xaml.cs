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

        // Total amount is the bill plus the tip
        var totalAmount = billAmount + tipAmount;
        
        // Calculate the amount per person if splitting
        var splitAmount = totalAmount / _split;
        
        // Update the UI labels
        TipLabel.Text = tipAmount.ToString("C");
        TotalLabel.Text = totalAmount.ToString("C");
        
         var isSplitting = _split > 1;
         PersonLabel.IsVisible = isSplitting;
         PersonCurrencyLabel.IsVisible = isSplitting;
         
         // Update the per-person amount label if splitting
        if (isSplitting)
        {
            PersonCurrencyLabel.Text = splitAmount.ToString("C");
        }
    }
    
    private void PercentEntry_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        CalculateAndDisplay();
    }
        
    private void AmountEntry_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        CalculateAndDisplay();
    }

    private void Picker_OnSelectedIndexChanged(object? sender, EventArgs e)
    {
       var picker = sender as Picker;
       
       // Ensure selectedIndex is not null before using it
       if (picker?.SelectedIndex != null && picker.SelectedIndex >= 0)
       {
        _split = picker.SelectedIndex + 1;
        }
        else
        {
            _split = 1; // Default to 1 if the selection is invalid
        }
        CalculateAndDisplay();
    }

    private void CheckBox_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        var checkbox = sender as CheckBox;
        _roundUp = checkbox?.IsChecked ?? false;
        CalculateAndDisplay();
    }
}