using MartApp.Logics;
using MartApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MartApp
{
    /// <summary>
    /// FruitPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FruitPage : Page
    {
        public FruitPage()
        {
            InitializeComponent();
<<<<<<< Updated upstream
        }
=======
            //LoadedFruitPage();
        }

        //private void LoadedFruitPage()
        //{
        //    List<MartItem> list = new List<MartItem>();
        //    {
        //        using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
        //        {
        //            if (conn.State == System.Data.ConnectionState.Closed) { conn.Open(); }

        //            var query = @"SELECT ProductId,
        //                                 Product,
        //                                 Price,
        //                                 Category,
        //                                 Image
        //                            FROM martdb
        //                            WHERE Category='과일'";

        //            var cmd = new MySqlCommand(query, conn);
        //            var adapter = new MySqlDataAdapter(cmd);
        //            var ds = new DataSet();
        //            adapter.Fill(ds, "martdb");

        //            foreach (DataRow row in ds.Tables["martdb"].Rows)
        //            {
        //                list.Add(new MartItem
        //                {
        //                    ProductId = Convert.ToInt32(row["ProductId"]),
        //                    Product = Convert.ToString(row["Product"]),
        //                    Price = Convert.ToInt32(row["Price"]),
        //                    Category = Convert.ToString(row["Category"]),
        //                    Image = Convert.ToString(row["Image"])
        //                });
        //            }

        //            this.DataContext = list;
        //        }
        //    }
        //}
>>>>>>> Stashed changes

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
            List<MartItem> list = new List<MartItem>();
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed) { conn.Open(); }

                    var query = @"SELECT ProductId,
                                         Product,
                                         Price,
                                         Category,
                                         Image
                                    FROM martdb
                                    WHERE Category='과일'";

                    var cmd = new MySqlCommand(query, conn);
                    var adapter = new MySqlDataAdapter(cmd);
                    var ds = new DataSet();
                    adapter.Fill(ds, "martdb");

                    for (int i = 0; i < ds.Tables["martdb"].Rows.Count; i++)
                    {
                        Debug.WriteLine($"{i}");
                        Debug.WriteLine($"{ds.Tables["martdb"].Rows[i]["Image"]}");
                        var imgSource = Convert.ToString(ds.Tables["martdb"].Rows[i]["Image"]);
                        Image image = this.FindName($"Img{i + 1}") as Image;
                        image.Source = new BitmapImage(new Uri(imgSource, UriKind.RelativeOrAbsolute));
                    }
                }
            }
        }
    }
}
