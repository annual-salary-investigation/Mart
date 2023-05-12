using MahApps.Metro.Controls;
using MartApp;
using MartApp.Logics;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;

namespace mart
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CartWindow : MetroWindow
    {
        DataSet ds = new DataSet();
        public CartWindow()
        {
            InitializeComponent();
        }

        private void BtnMart_Click(object sender, RoutedEventArgs e)
        {
            using(MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
            }
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
