using Npgsql;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

namespace CarRental
{
    /// <summary>
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class Signup : Window
    {
        public Signup()
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

        
        private bool checkInfor()
        {
            Regex regex = null;
            bool bErr = false;
            if (tb_userName.Text.Length < 3)
            {
                lb_username.Foreground = new SolidColorBrush(Colors.Red);
                bErr = true;
            }
            else
            {
                lb_username.Foreground = new SolidColorBrush(Colors.Black);
            }

            string firstName = tb_firstName.Text;
            if (firstName.Length <= 0)
            {
                lb_firstname.Foreground = new SolidColorBrush(Colors.Red);
                bErr = true;
            }
            else
            {
                lb_firstname.Foreground = new SolidColorBrush(Colors.Black);
            }

            string lastName = tb_lastName.Text;
            if (lastName.Length <= 0)
            {
                lb_lastname.Foreground = new SolidColorBrush(Colors.Red);
                bErr = true;
            }
            else
            {
                lb_lastname.Foreground = new SolidColorBrush(Colors.Black);
            }

            string phone = tb_phone.Text; //"727-190-9103";
            //regex = new Regex(@"^[1-9]\d*$ ");
            regex = new Regex(@"^(\d{3})-(\d){3}-(\d{4})$");
            if (phone.Length != 12 || !regex.IsMatch(phone))
            {
                lb_phone.Foreground = new SolidColorBrush(Colors.Red);
                bErr = true;
            }
            else
            {
                lb_phone.Foreground = new SolidColorBrush(Colors.Black);
            }

            string email = tb_email.Text; //tb_email.Text; //"jgodber1@1und1.de";
            regex = new Regex(@"^(\w)+(\.\w)*@(\w)+((\.\w+)+)$");
            if (email.Length <= 0 || (!regex.IsMatch(email)))
            {
                lb_email.Foreground = new SolidColorBrush(Colors.Red);
                bErr = true;
            }
            else
            {
                lb_email.Foreground = new SolidColorBrush(Colors.Black);
            }


            if (tb_licenseId.Text.Length <= 0)
            {
                lb_licenseid.Foreground = new SolidColorBrush(Colors.Red);
                bErr = true;
            }
            else
            {
                lb_licenseid.Foreground = new SolidColorBrush(Colors.Black);
            }

            //regex = new Regex(@"^(\d{4})-(\d{2})-(\d{2})$");
            //Match m = regex.Match(tb_issuedate.Text);
            //Console.WriteLine(m.Groups[1].Value);
            //Console.WriteLine(m.Groups[2].Value);

            bool bResult = DateTime.TryParse(tb_issuedate.Text, out DateTime issueDateTime);
            if (!bResult)
            {
                lb_issuedate.Foreground = new SolidColorBrush(Colors.Red);
                bErr = true;
            }
            else
            {
                lb_issuedate.Foreground = new SolidColorBrush(Colors.Black);
            }

            //regex = new Regex(@"^(\d){4}-(\d){2}-((\d){2})$");
            bResult = DateTime.TryParse(tb_expiredate.Text, out DateTime expireDateTime);
            if (tb_expiredate.Text.Length != 10 || !regex.IsMatch(tb_expiredate.Text))
            {
                lb_expiredate.Foreground = new SolidColorBrush(Colors.Red);
                bErr = true;
            }
            else
            {
                lb_expiredate.Foreground = new SolidColorBrush(Colors.Black);
            }

            if(expireDateTime <= issueDateTime)
            {
                lb_expiredate.Foreground = new SolidColorBrush(Colors.Red);
                lb_issuedate.Foreground = new SolidColorBrush(Colors.Red);
                bErr = true;
                //MessageBox.Show("date err");
            }

            if (tb_type.Text.Length <= 0)
            {
                lb_type.Foreground = new SolidColorBrush(Colors.Red);
                bErr = true;
            }
            else
            {
                lb_type.Foreground = new SolidColorBrush(Colors.Black);
            }


            if (tb_street.Text.Length <= 0)
            {
                lb_street.Foreground = new SolidColorBrush(Colors.Red);
                bErr = true;
            }
            else
            {
                lb_street.Foreground = new SolidColorBrush(Colors.Black);
            }
            if (tb_city.Text.Length <= 0)
            {
                lb_city.Foreground = new SolidColorBrush(Colors.Red);
                bErr = true;
            }
            else
            {
                lb_city.Foreground = new SolidColorBrush(Colors.Black);
            }
            if (tb_province.Text.Length <= 0)
            {
                lb_province.Foreground = new SolidColorBrush(Colors.Red);
                bErr = true;
            }
            else
            {
                lb_province.Foreground = new SolidColorBrush(Colors.Black);
            }
            if (tb_postalcode.Text.Length <= 0)
            {
                lb_postalcode.Foreground = new SolidColorBrush(Colors.Red);
                bErr = true;
            }
            else
            {
                lb_postalcode.Foreground = new SolidColorBrush(Colors.Black);
            }

            return bErr;

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection con = null;

            /*
             insert into car_rental.customer (first_name, last_name, phone, email, driver_license, address, username, password, salt)
                values ('First_Name', 'Last_Name', '727-190-9103', 'jgodber1@1und1.de', '{"license_id" : "license_id", "issue_date":"issuedate",
                "expire_date":"expiredate", "type":"type"}', '{"street" : "Street", "city":"city", "tb_province":"Province",
                "Postal_code":"Postal code"}', 'userName', '12345678', 'addf')
             */

            if (checkInfor())
            {
                MessageBox.Show("Please enter corect infor. The RED information is incorrect!");
                return;
            }

            try
            {
                bool bErr = false;
                string username = tb_userName.Text;

                string firstName = tb_firstName.Text;

                string lastName = tb_lastName.Text;

                string phone = tb_phone.Text; //"727-190-9103";

                string email = tb_email.Text; //tb_email.Text; //"jgodber1@1und1.de"


                var licenseObj = new
                {
                    license_id = tb_licenseId.Text,
                    issue_date = tb_issuedate.Text,
                    expire_date = tb_expiredate.Text,
                    type = tb_type.Text,
                };


                var license = JsonSerializer.Serialize(licenseObj);
                Console.WriteLine(license);
                var addressObj = new
                {
                    street = tb_street.Text,
                    city = tb_city.Text,
                    tb_province = tb_province.Text,
                    Postal_code = tb_postalcode.Text,
                };
                var address = JsonSerializer.Serialize(addressObj);

                string userName = tb_userName.Text;
                string password = MyUtility.getMD5(tb_password.Password + MyUtility.SALT); // tb_password.Password;
                string salt = MyUtility.getMD5(DateTime.Now.ToString());

                con = establishConnection();
                con.Open();
                string Query = $"insert into car_rental.customer (first_name, last_name, phone, email, driver_license, address, username, password, salt) " +
                    $"values('{firstName}', '{lastName}', '{phone}', '{email}', '{license}', '{address}', '{userName}', '{password}', '{salt}')";
                NpgsqlCommand cmd = new NpgsqlCommand(Query, con);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Register successfully");
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
    }
}
