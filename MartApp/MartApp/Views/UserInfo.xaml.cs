using MartApp.Logics;
using MartApp.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System;
using System.Windows.Controls;
using System.Windows;

namespace MartApp.Views
{
    /// <summary>
    /// UserInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserInfo : Page
    {
        public UserInfo()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            List<User> list = new List<User>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                {
                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var query = $@"SELECT Id,
                                          Name,
                                          PhoneNum
                                     FROM userdb;";

                    var cmd = new MySqlCommand(query, conn);
                    var adapter = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "userdb");

                    foreach (DataRow row in ds.Tables["userdb"].Rows)
                    {
                        list.Add(new User
                        {
                            Id = Convert.ToString(row["Id"]),
                            Name = Convert.ToString(row["Name"]),
                            PhoneNum = Convert.ToString(row["PhoneNum"]),
                        });
                    }
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

