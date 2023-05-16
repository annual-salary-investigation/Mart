﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using mart;
using MartApp.Logics;
using MartApp.Models;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Security.AccessControl;
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
        int currCount = 1;
        int price_product = 0;

        DataSet ds = new DataSet(); // martDB 데이터
        
        //DataSet usds = new DataSet(); // userDB 데이터

        public DetailWindow()
        {
            InitializeComponent();       
        }

        public DetailWindow(int productId) : this()
        {
            this.productId = productId;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.MettroWindow_Question(sender, new System.ComponentModel.CancelEventArgs());
        }

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


            // Update문을 사용해서 if 문써서 같은 상품이 들어오면 Update되게 Productid 값으로 날짜도 바뀌게
            if (result == MessageDialogResult.Affirmative)
            {
                try
                {
                    var insRes = 0;

                    using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                    {
                        if (conn.State == ConnectionState.Closed) { conn.Open(); }
                        string chkQuery = @"SELECT ProductId FROM orderdb 
                                             WHERE ProductId = @ProductId AND Id = @Id";
                        MySqlCommand cmd = new MySqlCommand(chkQuery, conn);
                        cmd.Parameters.AddWithValue("@ProductId", ds.Tables["martdb"].Rows[0]["ProductId"]);
                        cmd.Parameters.AddWithValue("@Id", Commons.Id);

                        var count = Convert.ToInt32( cmd.ExecuteScalar());
                        if (count == 0)
                        {
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

                            cmd = new MySqlCommand(insQuery, conn);
                            cmd.Parameters.AddWithValue("@ProductId", ds.Tables["martdb"].Rows[0]["ProductId"]);
                            cmd.Parameters.AddWithValue("@Id", Commons.Id);
                            cmd.Parameters.AddWithValue("@Product", ds.Tables["martdb"].Rows[0]["Product"]);
                            cmd.Parameters.AddWithValue("@Price", LblPrice.Content);
                            cmd.Parameters.AddWithValue("@Count", lblCount.Content);
                            cmd.Parameters.AddWithValue("@Category", ds.Tables["martdb"].Rows[0]["Category"]);
                            cmd.Parameters.AddWithValue("@Image", ds.Tables["martdb"].Rows[0]["Image"]);
                            cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
                            insRes += cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            string upQuery = @"UPDATE orderdb
                                                  SET Count = Count + @Count,
                                                      Price = Price + @Price,
                                                      DateTime = @DateTime
                                                WHERE ProductId = @ProductId";
                            cmd = new MySqlCommand(upQuery, conn);

                            int total_price = Convert.ToInt32(currCount) * price_product;

                            cmd.Parameters.AddWithValue("@Count", lblCount.Content);
                            cmd.Parameters.AddWithValue("@Price", LblPrice.Content);
                            cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
                            cmd.Parameters.AddWithValue("@ProductId", ds.Tables["martdb"].Rows[0]["ProductId"]);
                            insRes += cmd.ExecuteNonQuery();
                        }
                    }
                    await this.ShowMessageAsync("장바구니", "장바구니에 추가되었습니다!");
                    result = await this.ShowMessageAsync("장바구니", "장바구니에 추가하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative, mySettings_two);
                    
                    // 쇼핑게속하기 / 장바구니 확인
                    if(result == MessageDialogResult.Affirmative)
                    {
                        this.Close();
                    }
                    else if(result == MessageDialogResult.Negative)
                    {
                        this.Close();
                        var cartpage = new CartWindow();            // CartWindow 장바구니창
                        cartpage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        cartpage.ShowDialog();
                    }

                }
                catch (Exception ex)
                {
                    await this.ShowMessageAsync("오류", $"DB연결오류 {ex.Message}", MessageDialogStyle.Affirmative, null);
                }

            }
            else if (result == MessageDialogResult.Negative)
            {
                e.Cancel = true;
            }
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
            currCount = 1;
            lblCount.Content = currCount;  // 현재 수량 확인 변수
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
                    // var ds = new DataSet(); 
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
                        price_product = Convert.ToInt32(productPrice);
                    }

                    var insquery = @"SELECT ProductId
                                          , Id
                                          , Product
                                          , Price
                                          , Count
                                          , Category
                                          , Image
                                          , DateTime
                                       FROM orderdb;";
                }
            }
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (currCount < 99)
            {
                currCount++;
                lblCount.Content = currCount.ToString();

                int price = Convert.ToInt32(currCount) * price_product;
                LblPrice.Content = price;
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
