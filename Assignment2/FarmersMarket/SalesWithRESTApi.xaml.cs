using Assignment1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
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
    /// Interaction logic for SalesWithRESTApi.xaml
    /// </summary>
    public partial class SalesWithRESTApi : Window
    {
        HttpClient client = new HttpClient();
        DataTable salesItems = new DataTable();
        decimal totalCost = 0;
        public SalesWithRESTApi()
        {
            client.BaseAddress = new Uri("https://localhost:7092/FarmersMarket/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
              new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
          );

            salesItems.Columns.Add("Product Name");
            salesItems.Columns.Add("Product ID");
            salesItems.Columns.Add("Amount(Kg)");
            salesItems.Columns.Add("Price(CAD/kg)");
            salesItems.Columns.Add("Total(CAD)");

            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           SalesWithRESTApi salesWithRESTApi = new SalesWithRESTApi();
            salesWithRESTApi.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AdminWithRESTApi adminWithRESTApi = new AdminWithRESTApi();
            adminWithRESTApi.Show();
            this.Close();
        }

        private async void add_Click(object sender, RoutedEventArgs e)
        {
            var server_response = await client.GetStringAsync($"GetFruitbyName/{pName.Text}");
            Response response_JSON = JsonConvert.DeserializeObject<Response>(server_response);
            datagrid.ItemsSource = response_JSON.products;
            DataContext = this;

            if(response_JSON.statusCode == 200)
            {
                int productInStock = response_JSON.product.pd_amount;
                decimal price = response_JSON.product.pd_price;
                int amount = int.Parse(pAmount.Text);
                bool itemsInCart = false;

                for(int i = 0; i< salesItems.Rows.Count; i++)
                {
                    if (salesItems.Rows[i][0].ToString().Equals(pName.Text) )
                    {
                        itemsInCart = true;
                        int itemAmount = int.Parse(salesItems.Rows[i][2].ToString());

                        if(amount + itemAmount < productInStock) 
                        {
                            totalCost += price * amount;
                            total.Text = "$" + totalCost.ToString();
                            salesItems.Rows[i].SetField(2, (amount + itemAmount));
                            salesItems.Rows[i].SetField(4, price * (amount + itemAmount));
                            datagrid.ItemsSource = salesItems.AsDataView();

                        }
                        else
                        {
                            MessageBox.Show($"The product in stock is {productInStock}, please enter another amount to proceed");
                        }
                        break;
                    }
                }
                if (!itemsInCart)
                {
                    if(amount <= productInStock)
                    {
                        salesItems.Rows.Add(pName.Text, response_JSON.product.pd_id,amount,price, price*amount);
                        totalCost += price * amount;
                        total.Text="$" + totalCost.ToString();
                        datagrid.ItemsSource= salesItems.AsDataView();
                    }
                    else
                    {
                        MessageBox.Show($"The product in stock is {productInStock}, please enter another amount to proceed");
                    }
                }


            }
            else
            {
                MessageBox.Show($"{pName.Text} does not exist.");
            }
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            int amount = int.Parse(pAmount.Text);
            for(int i = 0; i < salesItems.Rows.Count; i++)
            {
                if (pName.Text.Equals(salesItems.Rows[i][0].ToString())){
                    int pre_amount = int.Parse(salesItems.Rows[i][2].ToString());
                    amount = pre_amount - amount;
                    if(amount <= 0)
                    {
                        totalCost -= pre_amount * decimal.Parse(salesItems.Rows[i][3].ToString());
                        total.Text = "$" + totalCost.ToString();
                        salesItems.Rows.RemoveAt(i);
                        break;
                    }

                    totalCost -= (pre_amount - amount) * decimal.Parse(salesItems.Rows[i][3].ToString());
                    total.Text = "$" + totalCost.ToString();
                    salesItems.Rows[i].SetField(2, amount.ToString());
                    salesItems.Rows[i].SetField(4, decimal.Parse(salesItems.Rows[i][3].ToString()) * amount);
                    break;
                }
            }
        }

        private async void checkout_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0;i < salesItems.Rows.Count; i++)
            {
                string name = salesItems.Rows[i][0].ToString();
                var server_response = await client.GetStringAsync($"GetFruitbyName/{name}");
                Response response_JSON = JsonConvert.DeserializeObject<Response>(server_response);

                if(response_JSON.statusCode == 200)
                {
                    Product product = new Product();
                    product.pd_name = response_JSON.product.pd_name;
                    product.pd_price = response_JSON.product.pd_price;
                    product.pd_amount = response_JSON.product.pd_amount - int.Parse(salesItems.Rows[i][2].ToString());
                    product.pd_id = response_JSON.product.pd_id;

                }
            }

            MessageBox.Show("The total cost is $" + totalCost);
            salesItems.Clear();
            total.Text = "$0.00";
        }

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectRow = datagrid.SelectedItem as DataRowView;
            if(selectRow != null)
            {
                pName.Text = selectRow[0].ToString();
                pAmount.Text = selectRow[2].ToString();

            }
        }
    }
}
