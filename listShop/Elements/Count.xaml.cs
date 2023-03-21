namespace listShop.Elements;
using MySql.Data.MySqlClient;

public partial class Count : ContentView
{
    int i;
    string cs = string.Empty;
    private int id;
    string unit = string.Empty;
    string random = string.Empty;
    public Count(int id_a, string sql, string random_arg)
    {
        InitializeComponent();
        id = id_a;
        cs = sql;
        random = random_arg;
        using var con = new MySqlConnection(cs);
        con.Open();
        string stm = "SELECT ilosc, jednostka FROM lista" + random + " WHERE id = " + id;
        using var cmd = new MySqlCommand(stm, con);
        using MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            i = rdr.GetInt32(0);
            number.Text = i.ToString();
            unit = rdr.GetString(1);
            unitLabel.Text = unit;
        }
        rdr.Close();
    }
    private void updateNumber()
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        string stm = "UPDATE lista" + random + " SET ilosc = " + i + " WHERE id = " + id;
        using var cmd = new MySqlCommand(stm, con);
        cmd.ExecuteNonQuery();
        number.Text = i.ToString();
    }
    private void getNumber(object sender, EventArgs e)
    {
        if (number == null)
        {

        }
        else
        {
            i = int.Parse(number.Text);
            updateNumber();
        }
    }

    private void ButtonMinus_Clicked(object sender, EventArgs e)
    {
        if (i > 0)
            i--;
        updateNumber();
    }
    private void ButtonPlus_Clicked(object sender, EventArgs e)
    {
        i++;
        updateNumber();
    }
}