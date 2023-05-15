﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MartApp.Logics;
using MartApp.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace MartApp
{
    /// <summary>
    /// SuccessWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SuccessWindow : MetroWindow
    {
        public SuccessWindow()
        {
            InitializeComponent();
        }

        private void BtnMart_Click(object sender, RoutedEventArgs e)
        {
            //using(MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
            //{
            //    if (conn.State == System.Data.ConnectionState.Closed)
            //    {
            //        conn.Open();
            //    }
            //}
            this.Close();

        }

        private void BtnPayment_Click(object sender, RoutedEventArgs e)
        {
            var Payment = new payment();
            Payment.Owner = this;
            Payment.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Payment.ShowDialog();
        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = null;
            List<OrderItem> list = new List<OrderItem>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                {
                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

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

                    foreach (DataRow row in ds.Tables["orderdb"].Rows)
                    {
                        //var TimeDate = DateTime.
                        list.Add(new OrderItem
                        {
                            ProductId = Convert.ToInt32(row["ProductId"]),
                            Id = Convert.ToString(row["Id"]),
                            Product = Convert.ToString(row["Product"]),
                            Price = Convert.ToInt32(row["Price"]),
                            Count = Convert.ToInt32(row["Count"]),
                            Category = Convert.ToString(row["Category"]),
                            // Image = new BitmapImage(new Uri(Convert.ToString(row["Image"], UriKind.RelativeOrAbsolute))),
                            Image = Convert.ToString(row["Image"]),
                            DateTime = Convert.ToDateTime(row["DateTime"])
                        });
                    }
                    // this.DataContext = list;
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
                await this.ShowMessageAsync("오류", $"장바구니 보기 오류 : {ex.Message}");
            }

        }

        private async void BtnDel_Click(object sender, RoutedEventArgs e)
        {

            if (GrdCart.SelectedItems.Count == 0)
            {
                await Commons.ShowMessageAsync("오류", "삭재할 상품을 선택하시오.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                {
                    conn.Open();

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
                    MetroWindow_Loaded(sender, e);
                }
            }
            catch (Exception ex)
            {
                await Commons.ShowMessageAsync("오류", $"DB삭제 오류 {ex.Message}");
            }

        }
    }
}
