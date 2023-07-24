using MahApps.Metro.Controls;
using MartApp.Logics;
using MartApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using MartApp.Logics;

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

        private int productId; // 부모창에서 넘어온 ProductID(DB 키값)
        int currCount = 1;
        int price_product = 0;
        int count = 0;


        // 과일 페이지 이동
        private void BtnSelFruit_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("Views/FruitPage.xaml", UriKind.Relative);
        }

        // 채소 페이지 이동
        private void BtnSelVege_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("Views/VegePage.xaml", UriKind.Relative);
        }
        
        // 육류 페이지 이동
        private void BtnSelMeat_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("Views/MeatPage.xaml", UriKind.Relative);
        }

        // 수산 페이지 이동
        private void BtnSelSeafood_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("Views/SeafoodPage.xaml", UriKind.Relative);
        }

        // 스낵 페이지 이동
        private void BtnSelSnack_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("Views/SnackPage.xaml", UriKind.Relative);
        }

        // 음료 페이지 이동
        private void BtnSelDrink_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("Views/DrinkPage.xaml", UriKind.Relative);
        }

        // 장바구니 버튼 클릭 이벤트
        private void BtnCart_Click(object sender, RoutedEventArgs e)
        {
            var cartpage = new Views.CartWindow();
            cartpage.Owner = this;
            cartpage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            cartpage.ShowDialog(); // 장바구니 열림
        }

        // MetroWindow 창이 로드 되었을때 로그인 창이 먼저 띄어지게 함
        public void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var loginpage = new Views.LoginWindow();
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

            using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
            {
                conn.Open();
                // 중복되지 않은 날짜를 가져오기 위해 DISTINCT를 사용하고, DATE 함수를 이용하여 DateTime 열에서 날짜만 추출합니다.
                // AS DateOnly 구문을 사용하여 결과 컬럼의 이름을 DateOnly로 지정합니다.
                var query = @"SELECT DISTINCT DATE(DateTime) AS DateOnly
                                  FROM mart.paymenttbl
                                  WHERE Id = @Id
                                  ORDER BY DateOnly"; // 중복된 날짜를 하나로 통합하여 가져옵니다.
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("Id", Commons.Id);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                List<string> DateTimelist = new List<string>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    // 날짜를 "yyyy-MM-dd" 형식으로 변환하여 리스트에 추가합니다.
                    DateTimelist.Add(Convert.ToDateTime(row["DateOnly"]).ToString("yyyy-MM-dd"));
                }

                // 콤보 박스의 ItemsSource에 날짜 목록을 바인딩하여 년/월/일만 표시합니다.
                CboReqDate.ItemsSource = DateTimelist;
            }
        }
        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        public void CboReqDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try 
            {
                if (CboReqDate.SelectedValue != null)
                {
                    //MessageBox.Show(CboReqDate.SelectedValue.ToString());
                    using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                    {
                        var query = @" SELECT Product,
                                              Id,
                                              Price,
                                              Count,  
                                              Image
                                         FROM paymenttbl
                                         WHERE Id = @Id and date_format(DateTime, '%Y-%m-%d')  = @DateTime;";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("DateTime", CboReqDate.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("Id", Commons.Id);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds, "paymenttbl");
                        List<PaymentItem> paymenttbl = new List<PaymentItem>();
                        foreach (DataRow row in ds.Tables["paymenttbl"].Rows)
                        {
                            paymenttbl.Add(new PaymentItem
                            {
                                Product = Convert.ToString(row["Product"]),
                                Price = Convert.ToInt32(row["Price"]),
                                Count = Convert.ToInt32(row["Count"]),
                                Image = Convert.ToString(row["Image"]),
                            });
                        }
                        this.DataContext = paymenttbl;
                        StsResult.Content = $"DB {paymenttbl.Count}건 조회완료";

                    }
                }
                else
                {
                    this.DataContext = null;
                    StsResult.Content = $"DB 조회클리어";
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show($"오류 발생 : {ex}");
            }
        }
    }
}
