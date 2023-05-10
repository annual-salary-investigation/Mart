using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
            this.Close();
        }

        private void BtnPayment_Click(object sender, RoutedEventArgs e)
        {
            // 결제완료 팝업창

            this.ShowMessageAsync("결제 완료", "10분 후 픽업가능합니다.");
        }
    }
}
