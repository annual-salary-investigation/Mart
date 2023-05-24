using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MartApp.Logics;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MartApp.Views
{
    /// <summary>
    /// detailWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DetailWindow : MetroWindow
    {
        private int productId; // 부모창에서 넘어온 ProductID(DB 키값)
        int currCount = 1;
        int price_product = 0;
        int count = 0;

        DataSet ds = new DataSet(); // martDB 데이터
        
        public DetailWindow()
        {
            InitializeComponent();       
        }
        public DetailWindow(int productId) : this()
        {
            this.productId = productId;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            currCount = 1;
            LblCount.Content = currCount;  // 현재 수량 확인 변수

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
                                    FROM marttbl
                                    WHERE ProductId = @ProductId";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductId", this.productId);
                var adapter = new MySqlDataAdapter(cmd);
                // var ds = new DataSet(); 
                adapter.Fill(ds, "mart");

                if (ds.Tables["mart"].Rows.Count == 1)
                {
                    Debug.WriteLine($"{ds.Tables["mart"].Rows[0]["Image"]}");
                    var ImageSource = Convert.ToString(ds.Tables["mart"].Rows[0]["Image"]);
                    var productName = Convert.ToString(ds.Tables["mart"].Rows[0]["Product"]);
                    var productPrice = Convert.ToString(ds.Tables["mart"].Rows[0]["Price"]);
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
                    price_product = Convert.ToInt32(productPrice);
                }
            }
        }

        private void BtnCart_Click(object sender, RoutedEventArgs e)
        {
            this.MettroWindow_Question(sender, new System.ComponentModel.CancelEventArgs());
        }

        // 장바구니 버튼 눌렀을 때
        private async void MettroWindow_Question(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            var mySettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "확인",
                NegativeButtonText = "취소",
                DefaultButtonFocus = MessageDialogResult.Affirmative,
                AnimateShow = true,
                AnimateHide = true,
            };

            var mySettings_two = new MetroDialogSettings
            {
                AffirmativeButtonText = "쇼핑 계속하기",
                NegativeButtonText = "장바구니 확인",
                DefaultButtonFocus = MessageDialogResult.Affirmative,
                AnimateShow = true,
                AnimateHide = true,
            };

            var result = await this.ShowMessageAsync("장바구니", "장바구니에 추가하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result == MessageDialogResult.Affirmative)
            {
                try
                {
                    //var insRes = 0;
                    using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                    {
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }

                        string query = @"SELECT ProductId FROM ordertbl
                                             WHERE ProductId = @ProductId AND Id = @Id";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ProductId", ds.Tables["mart"].Rows[0]["ProductId"]);
                        cmd.Parameters.AddWithValue("@Id", Commons.Id);
                        var count = Convert.ToInt32( cmd.ExecuteScalar()); // 해당하는 쿼리가 있는지 확인
                        
                        if (count == 0) // 해당하는 조건이 없다면 DB에 추가
                        {
                            string insQuery = @"INSERT INTO ordertbl
                                                     ( 
                                                       ProductId,
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

                            cmd = new MySqlCommand(insQuery, conn);
                            cmd.Parameters.AddWithValue("@ProductId", ds.Tables["mart"].Rows[0]["ProductId"]);
                            cmd.Parameters.AddWithValue("@Id", Commons.Id);
                            cmd.Parameters.AddWithValue("@Product", ds.Tables["mart"].Rows[0]["Product"]);
                            cmd.Parameters.AddWithValue("@Price", LblPrice.Content);
                            cmd.Parameters.AddWithValue("@Count", LblCount.Content);
                            cmd.Parameters.AddWithValue("@Category", ds.Tables["mart"].Rows[0]["Category"]);
                            cmd.Parameters.AddWithValue("@Image", ds.Tables["mart"].Rows[0]["Image"]);
                            cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
                            cmd.ExecuteNonQuery();
                            // insRes += cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            string upQuery = @"UPDATE ordertbl
                                                  SET Count = Count + @Count,
                                                      Price = Price + @Price,
                                                      DateTime = @DateTime
                                                WHERE ProductId = @ProductId";
                            cmd = new MySqlCommand(upQuery, conn);

                            int total_price = Convert.ToInt32(currCount) * price_product;

                            cmd.Parameters.AddWithValue("@Count", LblCount.Content);
                            cmd.Parameters.AddWithValue("@Price", LblPrice.Content);
                            cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
                            cmd.Parameters.AddWithValue("@ProductId", ds.Tables["mart"].Rows[0]["ProductId"]);
                            cmd.ExecuteNonQuery();
                            //insRes += cmd.ExecuteNonQuery();
                        }
                    }
                    result = await this.ShowMessageAsync("장바구니", "장바구니에 추가되었습니다!",  MessageDialogStyle.AffirmativeAndNegative, mySettings_two);
                   
                    // 쇼핑게속하기 / 장바구니 확인
                    if(result == MessageDialogResult.Affirmative)
                    {
                        this.Close();
                    }
                    else if(result == MessageDialogResult.Negative)
                    {
                        this.Close();
                        var cartpage = new Views.CartWindow();            // CartWindow 장바구니창
                        cartpage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        cartpage.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    await this.ShowMessageAsync("상품페이지", $"장바구니 버튼 오류 {ex.Message}", MessageDialogStyle.Affirmative, null);
                }
            }
            else if (result == MessageDialogResult.Negative)
            {
                e.Cancel = true;
            }
        }

        // 결제 버튼 눌렀을 때
        private void BtnBuy_Click(object sender, RoutedEventArgs e)
        {
            count = Convert.ToInt32(LblCount.Content);
            var directPayment = new Views.DirectPayment(this.productId, this.count);           // payment 결제창
            directPayment.Owner = this;
            directPayment.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            directPayment.ShowDialog();
        }

        // + 버튼
        private void BtnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (currCount < 99)
            {
                currCount++;
                LblCount.Content = currCount.ToString();

                int price = Convert.ToInt32(currCount) * price_product;
                LblPrice.Content = price;
            }
            else
            {
                this.ShowMessageAsync("오류", "상품은 최대 99개까지 가능합니다!");
            }
        }

        // - 버튼
        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (currCount > 0)
            {
                currCount--;
                LblCount.Content = currCount.ToString();

                int price = Convert.ToInt32(currCount) * price_product;
                LblPrice.Content = price;
            }
            else
            {
                this.ShowMessageAsync("오류", "상품이 적어도 1개는 필요합니다!");
            }
        }
    }
}
