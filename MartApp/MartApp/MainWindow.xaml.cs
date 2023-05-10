using MahApps.Metro.Controls;
using mart;
using MartApp.Logics;
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

        // 과일 페이지
        private void BtnSelFruit_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("FruitPage.xaml", UriKind.Relative);
        }

        // 채소 페이지 이동
        private void BtnSelVege_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("VegePage.xaml", UriKind.Relative);
        }

        // 
        private void BtnSelMeat_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("MeatPage.xaml", UriKind.Relative);
        }

        private void BtnSelSeafood_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("SeafoodPage.xaml", UriKind.Relative);
        }

        private void BtnSelSnack_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("SnackPage.xaml", UriKind.Relative);
        }

        private void BtnSelDrink_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("DrinkPage.xaml", UriKind.Relative);
        }

        private void BtnCart_Click(object sender, RoutedEventArgs e)
        {
            var cartpage = new CartWindow();
            cartpage.Owner = this;
            cartpage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            cartpage.ShowDialog();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var loginpage = new LoginWindow();
            loginpage.Owner = this;
            loginpage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            loginpage.ShowDialog();
            if (Commons.Islogin )
            {
                BtnSelFruit_Click(sender, e);
            }
            else
            {
                this.Close();
            }
        }
    }
}
