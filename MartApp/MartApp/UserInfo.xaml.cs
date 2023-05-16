using MartApp.Logics;
using MartApp.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System;
using System.Windows.Controls;

namespace MartApp
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
            this.DataContext = null;
            List<User> list = new List<User>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                {
                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var query = $@"SELECT Id
	                                    , Name
                                        , PhoneNum
                                     FROM userdb;";


                    var cmd = new MySqlCommand(query, conn);
                    var adapter = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "userdb");

                    foreach (DataRow row in ds.Tables["userdb"].Rows)
                    {
                        //var TimeDate = DateTime.
                        list.Add(new User
                        {
                            Id = Convert.ToString(row["Id"]),
                            Name = Convert.ToString(row["Name"]),
                            PhoneNum = Convert.ToString(row["PhoneNum"]),
                        });
                    }
                    this.DataContext = list;
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("오류남 오류남");
            }

        }
    }
}

