using MartApp.Logics;
using MartApp.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System.Collections.Generic;
using System.Data;
using System;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows;

namespace MartApp.Views
{
    /// <summary>
    /// OrderList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OrderList : Page
    {
        int order_id = 0;
        public OrderList()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            {
                this.DataContext = null;
                List<OrderItem> list = new List<OrderItem>();

                try
                {
                    using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                    {
                        if (conn.State == ConnectionState.Closed) { conn.Open(); }

                        var query = @"SELECT p.Order_Id, p.Product, p.Price, p.Id, p.DateTime
                                        FROM Paymenttbl AS p
                                        JOIN (
                                             SELECT Order_Id, MAX(Datetime) AS maxDate
                                               FROM Paymenttbl
                                              GROUP BY Order_Id
                                              ) AS sub
                                           ON p.Order_Id = sub.Order_Id AND p.Datetime = sub.maxDate;";
                        var cmd = new MySqlCommand(query, conn);
                        var adapter = new MySqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds, "mart");

                        foreach (DataRow row in ds.Tables["mart"].Rows)
                        {
                            //var TimeDate = DateTime.
                            list.Add(new OrderItem
                            {
                                Id = Convert.ToString(row["Id"]),
                                Order_Id = Convert.ToInt32(row["Order_Id"]),
                                Product = Convert.ToString(row["Product"]+" ..."),
                                Price = Convert.ToInt32(row["Price"]),
                                DateTime = Convert.ToDateTime(row["DateTime"])
                            });
                        }
                        this.DataContext = list;
                        //GrdUserInfo.ItemsSource = list; // 이미지 띄움
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"장바구니 오류!{ex.Message}", "장바구니");
                }
            }
        }

        private void GrdOrderInfo_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selItem = GrdOrderInfo.SelectedItem as OrderItem;
            var detailOrderList = new DetailOrderList(selItem.Order_Id);
            detailOrderList.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            detailOrderList.ShowDialog();
        }
    }
}
