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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab1Problem
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(radio_problem1.IsChecked == true)
            {
                BankCharges bankCharges = new BankCharges();
                bankCharges.Show();
                this.Close();
            }
            else if (radio_problem2.IsChecked == true)
            {
                Problem_2 problem2 = new Problem_2();
                problem2.Show();
                this.Close();
            }
            else if (radio_problem3.IsChecked == true)
            {
                Problem3 problem3 = new Problem3();
                problem3.Show();
                this.Close();
            }
        }
    }
}
