using MahApps.Metro.Controls;
using MartApp;
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

namespace mart
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CartWindow : MetroWindow
    {
        public CartWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BtnMart_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnPayment_Click(object sender, RoutedEventArgs e)
        {
            var Payment = new payment();
            Payment.Owner = this;
            Payment.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Payment.ShowDialog();
        }
    }
}
