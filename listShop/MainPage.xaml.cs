namespace listShop;
using MySql.Data.MySqlClient;
using static Google.Protobuf.Reflection.UninterpretedOption.Types;

public partial class MainPage : ContentPage
{
    private static string serverHost = "localhost";
    private static string login = "root";
    private static string pass = "";
    private static string db = "db";
    string cs = "server=" + serverHost + ";userid="+login+ ";password="+pass+ ";database="+db;
    string random = string.Empty;

    public MainPage()
    {
        InitializeComponent();
        string path = Path.Combine(FileSystem.AppDataDirectory, "random");
        if (File.Exists(path))
        {
            using (StreamReader sr = File.OpenText(path))
            {
                random = sr.ReadLine();
            }
        }
        else
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                string[] tabForRandom = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "w", "x", "y", "z", "q", "v", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "W", "X", "Y", "Z" };
                Random rnd = new Random();
                for (int i = 0; i < 14; i++)
                {
                    random += tabForRandom[rnd.Next(0, tabForRandom.Length)];
                }
                sw.WriteLine(random);
                using var con = new MySqlConnection(cs);
                con.Open();
                string stm = "CREATE TABLE `kategorie" + random + "` ( `id` INT(11) NOT NULL AUTO_INCREMENT, `Nazwa` TEXT NOT NULL COLLATE 'utf8mb4_general_ci', `KolorTla` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci', `KolorCzcionki` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci', `Otwarta` INT(1) NOT NULL DEFAULT '0', INDEX `Indeks 1` (`id`) USING BTREE ) COLLATE='utf8mb4_general_ci' ENGINE=InnoDB AUTO_INCREMENT=1 ;";
                using var cmd = new MySqlCommand(stm, con);
                cmd.ExecuteNonQuery();
                stm = "CREATE TABLE `lista" + random + "` ( `id` INT(11) NOT NULL AUTO_INCREMENT, `tekst` TEXT NOT NULL COLLATE 'utf8mb4_general_ci', `ilosc` INT(11) NOT NULL DEFAULT '1', `zrealizowane` INT(11) NOT NULL, `kategoria` INT(11) NOT NULL, `jednostka` TINYTEXT NULL DEFAULT 'szt' COLLATE 'utf8mb4_general_ci', INDEX `Indeks 1` (`id`) USING BTREE, INDEX `kategorie"+random+"` (`kategoria`) USING BTREE, CONSTRAINT `kategorie"+random+"` FOREIGN KEY (`kategoria`) REFERENCES `kategorie" + random+"` (`id`) ON UPDATE RESTRICT ON DELETE RESTRICT ) COLLATE='utf8mb4_general_ci' ENGINE=InnoDB AUTO_INCREMENT=1 ;";
                cmd.CommandText = stm;
                cmd.ExecuteNonQuery();
                
                stm = "INSERT INTO `kategorie" + random + "` (`Nazwa`, `KolorTla`, `KolorCzcionki`) VALUES ('Nabiał', 'LightBlue', 'Black'), ('Mięso', 'Red', 'Black'), ('Warzywa', 'LightGreen', 'Black'), ('Owoce', 'Yellow', 'Black'), ('Napoje', 'Orange', 'Black'), ('Przyprawy', 'LightBlue', 'Black'), ('Słodycze', 'Red', 'Black'), ('Inne', 'LightGreen', 'Black');";
                cmd.CommandText = stm;
                cmd.ExecuteNonQuery();
                con.Close();

            }
        }
        loadAllCat();
    }
    private void loadAllCat()
    {
        elements.Clear();
        using var con = new MySqlConnection(cs);
        con.Open();
        string stm = "SELECT * FROM kategorie"+random;
        using var cmd = new MySqlCommand(stm, con);
        using MySqlDataReader rdr = cmd.ExecuteReader();
        //create a list of categories
        while (rdr.Read())
        {
            elements.Add(new Elements.Category(rdr.GetInt32(0), cs, random));
            noneCat.IsVisible = false;
        }
        rdr.Close();
    }

    private void addCategory_clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddCategory(cs, random));
    }
    private void shopList_clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new shopList(cs, random));
    }
}