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

        private void BtnOrderList_Click(object sender, RoutedEventArgs e)
        {
            adminPage.Source = new Uri("OrderList.xaml", UriKind.Relative);
        }

        private void BtnUserInfo_Click(object sender, RoutedEventArgs e)
        {
            adminPage.Source = new Uri("UserInfo.xaml", UriKind.Relative);
        }
    }
}
