﻿using MartApp.Logics;
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
        }

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

                        Button btn = this.FindName($"Btn{i + 1}") as Button;
                        btn.Tag = Convert.ToInt32(ds.Tables["martdb"].Rows[i]["ProductId"]); // 태그 각 컨트롤내 숨기고 싶은 값을 가지고 가도록 해주는 속성
                        // 라벨
                        // Debug.WriteLine($"{ds.Tables["martdb"].Rows[i]["Product"]}");
                        var LblContent = Convert.ToString(ds.Tables["martdb"].Rows[i]["Product"]);
                        Label label = this.FindName($"Lbl{i + 1}") as Label;
                        label.Content = LblContent;


                    }
                }
            }
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;            
            CartWindowShow((int)clickedButton.Tag);
        }

        private void CartWindowShow(int productId)
        {
            var detailWindow = new DetailWindow(productId);
            detailWindow.ShowDialog();
        }
    }
}
