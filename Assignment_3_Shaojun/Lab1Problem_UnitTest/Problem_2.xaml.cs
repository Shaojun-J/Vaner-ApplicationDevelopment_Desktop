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
    /// Interaction logic for Problem_2.xaml
    /// </summary>
    public partial class Problem_2 : Window
    {
        public Problem_2()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(double.TryParse(textbox_weight.Text, out double weight))
            {
                /**
                 * 2 kg or less $1.10
Over 2kg but not more than 6 kg $2.20
Over 6 kg but not more than 10kg $3.70
Over 10 kg
                 */
                double totalCharges = 0;
                if(weight <= 2)
                {
                    totalCharges = weight * 1.10;
                }
                else if(weight <= 6)
                {
                    totalCharges = weight * 2.20;

                }
                else if (weight <= 10)
                {
                    totalCharges = weight * 3.70;
                }
                else
                {
                    totalCharges = weight * 4.80;
                }

                
                textBlock_total.Text = "$" + totalCharges.ToString(); 
            }
            else
            {
                MessageBox.Show("The weight of the pakage must be number not less than 0!");
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
