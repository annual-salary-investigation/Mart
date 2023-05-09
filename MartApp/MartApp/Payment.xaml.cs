using MahApps.Metro.Controls;
using System.Windows;

namespace MartApp
{
    /// <summary>
    /// Payment.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class payment : MetroWindow
    {
        public payment()
        {
            InitializeComponent();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            // 장바구니로 돌아가기
            //NavigationService.Navigate(new Uri("/ .xaml", UriKind.RelativeOrAbsolute)); 
        }

        private void BtnPayment_Click(object sender, RoutedEventArgs e)
        {
            // 결제완료 팝업창

            //this.ShowMessageAsync("결제 완료", "결제가 완료되었습니다.");
        }
    }
}
