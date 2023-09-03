using System.Globalization;

namespace TipCalcuator;

public partial class MainPage
{
    private float _tipPercent = .15f;
    private int _split = 1;
    
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnEntryCompleted(object sender, EventArgs e)
    {
        CalculateAndDisplay();
    }

    private void CalculateAndDisplay()
    {
        var billAmountString = AmountEntry.Text;
        if (!float.TryParse(billAmountString, NumberStyles.Any, CultureInfo.InvariantCulture, out var billAmount))
        {
            billAmount = 0f;
        }

        var tipAmount = billAmount * _tipPercent;
        var totalAmount = billAmount + _tipPercent;

        float splitAmount = 0;
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
        PercentLabel.Text = _tipPercent.ToString("P0", CultureInfo.InvariantCulture);
    }

    private void OnPlusButtonClicked(object sender, EventArgs e)
    {
        _tipPercent += 0.1f;
        CalculateAndDisplay();
    }

    private void OnNegativeButtonClicked(object sender, EventArgs e)
    {
        _tipPercent -= 0.1f;
        CalculateAndDisplay();
    }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedIndex = picker.SelectedIndex;
        _split = selectedIndex + 1;
        CalculateAndDisplay();
    }
}