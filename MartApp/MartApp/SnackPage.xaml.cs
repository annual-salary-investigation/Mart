using MartApp.Logics;
using MartApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

                    foreach (DataRow row in ds.Tables["martdb"].Rows)
                    {
                        list.Add(new MartItem
                        {
                            ProductId = Convert.ToInt32(row["ProductId"]),
                            Product = Convert.ToString(row["Product"]),
                            Price = Convert.ToInt32(row["Price"]),
                            Category = Convert.ToString(row["Category"]),
                            Image = Convert.ToString(row["Image"])
                        });
                    }

                    this.DataContext = list;
                }
            }
        }
    }
}
