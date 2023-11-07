using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
    /// Interaction logic for BankCharges.xaml
    /// </summary>
    public partial class BankCharges : Window
    {
        BankAccount account;
        public BankCharges()
        {
            InitializeComponent();
            account = new BankAccount();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double balance = 0;
            int numberOfCheck = 0;
            if (!double.TryParse(textbox_balance.Text, out balance) || balance < 0)
            {
                MessageBox.Show("The account balance must be an number not less than 0!");
                return;
            }
            if (!int.TryParse(textbox_numberOfCheck.Text, out numberOfCheck) || numberOfCheck < 0)
            {
                MessageBox.Show("The number of check must be an interge not less than 0!");
                return;
            }

            account.Balance = balance;
            account.NumberOfCheck = numberOfCheck;
            textbox_total.Text = $"${account.totalServiceFee()}";

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }

    public class BankAccount
    {
        const double CHECK_FEE_LV1 = 0.10;
        const double CHECK_FEE_LV2 = 0.08;
        const double CHECK_FEE_LV3 = 0.06;
        const double CHECK_FEE_LV4 = 0.10;

        static double monthlyfee = 10;
        static double extraFee = 15;
        private double balance = 0;

        public BankAccount()
        {
            Balance = 0;
            NumberOfCheck = 0;
        }
        public BankAccount(double balance, int numberOfCheck)
        {
            Balance = balance;
            NumberOfCheck = numberOfCheck;
        }

        public int NumberOfCheck
        {
            set; private get;
        }
        public double Balance
        {
            set { this.balance = value; }
            get { return this.balance; }
        }

        public double totalServiceFee()
        {
            double totalFees = monthlyfee;

            //int num = NumberOfCheck;
            //if(num > 59)
            //{
            //    totalFees += ((num-59) * CHECK_FEE_LV4);
            //    num = 59;
            //}

            //if (num > 39)
            //{
            //    totalFees += ((num - 39) * CHECK_FEE_LV3);
            //    num = 39;
            //}

            //if (num > 19)
            //{
            //    totalFees += ((num - 19) * CHECK_FEE_LV2);
            //    num = 19;
            //}

            //totalFees += (num * CHECK_FEE_LV1);



            if (NumberOfCheck < 20)
            {
                totalFees += (NumberOfCheck * CHECK_FEE_LV1);
            }
            else if (NumberOfCheck < 40)
            {
                totalFees += (NumberOfCheck * CHECK_FEE_LV2);
            }
            else if (NumberOfCheck < 60)
            {
                totalFees += (NumberOfCheck * CHECK_FEE_LV3);
            }
            else
            {
                totalFees += (NumberOfCheck * CHECK_FEE_LV4);
            }


            if (balance - 10 < 400)
            {
                totalFees += extraFee;
            }

            return totalFees;

        }
    }
}
