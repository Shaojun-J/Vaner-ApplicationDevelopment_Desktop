using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Assignment1
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {

        DataTable products = new DataTable();

        public Admin()
        {
            InitializeComponent();
            select.MouseEnter += new MouseEventHandler(select_MouseHover);

            showAll();
        }

        private string get_ConnectionString()
        {
            string host = "host=localhost;";
            string port = "port=5432;";
            string dbName = "database=Farmers_Market;";
            string userName = "username=postgres;";
            string password = "password=328111;";
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




        private void insert_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection con = null;

            try
            {
                con = establishConnection();
                con.Open();
                string Query = "insert into products values(@name, @id, @amount, @price)";
                NpgsqlCommand cmd = new NpgsqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@name", pName.Text);
                cmd.Parameters.AddWithValue("@id", int.Parse(pID.Text));
                cmd.Parameters.AddWithValue("@amount", int.Parse(pAmount.Text));
                cmd.Parameters.AddWithValue("@price", decimal.Parse(pPrice.Text));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Product insert successfully");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con?.Close();
            }

            showAll();
        }


        private void showAll()
        {
            NpgsqlConnection con = null;
            try
            {
                con = establishConnection();
                con.Open();
                string Query = "select * from products;";
                NpgsqlCommand cmd = new NpgsqlCommand(Query, con);
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);
                products.Clear();
                dataAdapter.Fill(products);
                datagrid.ItemsSource = products.AsDataView();
                DataContext = dataAdapter;

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


        private void update_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection con = null;
            try
            {

                con = establishConnection();
                con.Open();
                string Query = "Update products set pd_name=@name, pd_amount=@amount, pd_price=@price where pd_id=@id";
                NpgsqlCommand cmd = new NpgsqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@name", pName.Text);
                cmd.Parameters.AddWithValue("@amount", int.Parse(pAmount.Text));
                cmd.Parameters.AddWithValue("@price", decimal.Parse(pPrice.Text));
                cmd.Parameters.AddWithValue("@id", int.Parse(pID.Text));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Update successful");

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con?.Close();
            }
            showAll();

        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection con = null;
            try
            {
                con = establishConnection();
                con.Open();
                string Query = "delete from products where pd_id=@id";
                NpgsqlCommand cmd = new NpgsqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@id", int.Parse(pID.Text));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Delete Successful");

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con?.Close();
            }
            showAll();
        }


        private void select_MouseHover(object sender, MouseEventArgs e)
        {

            //if (!int.TryParse(pID.Text, out int id))
            //{
            //    MessageBox.Show("Please enter product ID which should be a integer.");
            //}
        }

        private void select_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection con = null;

            if (!int.TryParse(pID.Text, out int id))
            {
                MessageBox.Show("Please enter product ID which should be a integer.");
                return;
            }

            try
            {
                con = establishConnection();
                con.Open();
                string Query = "select * from products where pd_id=@Id";
                NpgsqlCommand cmd = new NpgsqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@Id", int.Parse(pID.Text));
                NpgsqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        pName.Text = dataReader["pd_name"].ToString();
                        pID.Text = dataReader["pd_id"].ToString();
                        pAmount.Text = dataReader["pd_amount"].ToString();
                        pPrice.Text = dataReader["pd_price"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show($"Product ID {pID.Text} does not exist!");
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
            showAll();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {

            MainWindow win = new MainWindow();
            win.Show();
            this.Close();

        }


        private void sales_Click(object sender, RoutedEventArgs e)
        {
            Sales sales = new Sales();
            sales.Show();
            this.Close();

        }

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectRow = datagrid.SelectedItem as DataRowView;
            if (selectRow != null)
            {
                pName.Text = selectRow[0].ToString();
                pID.Text = selectRow[1].ToString();
                pAmount.Text = selectRow[2].ToString();
                pPrice.Text = selectRow[3].ToString();
            }
        }
    }
}

