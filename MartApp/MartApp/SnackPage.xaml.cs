using mart;
using MartApp.Logics;
using MartApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MartApp
{
    /// <summary>
    /// SnackPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SnackPage : Page
    {
        public SnackPage()
        {
            InitializeComponent();
            LoadedSnackPage();
        }

        private void LoadedSnackPage()
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
                                    WHERE Category='과자'";

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
            CartWindowShow();
        }

        private void CartWindowShow()
        {
            var detailWindow = new DetailWindow();
            detailWindow.ShowDialog();
        }
    }
}
