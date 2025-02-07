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
        // Check for a valid value in CustomTipEntry first
        if (!string.IsNullOrEmpty(CustomTipEntry.Text) &&
            double.TryParse(CustomTipEntry.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var customTip))
        {
            return customTip;
        }

        // Check if TipPercentagePicker has a valid selection
        if (TipPercentagePicker.SelectedIndex != -1 &&
            double.TryParse(TipPercentagePicker.SelectedItem?.ToString()?.TrimEnd('%'), NumberStyles.Any, CultureInfo.InvariantCulture, out var pickerTip))
        {
            return pickerTip;
        }

        // Default to 0.0 if no valid custom or selected value is found
        return 0.0;
    }

    public void CalculateAndDisplay()
    {
        if (!double.TryParse(AmountEntry.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var billAmount))
        {
            billAmount = 0.0;
        }

        var tipPercent = GetTipPercentage();
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
        UpdateUI(tipAmount, totalAmount, splitAmount);
    }

    private void UpdateUI(double tipAmount, double totalAmount, double splitAmount)
    {
        TipLabel.Text = tipAmount.ToString(CurrencyFormat);
        TotalLabel.Text = totalAmount.ToString(CurrencyFormat);

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

    private void TipPercentagePicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker?.SelectedIndex == 4)
        {
            CustomTipEntry.IsVisible = true;
        }
        else
        {
            CustomTipEntry.IsVisible = false;
            CalculateAndDisplay(); // Update the calculation
        }
    }

    private void CustomTipEntry_TextChanged(object? sender, TextChangedEventArgs e)
    {
        CalculateAndDisplay();
    }

    // Reset Button Logic
    public void OnResetButtonClicked(object sender, EventArgs e)
    {
        // Clear the bill amount and custom tip entry
        AmountEntry.Text = string.Empty;
        CustomTipEntry.Text = string.Empty;

        // Reset the picker to the default index (first one)
        TipPercentagePicker.SelectedIndex = 0;
        
        // Reset the split picker to the default value (1 way)
        SplitPicker.SelectedIndex = 0;

        // Uncheck the round-up checkbox
        CheckBox.IsChecked = false;

        // Reset the calculated values
        TipLabel.Text = TotalLabel.Text = PersonCurrencyLabel.Text = "0.00";
        PersonLabel.IsVisible = PersonCurrencyLabel.IsVisible = false;

        // Optionally reset _split and _roundUp as well
        _split = 1;
        _roundUp = false;
    }
}
