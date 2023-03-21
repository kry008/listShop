using MySql.Data.MySqlClient;
//using System.Data;

namespace listShop.Elements;


public partial class Category : ContentView
{
    int id = 0;
    string cs = string.Empty;
    bool open = false;
    string random = string.Empty;

    public Category(int id_a, string sql, string random_qrg)
    {
        InitializeComponent();
        id = id_a;
        cs = sql;
        random = random_qrg;
        //get from table name of category and set name to it
        using var con = new MySqlConnection(cs);
        string stm = "SELECT * FROM kategorie" + random + " WHERE id = " + id;
        con.Open();
        using var cmd = new MySqlCommand(stm, con);
        using MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            name.Text = rdr.GetString("Nazwa");
            if (rdr.IsDBNull(rdr.GetOrdinal("KolorTla")))
            {
                name.BackgroundColor = Color.Parse("White");
                plusBtn.BackgroundColor = Color.Parse("White");
                delete.BackgroundColor = Color.Parse("White");
            }
            else
            {
                name.BackgroundColor = Color.Parse(rdr.GetString("KolorTla"));
                plusBtn.BackgroundColor = Color.Parse(rdr.GetString("KolorTla"));
                delete.BackgroundColor = Color.Parse(rdr.GetString("KolorTla"));
            }
            if (rdr.IsDBNull(rdr.GetOrdinal("KolorCzcionki")))
            {
                name.TextColor = Color.Parse("Black");
                plusBtn.TextColor = Color.Parse("Black");
                delete.TextColor = Color.Parse("Black");
            }
            else
            {
                name.TextColor = Color.Parse(rdr.GetString("KolorCzcionki"));
                plusBtn.TextColor = Color.Parse(rdr.GetString("KolorCzcionki"));
                delete.TextColor = Color.Parse(rdr.GetString("KolorCzcionki"));
            }
            if (rdr.GetValue(4).ToString() == "1")
            {
                openingCat();
            }

        }
        rdr.Close();
    }
    private void openingCat()
    {
        elementy.Clear();
        if (!open)
        {
            using var con = new MySqlConnection(cs);
            con.Open();
            string stm = "SELECT * FROM lista" + random + " WHERE kategoria = " + id + " ORDER BY zrealizowane, tekst";
            using var cmd = new MySqlCommand(stm, con);
            using MySqlDataReader rdr = cmd.ExecuteReader();
            int i = 0;
            while (rdr.Read())
            {
                elementy.Add(new Elements.ElementList(rdr.GetInt32(0), cs, rdr.GetInt32(3), random));
                i++;
            }
            if (i == 0)
            {
                elementy.Add(new Label { Text = "Brak elementów", TextColor = Color.Parse("Black") });
            }
        }
        open = !open;

        using var con2 = new MySqlConnection(cs);
        con2.Open();
        string stm2 = "UPDATE kategorie" + random + " SET Otwarta = " + (open ? 1 : 0) + " WHERE id = " + id;
        using var cmd2 = new MySqlCommand(stm2, con2);
        cmd2.ExecuteNonQuery();
        con2.Close();
    }
    private void name_Clicked(object sender, EventArgs e)
    {
        openingCat();
    }
    private void delete_Clicked(object sender, EventArgs e)
    {
        using var con = new MySqlConnection(cs);
        //delete all from lista where kategoria = id
        con.Open();
        string stm = "DELETE FROM lista" + random + " WHERE kategoria = " + id;
        using var cmd = new MySqlCommand(stm, con);
        cmd.ExecuteNonQuery();
        //delete from kategorie where id = id
        stm = "DELETE FROM kategorie" + random + " WHERE id = " + id;
        using var cmd2 = new MySqlCommand(stm, con);
        cmd2.ExecuteNonQuery();
        con.Close();
        Navigation.PushAsync(new MainPage());

    }
    public void reloadList()
    {
        elementy.Clear();
        using var con = new MySqlConnection(cs);
        con.Open();
        string stm = "SELECT * FROM lista" + random + " WHERE kategoria = " + id + " ORDER BY zrealizowane, tekst";
        using var cmd = new MySqlCommand(stm, con);
        using MySqlDataReader rdr = cmd.ExecuteReader();
        int i = 0;
        while (rdr.Read())
        {
            elementy.Add(new Elements.ElementList(rdr.GetInt32(0), cs, rdr.GetInt32(3), random));
            i++;
        }
        if (i == 0)
        {
            elementy.Add(new Label { Text = "Brak elementów", TextColor = Color.Parse("Black") });
        }
    }

    private void plusBtn_Clicked(object sender, EventArgs e)
    {
        string name_arg = name.Text;
        Navigation.PushAsync(new AddElementList(id, name_arg, cs, random));
    }
}