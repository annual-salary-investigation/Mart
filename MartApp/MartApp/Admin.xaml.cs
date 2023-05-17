using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace MartApp
{
    /// <summary>
    /// admin.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Admin : MetroWindow
    {
        public Admin()
        {
            InitializeComponent();
        }

        // 주문내역 버튼 누르면 페이지 이동
        private void BtnOrderList_Click(object sender, RoutedEventArgs e)
        {
            adminPage.Source = new Uri("OrderList.xaml", UriKind.Relative);
        }

        // 회원조회 버튼 누르면 페이지 이동
        private void BtnUserInfo_Click(object sender, RoutedEventArgs e)
        {
            adminPage.Source = new Uri("UserInfo.xaml", UriKind.Relative);
        }
    }
}
