using MartApp.Logics;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MartApp
{
    /// <summary>
    /// DrinkPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DrinkPage : Page
    {
        public DrinkPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
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
                                    WHERE Category='음료'";

                var cmd = new MySqlCommand(query, conn);
                var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds, "martdb");

                for (int i = 0; i < ds.Tables["martdb"].Rows.Count; i++)
                {
                    var imgSource = Convert.ToString(ds.Tables["martdb"].Rows[i]["Image"]);
                    Image image = this.FindName($"Img{i + 1}") as Image;
                    image.Source = new BitmapImage(new Uri(imgSource, UriKind.RelativeOrAbsolute));

                    Button btn = this.FindName($"Btn{i + 1}") as Button;
                    btn.Tag = Convert.ToInt32(ds.Tables["martdb"].Rows[i]["ProductId"]); // 태그 각 컨트롤내 숨기고 싶은 값을 가지고 가도록 해주는 속성

                    var TxbText = Convert.ToString(ds.Tables["martdb"].Rows[i]["Product"]);
                    TextBlock textBlock = this.FindName($"Txb{i + 1}") as TextBlock;
                    textBlock.Text = TxbText;
                }
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            Commons.CartWindowShow((int)clickedButton.Tag);
        }
    }
}
