using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Lab1Problem
{
    /// <summary>
    /// Interaction logic for Problem3.xaml
    /// </summary>
    public partial class Problem3 : Window
    {
        public Problem3()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(double.TryParse(tb_initPopulation.Text, out var initPopulation) && initPopulation >= 2)
            {
                if(double.TryParse(tb_rate.Text, out var rate) && rate >0)
                {
                    rate /= 100;
                    int days = int.Parse(comb_days.Text);
                    tb_output.Text = "Predict output:";
                    for (int i = 0; i<days; i++)
                    {
                        initPopulation = initPopulation + (initPopulation * rate);
                        tb_output.Text += $"\n\tThe population after {i+1} day are: {(int)initPopulation}";
                    }
                    
                }
                else
                {
                    MessageBox.Show("The increas rate must be numbe not less than 0!");
                }
            }
            else
            {
                MessageBox.Show("The init population must be number not less than 2!");

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
