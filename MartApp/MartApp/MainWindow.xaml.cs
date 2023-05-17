using MahApps.Metro.Controls;
using mart;
using MartApp.Logics;
using System;
using System.Diagnostics;
using System.Windows;

namespace MartApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 과일 페이지 이동
        private void BtnSelFruit_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("FruitPage.xaml", UriKind.Relative);
        }

        // 채소 페이지 이동
        private void BtnSelVege_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("VegePage.xaml", UriKind.Relative);
        }
        
        // 육류 페이지 이동
        private void BtnSelMeat_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("MeatPage.xaml", UriKind.Relative);
        }

        // 수산 페이지 이동
        private void BtnSelSeafood_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("SeafoodPage.xaml", UriKind.Relative);
        }

        // 스낵 페이지 이동
        private void BtnSelSnack_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("SnackPage.xaml", UriKind.Relative);
        }

        // 음료 페이지 이동
        private void BtnSelDrink_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("DrinkPage.xaml", UriKind.Relative);
        }

        // 장바구니 버튼 클릭 이벤트
        private void BtnCart_Click(object sender, RoutedEventArgs e)
        {
            var cartpage = new CartWindow();
            cartpage.Owner = this;
            cartpage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            cartpage.ShowDialog(); // 장바구니 열림
        }

        // MetroWindow 창이 로드 되었을때 로그인 창이 먼저 띄어지게 함
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var loginpage = new LoginWindow();
            loginpage.Owner = this;
            loginpage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            loginpage.ShowDialog();
            if (Commons.Islogin )
            {
                BtnSelFruit_Click(sender, e);
                LblLogin.Content = $"{Commons.Id}님 환영합니다!";
            }
            else
            {
                this.Close();
            }
        }
        
        //MetroWindow가 종료되면 프로세스도 종료
        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
