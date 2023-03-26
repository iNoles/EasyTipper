using System.Globalization;

namespace TipCalcuator;

public partial class MainPage : ContentPage
{
    private float tipPercent = .15f;
    private int split = 1;

    public MainPage()
	{
		InitializeComponent();
	}

    void OnEntryCompleted(object sender, EventArgs e)
	{
		CalculateAndDisplay();
	}

	void OnPlusButtonClicked(object sender, EventArgs args)
	{
		tipPercent += 0.1f;
		CalculateAndDisplay();
	}

    void OnNegativeButtonClicked(object sender, EventArgs args)
	{
		tipPercent -= 0.1f;
		CalculateAndDisplay();
    }

	void OnPickerSelectedIndexChanged(object sender, EventArgs e)
	{
        var picker = (Picker)sender;
		int selectedIndex = picker.SelectedIndex;
        split = selectedIndex + 1;
        CalculateAndDisplay();
	}

    private void CalculateAndDisplay()
	{
		string billAmountString = AmountEntry.Text;
		float billAmount;
		if (billAmountString.Equals("")) {
            billAmount = 0;
        }
        else
        {
			billAmount = float.Parse(billAmountString);
		}

        float tipAmount = billAmount * tipPercent;
        float totalAmount = billAmount + tipPercent;

        float splitAmount = 0;
		if (split == 1) {
			personLabel.IsVisible = false;
			personCurrencyLabel.IsVisible = false;
		}
		else
		{
			splitAmount = totalAmount / split;
			personLabel.IsVisible = true;
			personCurrencyLabel.IsVisible = true;
		}

 
		tipLabel.Text = tipAmount.ToString("C");
		totalLabel.Text = totalAmount.ToString("C");
		personCurrencyLabel.Text = splitAmount.ToString("C");
		percentLabel.Text = tipPercent.ToString("P0", CultureInfo.InvariantCulture);
    }
}


