using Newtonsoft.Json;
using FarmersMarket.Models;
using System;
using System.Collections.Generic;
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
using System.Data;


namespace FarmersMarket
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        HttpClient client = new HttpClient();
        public Admin()
        {
            client.BaseAddress = new Uri("https://localhost:7173/controller/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );

            InitializeComponent();
            showAll();
        }

        private async void showAll()
        {
            var server_response = await client.GetStringAsync("GetAllProducts");
            Response response_Json = JsonConvert.DeserializeObject<Response>(server_response);
            datagrid.ItemsSource = response_Json.products;
            DataContext = this;
        }

        private async void select_Click(object sender, RoutedEventArgs e)
        {
            int id;
            if (!int.TryParse(pID.Text, out id))
            {
                MessageBox.Show("Please enter product ID which should be a integer.");
                return;
            }

            var server_response = await client.GetStringAsync($"GetProductbyId/{id}");
            Response response_Json = JsonConvert.DeserializeObject<Response>(server_response);
            if (response_Json.product != null)
            {
                pName.Text = response_Json.product.pd_name;
                pID.Text = response_Json.product.pd_id.ToString();
                pAmount.Text = response_Json.product.pd_amount.ToString();
                pPrice.Text = response_Json.product.pd_price.ToString();
            }
            else
            {
                MessageBox.Show($"Product ID {pID.Text} does not exist!");
            }

            showAll();
        }

        private async void insert_Click(object sender, RoutedEventArgs e)
        {
            Product product = new Product();
            product.pd_name = pName.Text;
            product.pd_id = int.Parse(pID.Text);
            product.pd_amount = int.Parse(pAmount.Text);
            product.pd_price = decimal.Parse(pPrice.Text);

            var server_response = await client.PostAsJsonAsync("AddProduct", product);
            
            if(server_response.IsSuccessStatusCode)
            {
                MessageBox.Show("Insertion successful");
            }
            else
            {
                MessageBox.Show("Insertion failed");
            }
            //MessageBox.Show(server_response.StatusCode.ToString());
            //Response res = JsonConvert.DeserializeObject<Response>(server_response);
            //if(res.statusCode == 200)
            //{
            //    MessageBox.Show("Insertion successful");
            //}
            //else
            //{
            //    MessageBox.Show("Insertion failed");
            //}

            showAll();
        }

        private async void update_Click(object sender, RoutedEventArgs e)
        {
            Product product = new Product();
            product.pd_name = pName.Text;
            product.pd_id = int.Parse(pID.Text);
            product.pd_amount = int.Parse(pAmount.Text);
            product.pd_price = decimal.Parse(pPrice.Text);

            var server_response = await client.PutAsJsonAsync($"UpdateProduct", product);
            if (server_response.IsSuccessStatusCode)
            {
                MessageBox.Show("update successfully");
            }
            else
            {
                MessageBox.Show("failed to update");
            }

            showAll();
        }

        private async void delete_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(pID.Text);
            var server_response = await client.DeleteAsync($"DeleteProductbyId/{id}");
            Console.WriteLine("statusCode:" + server_response.StatusCode);
            if(server_response.IsSuccessStatusCode)
            {
                MessageBox.Show("deleted successfully");
            }
            else
            {
                MessageBox.Show("failed to delete");
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
            Product product = datagrid.SelectedItem as Product;
            if (product != null)
            {
                pName.Text = product.pd_name;
                pID.Text = product.pd_id.ToString();
                pAmount.Text = product.pd_amount.ToString();
                pPrice.Text = product.pd_price.ToString();
            }
        }
    }
}
