using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MartApp;
using MartApp.Logics;
using MartApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace mart
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CartWindow : MetroWindow
    {
        public CartWindow()
        {
            InitializeComponent();
        }

        public async void CartWindow_Loaded(object sender, RoutedEventArgs e)
        {
            List<OrderItem> list = new List<OrderItem>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    var query = $@"SELECT ProductId,
                                          Id,
                                          Product,
                                          Price,
                                          Count,
                                          Category,
                                          Image,
                                          DateTime
                                     FROM orderdb
                                    WHERE Id = '{Commons.Id}'";

                    var cmd = new MySqlCommand(query, conn);
                    var adapter = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "orderdb");

                    // 바인딩 하기 위해
                    foreach (DataRow row in ds.Tables["orderdb"].Rows)
                    {
                        list.Add(new OrderItem
                        {
                            ProductId = Convert.ToInt32(row["ProductId"]),
                            Id = Convert.ToString(row["Id"]),
                            Product = Convert.ToString(row["Product"]),
                            Price = Convert.ToInt32(row["Price"]),
                            Count = Convert.ToInt32(row["Count"]),
                            Category = Convert.ToString(row["Category"]),
                            Image = Convert.ToString(row["Image"]),
                            DateTime = Convert.ToDateTime(row["DateTime"])
                        });
                    }

                    GrdCart.ItemsSource = list; // 이미지 띄움
                    StsResult.Content = $"장바구니 {list.Count} 건 조회";

                    // 총 합계금액
                    query = $@"SELECT Id,
                                      SUM(Price) AS Total
                                 FROM orderdb
                                WHERE Id = '{Commons.Id}'
                             GROUP BY Id";

                    cmd = new MySqlCommand(query, conn);
                    adapter = new MySqlDataAdapter(cmd);
                    ds = new DataSet();
                    adapter.Fill(ds, "orderdb");

                    var labeltext = Convert.ToString(ds.Tables["orderdb"].Rows[0]["Total"]);
                    LblTotalPrice.Content = $"총 합계 금액 : {labeltext}";
                }
            }
            catch (System.Exception ex)
            {
                await this.ShowMessageAsync("장바구니", $"장바구니 보기 오류 : {ex.Message}");
            }
        }

        // 상품 버튼 누를때
        private void BtnProduct_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // 장바구니창 꺼지면서 상품으로 돌아가게 됨
        }

        // 결제 버튼 누를때
        private void BtnPayment_Click(object sender, RoutedEventArgs e)
        {
            // orderdb Checked 업데이트

            if (GrdCart.SelectedItems.Count == 0) // 하나도 선택된 것이 없으면
            {
                this.ShowMessageAsync("장바구니", "결제할 상품을 선택하세요.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    // Checked 기본값을 한번 초기화
                    var query = @"UPDATE orderdb SET Checked = NULL";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.ExecuteNonQuery();

                    // 선택된 셀의 Checked 값을 업데이트
                    query = @"UPDATE orderdb
                                 SET Checked = 1
                               WHERE Id = @Id AND ProductId=@ProductId AND DateTime=@DateTime";

                    foreach (OrderItem item in GrdCart.SelectedItems)
                    {
                        cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                        cmd.Parameters.AddWithValue("@Id", item.Id);
                        cmd.Parameters.AddWithValue("@DateTime", item.DateTime);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("장바구니", $"결제 오류 {ex.Message}");
            }
            
            // 결제창 띄움
            var Payment = new payment();
            Payment.Owner = this;
            Payment.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Payment.ShowDialog();
            // 장바구니창 새로고침
            CartWindow_Loaded(sender, e);
        }

        // 삭제 버튼 누를때 
        private async void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (GrdCart.SelectedItems.Count == 0)
            {
                await Commons.ShowMessageAsync("장바구니", "삭제할 상품을 선택하시오.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    var query = @"DELETE FROM orderdb WHERE Id = @Id AND ProductId=@ProductId AND DateTime=@DateTime";

                    foreach (OrderItem item in GrdCart.SelectedItems)
                    {
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                        cmd.Parameters.AddWithValue("@Id", item.Id);
                        cmd.Parameters.AddWithValue("@DateTime", item.DateTime);
                        cmd.ExecuteNonQuery();
                    }

                    await this.ShowMessageAsync("장바구니", "장바구니 삭제 완료");
                    CartWindow_Loaded(sender, e);
                }
            }
            catch (Exception ex)
            {
                await Commons.ShowMessageAsync("장바구니", $"DB삭제 오류 {ex.Message}");
            }
        }
    }
}
