using MySql.Data.MySqlClient;

namespace listShop;

public partial class shopList : ContentPage
{
    string cs = string.Empty;
    string random = string.Empty;
    List<string> shareList = new List<string>();
    public shopList(string cs_arg, string random_arg)
    {
        cs = cs_arg;
        random = random_arg;
        InitializeComponent();
        using var con = new MySqlConnection(cs);
        con.Open();
        string stm = "SELECT tekst, ilosc, jednostka FROM lista" + random + " WHERE zrealizowane = 0 ORDER BY kategoria, tekst";
        using var cmd = new MySqlCommand(stm, con);
        using MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            string name = rdr.GetString(0);
            int amount = rdr.GetInt32(1);
            string unit = rdr.GetString(2);

            elements.Add(new Elements.shopListElement(name, amount, unit));
            shareList.Add(name + " - " + amount + unit);
        }
        rdr.Close();
    }

    private void goBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
    public async Task ShareText(string text)
    {
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = text,
            Title = "Udostêpniona lista zakupów"
        });
    }
    private async void share_Clicked(object sender, EventArgs e)
    {
        string toShare = "Lista zakupów\n";
        for (int i = 0; i < shareList.Count; i++)
        {
            toShare += shareList[i] + "\n";
        }
        await ShareText(toShare);
    }
}