using MahApps.Metro.Controls;
using MartApp.Logics;
using MartApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace MartApp.Views
{
    /// <summary>
    /// DetailOrderList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DetailOrderList : MetroWindow
    {
        int order_id;
        public DetailOrderList()
        {
            InitializeComponent();
        }

        public DetailOrderList(int order_id)
        {
            this.order_id = order_id;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = null;
            List<OrderItem> list = new List<OrderItem>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                {
                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var query = @"SELECT Order_Id
                                         Id,
                                         Product,
                                         Price,
                                         Count,
                                         Category,
                                         Image,
                                         DateTime
                                    FROM paymenttbl
                                   WHERE Order_Id = @Order_Id";
                    var cmd = new MySqlCommand(query, conn);
                    var adapter = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "mart");
                    cmd.Parameters.AddWithValue("@Order_Id", this.order_id);

                    foreach (DataRow row in ds.Tables["mart"].Rows)
                    {
                        //var TimeDate = DateTime.
                        list.Add(new OrderItem
                        {
                            Id = Convert.ToString(row["Id"]),
                            Product = Convert.ToString(row["Product"]),
                            Price = Convert.ToInt32(row["Price"]),
                            Count = Convert.ToInt32(row["Count"]),
                            Category = Convert.ToString(row["Category"]),
                            Image = Convert.ToString(row["Image"]),
                            DateTime = Convert.ToDateTime(row["DateTime"]),
                        });
                    }
                    this.DataContext = list;
                    GrdUserInfo.ItemsSource = list; // 이미지 띄움
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"장바구니 오류!{ex.Message}", "장바구니");
            }

        }
    }
}

