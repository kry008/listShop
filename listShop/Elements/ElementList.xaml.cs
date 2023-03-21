namespace listShop.Elements;
using MySql.Data.MySqlClient;

public partial class ElementList : ContentView
{
    string cs = string.Empty;
    int id = 0;
    int done = 0;
    string random = string.Empty;
    public ElementList(int id_a, string sql, int done_a, string random_arg)
    {

        InitializeComponent();
        cs = sql;
        id = id_a;
        random = random_arg;
        counter.Add(new Count(id, cs, random));
        text.Text = id_a.ToString();
        done = done_a;
        if (done == 1)
        {
            checkedBtn.Text = "☑️";
            ele.BackgroundColor = Color.FromArgb("#44444444");
        }
        using var con = new MySqlConnection(cs);
        con.Open();
        string stm = "SELECT tekst FROM lista" + random + " WHERE id = " + id;
        using var cmd = new MySqlCommand(stm, con);
        using MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            text.Text = rdr.GetString(0);
        }
        rdr.Close();
    }

    private void checkedBtn_Clicked(object sender, EventArgs e)
    {
        checkedBtn.Text = "☑️";
        using var con = new MySqlConnection(cs);
        con.Open();
        string stm = "UPDATE lista" + random + "  SET zrealizowane = 1 WHERE id = " + id;
        using var cmd = new MySqlCommand(stm, con);
        cmd.ExecuteNonQuery();
        con.Close();
        Navigation.PushAsync(new MainPage());

    }
    private void usun_Btn(object sender, EventArgs e)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        string stm = "DELETE FROM lista" + random + "  WHERE id = " + id;
        using var cmd = new MySqlCommand(stm, con);
        cmd.ExecuteNonQuery();
        con.Close();
        text.Text = "";
        ele.HeightRequest = 0;
    }
}