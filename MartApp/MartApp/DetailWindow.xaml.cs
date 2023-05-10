using MahApps.Metro.Controls;
using mart;
using System.Windows;

namespace MartApp
{
    /// <summary>
    /// detailWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DetailWindow : MetroWindow
    {
        private int productId; // 부모창에서 넘어온 ProductID(DB 키값)

        public DetailWindow()
        {
            InitializeComponent();       
        }

        public DetailWindow(int productId)
        {
            this.productId = productId;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var cartpage = new CartWindow();            // CartWindow 장바구니창
            cartpage.Owner = this;
            cartpage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            cartpage.ShowDialog();
        }

        private void BtnBuy_Click(object sender, RoutedEventArgs e)
        {
            var cartpage = new payment();           // payment 결제창
            cartpage.Owner = this;
            cartpage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            cartpage.ShowDialog();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
