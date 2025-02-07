using System.Globalization;

namespace EasyTipperApp;

public partial class MainPage : ContentPage
{
    private int _split = 1;
    private bool _roundUp;
    private const string CurrencyFormat = "C";

    public MainPage()
    {
        InitializeComponent();
    }

    private double GetTipPercentage()
    {
        // Try to get a custom tip if available
        if (!string.IsNullOrEmpty(CustomTipEntry.Text) && 
            double.TryParse(CustomTipEntry.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var customTip))
        {
            return customTip;
        }

        // Check the selected tip percentage
        if (TipPercentagePicker.SelectedIndex >= 0 && 
            double.TryParse(TipPercentagePicker.SelectedItem?.ToString()?.TrimEnd('%'), NumberStyles.Any, CultureInfo.InvariantCulture, out var selectedTip))
        {
            return selectedTip;
        }

        // Default to 0% if neither a custom value nor a selection exists
        return 0.0;
    }

    public void CalculateAndDisplay()
    {
        // Attempt to parse the bill amount, default to 0 if invalid
        if (!double.TryParse(AmountEntry.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var billAmount) || billAmount <= 0)
        {
            billAmount = 0.0;
            TipLabel.Text = TotalLabel.Text = PersonCurrencyLabel.Text = "0.00"; // Clear labels when input is invalid
            return;
        }

        var tipPercent = GetTipPercentage();
        var tipAmount = tipPercent / 100 * billAmount;

        if (_roundUp)
        {
            tipAmount = Math.Ceiling(tipAmount);
        }

        // Total amount includes bill + tip
        var totalAmount = billAmount + tipAmount;

        // Split total if necessary
        var splitAmount = totalAmount / _split;

        // Update UI based on the calculated values
        UpdateUI(tipAmount, totalAmount, splitAmount);
    }

    private void UpdateUI(double tipAmount, double totalAmount, double splitAmount)
    {
        TipLabel.Text = tipAmount.ToString(CurrencyFormat);
        TotalLabel.Text = totalAmount.ToString(CurrencyFormat);

        // Handle visibility of the "Per Person" labels based on whether the bill is being split
        var isSplitting = _split > 1;
        PersonLabel.IsVisible = isSplitting;
        PersonCurrencyLabel.IsVisible = isSplitting;

        if (isSplitting)
        {
            PersonCurrencyLabel.Text = splitAmount.ToString(CurrencyFormat);
        }
    }

    private void AmountEntry_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        CalculateAndDisplay();
    }

    private void SplitPicker_OnSelectedIndexChanged(object? sender, EventArgs e)
    {
        // Handle invalid index and update split accordingly
        var picker = sender as Picker;
        _split = picker?.SelectedIndex >= 0 ? picker.SelectedIndex + 1 : 1; // Default to 1 if invalid index

        CalculateAndDisplay();
    }

    private void CheckBox_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        _roundUp = e.Value;
        CalculateAndDisplay();
    }

    private void TipPercentagePicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        // Show custom tip entry if the "Custom" option is selected
        CustomTipEntry.IsVisible = TipPercentagePicker.SelectedIndex == 4;
        CalculateAndDisplay(); // Recalculate on tip change
    }

    private void CustomTipEntry_TextChanged(object? sender, TextChangedEventArgs e)
    {
        CalculateAndDisplay();
    }
}
