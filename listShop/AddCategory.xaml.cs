using MySql.Data.MySqlClient;
using static Google.Protobuf.Reflection.UninterpretedOption.Types;

namespace listShop;


public partial class AddCategory : ContentPage
{
    string color = string.Empty;
    string cs = string.Empty;
    string random = string.Empty;
    public AddCategory(string cs_arg, string random_arg)
    {
        cs = cs_arg;
        random = random_arg;
        InitializeComponent();
    }
    public void selectedRadio(object sender, EventArgs e)
    {
        //get value of clicked radiobutton
        RadioButton rb = (RadioButton)sender;
        color = rb.Value.ToString();
    }

    private void addNewCategory_Clicked(object sender, EventArgs e)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        //check if categoryName is not empty
        if (categoryName.Text != string.Empty && categoryName.Text != "")
        {
            //check if color is not empty
            if (color != string.Empty && color != "")
            {
                //check if categoryName is not already in database
                string stm = "SELECT * FROM kategorie" + random + " WHERE Nazwa = '" + categoryName.Text + "'";
                using var cmd = new MySqlCommand(stm, con);
                using MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    DisplayAlert("B³¹d", "Kategoria o takiej nazwie ju¿ istnieje", "OK");
                }
                else
                {
                    rdr.Close();
                    //add new category to database
                    string stm2 = "INSERT INTO kategorie" + random + " (Nazwa, KolorTla, KolorCzcionki) VALUES ('" + categoryName.Text + "', '" + color + "', 'Black')";
                    using var cmd2 = new MySqlCommand(stm2, con);
                    //if everything is ok, show alert
                    if (cmd2.ExecuteNonQuery() == 1)
                    {
                        DisplayAlert("Sukces", "Dodano now¹ kategoriê", "OK");
                        //and go back to main page

                        Navigation.PushAsync(new MainPage());
                    }
                    else
                    {
                        DisplayAlert("B³¹d", "Nie uda³o siê dodaæ kategorii", "OK");
                    }
                }
            }
            else
            {
                DisplayAlert("B³¹d", "Nie wybrano koloru", "OK");
            }
        }
        else
        {
            DisplayAlert("B³¹d", "Nazwa kategorii nie mo¿e byæ pusta", "OK");
        }
    }
}