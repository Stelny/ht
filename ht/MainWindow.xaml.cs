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
        public int editId = 0;

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

            SqlDataReader _read = _cmd.ExecuteReader();

            List<User> users = new List<User>();

            while (_read.Read())
            {
                User user = new User();

                user.id = _read.GetInt32(0);
                user.name = _read.GetString(1);
                user.surname = _read.GetString(2);
                user.username = _read.GetString(3);

                users.Add(user);
            }

            DG1.DataContext = users;


        }

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

            var type = ((Button)sender).Tag;
            if ((string)type == "edit")
            {
                var _sql = "UPDATE [dbo].[user] SET [name] = @name, [surname] = @surname, [username] = @username WHERE [id] = @id";

                using var _cmd = new SqlCommand(_sql, _con);
                _cmd.Parameters.Add("@id", SqlDbType.Int).Value = editId;
                _cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = name_db;
                _cmd.Parameters.Add("@surname", SqlDbType.VarChar).Value = surname_db;
                _cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username_db;
                var _ret = _cmd.ExecuteScalar();

            }
            else
            {
                var _sql = "INSERT INTO [dbo].[user] ([id],[name], [surname],[username]) VALUES (@id,@name,@surname,@username)";

                using var _cmd = new SqlCommand(_sql, _con);
                _cmd.Parameters.Add("@id", SqlDbType.Int).Value = lastId;
                _cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = name_db;
                _cmd.Parameters.Add("@surname", SqlDbType.VarChar).Value = surname_db;
                _cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username_db;

                var _ret = _cmd.ExecuteScalar();
            }
            update.Content = "Přidat";
            update.Tag = "insert";

            updateData();

        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            var id = ((Button)sender).Tag;
            this.editId = (int)id;

            fillUpdateData((int)id);

        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            int id = (int)((Button)sender).Tag;

            using var _con = new SqlConnection(_cs);
            _con.Open();

            var _sql = "DELETE FROM [dbo].[user] WHERE id = @id";
            using var _cmd = new SqlCommand(_sql, _con);
            _cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            var _ret = _cmd.ExecuteScalar();

            updateData();


        }


        protected void fillUpdateData(int id)
        {
            using var _con = new SqlConnection(_cs);
            _con.Open();

            var _sql = "SELECT * FROM [dbo].[user] WHERE [id] = @id";

            using var _cmd = new SqlCommand(_sql, _con);
            _cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            SqlDataReader _read = _cmd.ExecuteReader();
            

            while (_read.Read())
            {
                name.Text = _read.GetString(1);
                surname.Text = _read.GetString(2);
                username.Text = _read.GetString(3);
                update.Content = "Upravit";
                update.Tag = "edit";
            }
            

        
        }


        public class User {
            public int id { get; set; }
            public string name { get; set; }
            public string surname { get; set; }
            public string username { get; set; }

        }
    }
}
