using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace CarRental
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        

        private string get_ConnectionString()
        {
            string host = "host=localhost;";
            string port = "port=5432;";
            string dbName = "database=vanierdb;";
            string userName = "username=postgres;";
            string password = "password=shaojun123;";
            //string connectionString = string.Format("{0}{1}{2}{3}{4}", host, port, dbName, userName, password);
            string connectionString = $"{host} {port} {dbName} {userName} {password}";
            return connectionString;
        }

        private NpgsqlConnection establishConnection()
        {
            NpgsqlConnection con = null;
            try
            {
                con = new NpgsqlConnection(get_ConnectionString());
                //MessageBox.Show("Connection established.");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return con;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (userName.Text.Length == 0)
            {
                MessageBox.Show("Please enter user name.");
                return;
            }
            else if ( password.Password.Length <= 0)
            {
                MessageBox.Show("Please enter password.");
                return;
            }

            NpgsqlConnection con = null;
            try
            {

                
                con = establishConnection();
                con.Open();
                con.GetSchema();
                string table = customer.IsChecked==true ? "customer" : "staff";
                string Query = $"select * from car_rental.{table} where username=@uname;";
                NpgsqlCommand cmd = new NpgsqlCommand(Query, con);
                
                cmd.Parameters.AddWithValue("@uname", userName.Text.ToString());
                NpgsqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    {
                        string salt = dataReader["salt"].ToString();
                        string pwdHash = dataReader["password"].ToString();
                        pwdHash = MyUtility.getMD5(pwdHash + salt);

                        string pwdInput = password.Password;
                        pwdInput = MyUtility.getMD5(MyUtility.getMD5(pwdInput+MyUtility.SALT) + salt);
                        if (pwdHash.Equals(pwdInput))
                        {
                            MessageBox.Show("Login sucessfull");
                        }
                        else
                        {
                            MessageBox.Show("Login failed. Incorrect passwrod! ");
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"User name does not exist!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con?.Close();
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Signup signup = new Signup();
            signup.Show();
            this.Close();
        }

        private void staff_Checked(object sender, RoutedEventArgs e)
        {
            if (staff.IsChecked == true)
            {
                //btn_creat.Visibility = Visibility.Collapsed;
            }
            else
            {
                //btn_creat.Visibility = Visibility.Visible;
            }
        }

        private void customer_Checked(object sender, RoutedEventArgs e)
        {
            if (customer.IsChecked == true)
            {
                //btn_creat.Visibility= Visibility.Visible;
            }
            else
            {
                //btn_creat.Visibility = Visibility.Collapsed;
            }
        }
    }
}
