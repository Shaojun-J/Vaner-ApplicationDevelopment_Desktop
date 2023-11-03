using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for Sales.xaml
    /// </summary>
    public partial class Sales : Window
    {
        DataTable saleItems = new DataTable();
        decimal totalCost = 0;

        public Sales()
        {
            InitializeComponent();

            saleItems.Columns.Add("Product Name");
            saleItems.Columns.Add("Product ID");
            saleItems.Columns.Add("Amount(kg)");
            saleItems.Columns.Add("Price(CAD)kg");
            saleItems.Columns.Add("Total(CAD)");
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow();
            newWindow.Show();
            this.Close();
        }

        private void admin_Click(object sender, RoutedEventArgs e)
        {
            Admin newWindow = new Admin();
            newWindow.Show();
            this.Close();
        }

        private string get_ConnectionString()
        {
            string host = "host=localhost;";
            string port = "port=5432;";
            string dbName = "database=Farmers_Market;";
            string userName = "username=postgres;";
            string password = "password=328111;";
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

        private void add_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection con = null;
            try
            {
                con = establishConnection();
                con.Open();
                string Query = "select * from products where pd_name=@name";
                NpgsqlCommand cmd = new NpgsqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@name", pName.Text);
                NpgsqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    int inventoryAmount = int.Parse(dataReader["pd_amount"].ToString());
                    decimal price = decimal.Parse(dataReader["pd_price"].ToString());

                    int amount = int.Parse(pAmount.Text);
                    bool bInList = false;

                    for (int i = 0; i < saleItems.Rows.Count; i++)
                    {
                        if (saleItems.Rows[i][0].ToString().Equals(pName.Text))
                        {
                            bInList = true;
                            int listAmount = int.Parse(saleItems.Rows[i][2].ToString());
                            if (amount + listAmount <= inventoryAmount)
                            {

                                totalCost += amount * price;
                                total.Text = "$" + totalCost.ToString();
                                saleItems.Rows[i].SetField(2, (amount + listAmount));
                                saleItems.Rows[i].SetField(4, price * (amount + listAmount));
                            }
                            else
                            {
                                MessageBox.Show($"The product inventory quantity is {inventoryAmount}, and the inventory is not enough to sell {amount + listAmount}.");
                            }
                            break;
                        }
                    }

                    if (!bInList)
                    {
                        if (amount <= inventoryAmount)
                        {
                            saleItems.Rows.Add(pName.Text,
                                dataReader["pd_id"].ToString(),
                                amount,
                                price,
                                price * amount);

                            totalCost += amount * price;
                            total.Text = "$" + totalCost.ToString();

                            datagrid.ItemsSource = saleItems.AsDataView();
                            //DataContext = dataAdapter;
                        }
                        else
                        {
                            MessageBox.Show($"The product inventory quantity is {inventoryAmount}, and the inventory is not enough to sell {amount}.");
                        }
                    }

                }
                else
                {
                    MessageBox.Show($"{pName.Text} does not exist!");
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

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int amount = int.Parse(pAmount.Text);
                for (int i = 0; i < saleItems.Rows.Count; i++)
                {
                    if (pName.Text.Equals(saleItems.Rows[i][0].ToString()))
                    {
                        int pre_amount = int.Parse(saleItems.Rows[i][2].ToString());

                        amount = int.Parse(saleItems.Rows[i][2].ToString()) - amount;
                        if (amount <= 0)
                        {
                            totalCost -= pre_amount * decimal.Parse(saleItems.Rows[i][3].ToString());
                            total.Text = "$" + totalCost.ToString();
                            saleItems.Rows.RemoveAt(i);
                            break;
                        }

                        totalCost -= (pre_amount - amount) * decimal.Parse(saleItems.Rows[i][3].ToString());
                        total.Text = "$" + totalCost.ToString();
                        saleItems.Rows[i].SetField(2, amount.ToString());
                        saleItems.Rows[i].SetField(4, decimal.Parse(saleItems.Rows[i][3].ToString()) * amount);
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void checkout_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection con = null;
            NpgsqlCommand cmd = null;
            string Query = null;
            try
            {
                con = establishConnection();
                con.Open();
                for (int i = 0; i < saleItems.Rows.Count; i++)
                {
                    Query = "select * from products where pd_name=@name";
                    cmd = new NpgsqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@name", saleItems.Rows[i][0].ToString());
                    NpgsqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader != null)
                    {
                        dataReader.Read();
                        //amount = int.Parse(dataReader["pd_amount"].ToString()) - int.Parse(saleItems.Rows[i][2].ToString());
                        string pd_name = dataReader["pd_name"].ToString();
                        int pd_amount = int.Parse(dataReader["pd_amount"].ToString()) - int.Parse(saleItems.Rows[i][2].ToString());
                        int pd_id = int.Parse(dataReader["pd_id"].ToString());
                        decimal pd_price = decimal.Parse(dataReader["pd_price"].ToString());
                        dataReader.Close();
                        con?.Close();

                        con = establishConnection();
                        con.Open();
                        Query = "Update products set pd_name=@name, pd_amount=@amount, pd_price=@price where pd_id=@id;";
                        cmd = new NpgsqlCommand(Query, con);
                        cmd.Parameters.AddWithValue("@name", pd_name);
                        cmd.Parameters.AddWithValue("@amount", pd_amount);
                        cmd.Parameters.AddWithValue("@price", pd_price);
                        cmd.Parameters.AddWithValue("@id", pd_id);
                        cmd.ExecuteNonQuery();

                    }
                }

                MessageBox.Show("The total cost is $" + totalCost);
                saleItems.Clear();
                total.Text = "$0.00";
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

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectRow = datagrid.SelectedItem as DataRowView;
            if(selectRow != null) { 
                pName.Text = selectRow[0].ToString();
                pAmount.Text = selectRow[2].ToString();
            }

        }
    }
}
