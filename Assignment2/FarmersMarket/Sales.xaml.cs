using FarmersMarket.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting;
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

namespace FarmersMarket
{
    /// <summary>
    /// Interaction logic for Sales.xaml
    /// </summary>
    public partial class Sales : Window
    {
        HttpClient client = new HttpClient();
        DataTable saleItems = new DataTable();
        decimal totalCost = 0;

        public Sales()
        {
            client.BaseAddress = new Uri("https://localhost:7173/controller/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );

            saleItems.Columns.Add("Product Name");
            saleItems.Columns.Add("Product ID");
            saleItems.Columns.Add("Amount(kg)");
            saleItems.Columns.Add("Price(CAD)kg");
            saleItems.Columns.Add("Total(CAD)");

            InitializeComponent();


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

        private async void add_Click(object sender, RoutedEventArgs e)
        {


            var server_response = await client.GetStringAsync($"GetProductbyName/{pName.Text}");
            Response response_Json = JsonConvert.DeserializeObject<Response>(server_response);
            datagrid.ItemsSource = response_Json.products;
            DataContext = this;

            if (response_Json.statusCode == 200)
            {
                int inventoryAmount = response_Json.product.pd_amount;
                decimal price = response_Json.product.pd_price;

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
                            datagrid.ItemsSource = saleItems.AsDataView();
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
                            response_Json.product.pd_id,
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

        private void remove_Click(object sender, RoutedEventArgs e)
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

        private async void checkout_Click(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i < saleItems.Rows.Count; i++)
            {
                string name = saleItems.Rows[i][0].ToString();
                var server_response = await client.GetStringAsync($"GetProductbyName/{name}");
                Response res_Json = JsonConvert.DeserializeObject<Response>(server_response);

                if (res_Json.statusCode == 200)
                {
                    Product product = new Product();
                    product.pd_name = res_Json.product.pd_name;
                    product.pd_amount = res_Json.product.pd_amount - int.Parse(saleItems.Rows[i][2].ToString());
                    product.pd_id = res_Json.product.pd_id;
                    product.pd_price = res_Json.product.pd_price;                    
                    var update_response = await client.PutAsJsonAsync($"UpdateProduct", product);
                    if(update_response.IsSuccessStatusCode)
                    {

                    }
                    else
                    {

                    }
                    
                    
                }
            }

            MessageBox.Show("The total cost is $" + totalCost);
            saleItems.Clear();
            total.Text = "$0.00";
        }

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectRow = datagrid.SelectedItem as DataRowView;
            if (selectRow != null)
            {
                pName.Text = selectRow[0].ToString();
                pAmount.Text = selectRow[2].ToString();
            }
        }
    }
}
