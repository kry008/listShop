using MySql.Data.MySqlClient;
using static Google.Protobuf.Reflection.UninterpretedOption.Types;

namespace listShop;

public partial class AddElementList : ContentPage
{
    string cs = string.Empty;
    int id = 0;
    string name = string.Empty;
    string unit = string.Empty;
    string random = string.Empty;
    public AddElementList(int id_g, string name_g, string cs_g, string random_arg)
    {
        InitializeComponent();
        catName.Text = name_g;
        cs = cs_g;
        id = id_g;
        name = name_g;
        random = random_arg;
    }
    private void selectedRadio(object sender, EventArgs e)
    {
        RadioButton rb = (RadioButton)sender;
        unit = rb.Value.ToString();
    }

    private void addNewItem_Clicked(object sender, EventArgs e)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        //chceck if listName is not null
        if (listName.Text != null)
        {
            //check if listName is not empty
            if (listName.Text != "")
            {
                //check if listName is not only spaces
                if (listName.Text.Trim() != "")
                {
                    //check if listName is not already in database
                    string stm = "SELECT * FROM lista" + random + " WHERE tekst = '" + listName.Text + "' AND kategoria = " + id;
                    using var cmd = new MySqlCommand(stm, con);
                    using MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        DisplayAlert("B³¹d", "Taki element ju¿ istnieje", "OK");
                    }
                    else
                    {
                        if (unit == string.Empty || unit == "")
                        {
                            DisplayAlert("B³¹d", "Wybierz jednostke", "OK");
                        }
                        else
                        {
                            //add new item to database
                            using var con2 = new MySqlConnection(cs);
                            con2.Open();
                            string stm2 = "INSERT INTO lista" + random + " (tekst, ilosc, zrealizowane, kategoria, jednostka) VALUES ('" + listName.Text + "', 1, 0, " + id + ", '" + unit + "')";
                            using var cmd2 = new MySqlCommand(stm2, con2);
                            cmd2.ExecuteNonQuery();
                            DisplayAlert("Sukces", "Dodano nowy element do listy", "OK");
                            listName.Text = "";
                            Navigation.PushAsync(new MainPage());
                        }
                    }
                }
                else
                {
                    DisplayAlert("B³¹d", "Nazwa nie mo¿e byæ pusta", "OK");
                }
            }
            else
            {
                DisplayAlert("B³¹d", "Nazwa nie mo¿e byæ pusta", "OK");
            }
        }
        else
        {
            DisplayAlert("B³¹d", "Nazwa nie mo¿e byæ pusta", "OK");
        }

    }
}