namespace listShop.Elements;

public partial class shopListElement : ContentView
{
    public shopListElement(string name_arg, int amout, string unit)
    {
        InitializeComponent();
        nameLabel.Text = name_arg;
        amountLabel.Text = amout.ToString() + " " + unit;
    }
}