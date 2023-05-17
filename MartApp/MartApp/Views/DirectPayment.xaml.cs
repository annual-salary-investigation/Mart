using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MartApp.Logics;
using MySql.Data.MySqlClient;
using System.Data;
using System;
using System.Diagnostics;

namespace MartApp.Views
{
    /// <summary>
    /// DirectPayment.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DirectPayment : MetroWindow
    {
        DataSet ds = new DataSet(); 
        int productId = 0;
        int count = 0;
        public DirectPayment()
        {
            InitializeComponent();
        }

        public DirectPayment(int productId, int count) : this()
        {
            this.productId = productId;
            this.count = count;
        }

        private async void BtnPayment_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                {
                    if (conn.State == ConnectionState.Closed)
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

                    query = @"INSERT INTO paymentdb
                                        ( ProductId,
                                          Id,
                                          Product,
                                          Count,
                                          Price,
                                          Category,
                                          Image,
                                          DateTime
                                          )
                                   VALUES
                                        ( @ProductId,
                                          @Id,
                                          @Product,
                                          @Count,
                                          @Price,
                                          @Category,
                                          @Image,
                                          @DateTime
                                          )";

                    foreach (DataRow row in ds.Tables["martdb"].Rows)
                    {
                        cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ProductId", row["ProductId"]);
                        cmd.Parameters.AddWithValue("@Id", Commons.Id);
                        cmd.Parameters.AddWithValue("@Product", row["Product"]);
                        cmd.Parameters.AddWithValue("@Price", Convert.ToString(Convert.ToInt32(row["Price"])*count));
                        cmd.Parameters.AddWithValue("@Count", Convert.ToString(count));
                        cmd.Parameters.AddWithValue("@Category", row["Category"]);
                        cmd.Parameters.AddWithValue("@Image", row["Image"]);
                        cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                    await this.ShowMessageAsync("결제", "결제가 완료되었습니다.");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("결제", $"결제오류 {ex.Message}");
            }
        }

        private void BtnNo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
