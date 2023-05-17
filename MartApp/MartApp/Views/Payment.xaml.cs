using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MartApp.Logics;
using MySql.Data.MySqlClient;
using System.Data;
using System;
using System.Windows;

namespace MartApp.Views
{
    /// <summary>
    /// Payment.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class payment : MetroWindow
    {
        public payment()
        {
            InitializeComponent();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void BtnPayment_Click(object sender, RoutedEventArgs e)
        {
            // 결제완료 팝업창
            // 결제창에서 확인을 누르면
            // 1. 해당 상품을 PaymenntDB에 insert
            // 2. orderDB에서 결제된 상품 삭제

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    // 장바구니에서 선택된 상품 조회
                    var query = @"SELECT ProductId,
                                         Id,
                                         Product,
                                         Price,
                                         Count,
                                         Category,
                                         Image,
                                         DateTime,
                                         Checked
                                    FROM orderdb
                                   WHERE Checked = 1";

                    var cmd = new MySqlCommand(query, conn);
                    var adapter = new MySqlDataAdapter(cmd);
                    var ds = new DataSet();
                    adapter.Fill(ds, "orderdb");

                    // PaymentDB에 insert
                    query = @"INSERT INTO paymentdb
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
                                          @DateTime)";
                    
                    foreach (DataRow row in ds.Tables["orderdb"].Rows)
                    {
                        cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ProductId", row["ProductId"]);
                        cmd.Parameters.AddWithValue("@Id", row["Id"]);
                        cmd.Parameters.AddWithValue("@Product", row["Product"]);
                        cmd.Parameters.AddWithValue("@Price", row["Price"]);
                        cmd.Parameters.AddWithValue("@Count", row["Count"]);
                        cmd.Parameters.AddWithValue("@Category", row["Category"]);
                        cmd.Parameters.AddWithValue("@Image", row["Image"]);
                        cmd.Parameters.AddWithValue("@DateTime", row["DateTime"]);
                        cmd.ExecuteNonQuery();
                    }

                    // 장바구니에서 삭제
                    query = @"DELETE FROM orderdb WHERE Checked=1;";
                    cmd = new MySqlCommand(query, conn);
                    cmd.ExecuteNonQuery();

                    await this.ShowMessageAsync("결제", "결제가 완료되었습니다.");
                    this.Close();
                }
            }
            catch(Exception ex) 
            {
                await this.ShowMessageAsync("결제", $"결제오류 {ex.Message}");
            }
        }
    }
}
