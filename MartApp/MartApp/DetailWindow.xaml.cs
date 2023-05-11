using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using mart;
using MartApp.Logics;
using MartApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace MartApp
{
    /// <summary>
    /// detailWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DetailWindow : MetroWindow
    {
        private int productId; // 부모창에서 넘어온 ProductID(DB 키값)

        private int currCount = 0;  // 현재 수량 확인 변수

        DataSet ds = new DataSet(); // martDB 데이터

        public DetailWindow()
        {
            InitializeComponent();       
        }

        public DetailWindow(int productId) : this()
        {
            this.productId = productId;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // DB insert 테스트
            try
            {
                var insRes = 0;

                using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                {
                    if (conn.State == ConnectionState.Closed) { conn.Open(); }
                    string insQuery = @"INSERT INTO orderdb
                                                   ( ProductId,
                                                     Id,
                                                     Product,
                                                     Price,
                                                     Count,
                                                     Category,
                                                     Image,
                                                     DateTime)
                                                     VALUES
                                                   ( @ProductId,
                                                     @Id,
                                                     @Product,
                                                     @Price,
                                                     @Count,
                                                     @Category,
                                                     @Image,
                                                     @DateTime )";
                    
                    MySqlCommand cmd = new MySqlCommand(insQuery, conn);
                    cmd.Parameters.AddWithValue("@ProductId", ds.Tables["martdb"].Rows[0]["ProductId"]);
                    // cmd.Parameters.AddWithValue("@Id", );
                    cmd.Parameters.AddWithValue("@Product", ds.Tables["martdb"].Rows[0]["Product"]);
                    cmd.Parameters.AddWithValue("@Price", ds.Tables["martdb"].Rows[0]["Price"]);
                    cmd.Parameters.AddWithValue("@Count", lblCount.Content);
                    cmd.Parameters.AddWithValue("@Category", ds.Tables["martdb"].Rows[0]["Category"]);
                    cmd.Parameters.AddWithValue("@Image", ds.Tables["martdb"].Rows[0]["Image"]);
                    cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
                    insRes += cmd.ExecuteNonQuery();
                }

                MessageBox.Show("DB저장성공", "저장");
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"DB연결오류 {ex.Message}", MessageDialogStyle.Affirmative, null);
            }

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
            List<MartItem> list = new List<MartItem>();
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    var query = @"SELECT ProductId,
                                         Product,
                                         Price,
                                         Category,
                                         Image
                                    FROM martdb
                                    WHERE ProductId = @ProductId";

                    var cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProductId", this.productId);
                    var adapter = new MySqlDataAdapter(cmd);
                    
                    adapter.Fill(ds, "martdb");

                    if (ds.Tables["martdb"].Rows.Count == 1)
                    {
                        Debug.WriteLine($"{ds.Tables["martdb"].Rows[0]["Image"]}");
                        var ImageSource = Convert.ToString(ds.Tables["martdb"].Rows[0]["Image"]);
                        var productName = Convert.ToString(ds.Tables["martdb"].Rows[0]["Product"]);
                        var productPrice = Convert.ToString(ds.Tables["martdb"].Rows[0]["Price"]);
                        if (string.IsNullOrEmpty(ImageSource))
                        {
                            ImgProduct.Source = new BitmapImage(new Uri("/No_Picture.png", UriKind.RelativeOrAbsolute));

                        }
                        else
                        {
                            ImgProduct.Source = new BitmapImage(new Uri(ImageSource, UriKind.RelativeOrAbsolute));
                        }
                        LblProductName.Content = productName;
                        LblPrice.Content = productPrice;
                    }

                    
                }
            }
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (currCount < 99)
            {
                currCount++;
                lblCount.Content = currCount.ToString();
            }
            else
            {
                this.ShowMessageAsync("오류", "상품은 최대 99개까지 가능합니다!");
            }
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (currCount > 0)
            {
                currCount--;
                lblCount.Content = currCount.ToString();
            }
            else
            {
                this.ShowMessageAsync("오류", "상품이 적어도 1개는 필요합니다!");
            }
        }
    }
}
