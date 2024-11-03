namespace TipCalculator.Tests;

public class UnitTest1
{
    [Theory]
    [InlineData(100, 15, 15, 115)]
    [InlineData(200, 20, 40, 240)]
    [InlineData(50, 10, 5, 55)]
    public void CalculateTip_ShouldReturnCorrectValues(double billAmount, double tipPercentage, double expectedTip, double expectedTotal)
    {
        // Arrange
        var mainPage = new MainPage();
        mainPage.AmountEntry.Text = billAmount.ToString();
        mainPage.CustomTipEntry.Text = tipPercentage.ToString();

        // Act
        mainPage.CalculateAndDisplay();

        // Assert
        Assert.Equal(expectedTip.ToString("C"), mainPage.TipLabel.Text);
        Assert.Equal(expectedTotal.ToString("C"), mainPage.TotalLabel.Text);
    }
}