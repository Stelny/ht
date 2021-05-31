using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ht
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public string _cs = @"Server=mssql.sps-prosek.local;Database=stellja18;User ID=stellja18;Password=stellja18";
        public int lastId = 0;

        public MainWindow()
        {

            InitializeComponent();
            updateData();

        }

        void updateData()
        {

            using var _con = new SqlConnection(_cs);
            _con.Open();

            var _sql = "SELECT * FROM [dbo].[user]";

            using var _cmd = new SqlCommand(_sql, _con);

            /*string _ret = _cmd.ExecuteScalar().ToString();
            Console.WriteLine(_ret);*/

            SqlDataReader _read = _cmd.ExecuteReader();


            DataTable dt = new DataTable();

            DataColumn name = new DataColumn("Jméno");
            DataColumn surname = new DataColumn("Příjmení");
            DataColumn city = new DataColumn("Přezdívka");


            dt.Columns.Add(name);
            dt.Columns.Add(surname);
            dt.Columns.Add(city);

            while (_read.Read())
            {
                DataRow row = dt.NewRow();
                row[0] = _read.GetString(1);
                row[1] = _read.GetString(2);
                row[2] = _read.GetString(3);

                dt.Rows.Add(row);

                this.lastId = _read.GetInt32(0)+1;

            }

            myDataGrid.ItemsSource = dt.DefaultView;
            

        }


       /* void fillingData(List data)
        {
            DataTable dt = new DataTable();

            DataColumn name = new DataColumn("Jméno");
            DataColumn surname = new DataColumn("Příjmení");
            DataColumn city = new DataColumn("Město");
            DataColumn phone = new DataColumn("Mobil");


            dt.Columns.Add(name);
            dt.Columns.Add(surname);
            dt.Columns.Add(city);
            dt.Columns.Add(phone);

            foreach (var item in data)
            {
                DataRow row = dt.NewRow();
                row[0] = item.name;
                row[1] = item.surname;
                row[2] = item.city;
                row[3] = item.phone;

                dt.Rows.Add(row);
            }

            myDataGrid.ItemsSource = dt.DefaultView;
        }*/
        private void addUser(object sender, RoutedEventArgs e)
        {
            string name_db = name.Text;
            string surname_db = surname.Text;
            string username_db = username.Text;

            name.Text = "";
            surname.Text = "";
            username.Text = "";


            using var _con = new SqlConnection(_cs);
            _con.Open();

            var _sql = "INSERT INTO [dbo].[user] ([id],[name], [surname],[username]) VALUES (@id,@name,@surname,@username)";

            using var _cmd = new SqlCommand(_sql, _con);
            _cmd.Parameters.Add("@id", SqlDbType.Int).Value = lastId;
            _cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = name_db;
            _cmd.Parameters.Add("@surname", SqlDbType.VarChar).Value = surname_db;
            _cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username_db;


            var _ret = _cmd.ExecuteScalar();


            updateData();

        }
    }
}
