using Assignment1.Models;
using Newtonsoft.Json;
using Npgsql;
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

namespace Assignment1
{
    /// <summary>
    /// Interaction logic for AdminWithRESTApi.xaml
    /// </summary>
    public partial class AdminWithRESTApi : Window
    {
        HttpClient client = new HttpClient();

        public AdminWithRESTApi()
        {
            //set up the base address for created RESTAPI
            client.BaseAddress = new Uri("https://localhost:7092/FarmersMarket/");

            //clear the default network packet header info
            client.DefaultRequestHeaders.Accept.Clear();

            //add packet info to http request
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")  
            );

            InitializeComponent();
        }


        private async void showAll()
        {
            var server_response = await client.GetStringAsync("GetAllFruits"); // call the method to get all fruits
            Response response_JSON = JsonConvert.DeserializeObject<Response>(server_response);  //Decrypt/extract the server_response
            datagrid.ItemsSource = response_JSON.products;
            DataContext = this;
        }

        private async void select_Click(object sender, RoutedEventArgs e)
        {
            int id;
            if(!int.TryParse(pID.Text, out id))
            {
                MessageBox.Show("Please enter an valid product ID");
                return;
            }
            var server_response = await client.GetStringAsync("GetFruitbyId/" + int.Parse( pID.Text));
            Response response_JSON = JsonConvert.DeserializeObject<Response>(server_response);
            
            if(response_JSON.product != null)
            {
                pName.Text = response_JSON.product.pd_name;
                pID.Text = response_JSON.product.pd_id.ToString();
                pAmount.Text = response_JSON.product.pd_amount.ToString();
                pPrice.Text = response_JSON.product.pd_price.ToString();
            }
            else
            {
                MessageBox.Show($"The product id {pID.Text} doesn't exsit.");
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

            var server_response = await client.PostAsJsonAsync("AddFruit", product); //here need add package: Microsoft.AspNet.WebApi.Client in order to use PostAsJsonAsync
            //Response response_JSON = JsonConvert.DeserializeObject<Response>(server_response.ToString());

            MessageBox.Show(server_response.ToString());

            showAll();
        }

        private async void update_Click(object sender, RoutedEventArgs e)
        {
            Product product = new Product();
            product.pd_name = pName.Text;
            product.pd_id = int.Parse(pID.Text);
            product.pd_amount = int.Parse(pAmount.Text);
            product.pd_price = decimal.Parse(pPrice.Text);

            var server_response = await client.PutAsJsonAsync("UpdateFruit", product);
            MessageBox.Show(server_response.ToString());
            showAll();

        }

        private async void delete_Click(object sender, RoutedEventArgs e)
        {
            int id;
            if(!int.TryParse(pID.Text, out id))
            {
                MessageBox.Show("Please enter a valid product ID");
                return;
            }
            var server_response = await client.DeleteAsync("DeleteFruitById/"+ id);
            MessageBox.Show(server_response.StatusCode.ToString());

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Sales sales = new Sales();
            sales.Show();
            this.Close();
        }
    }
}
