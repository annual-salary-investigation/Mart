using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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

namespace payment
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
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
            //NavigationService.Navigate(new Uri("CarWindow.xaml", UriKind.Relative)); 
            this.Close();
        }

        private void BtnPayment_Click(object sender, RoutedEventArgs e)
        {
            // 결제완료 팝업창
            
            this.ShowMessageAsync("결제 완료", "10분 후 픽업을 준비해주세요.");
        }
    }
}
