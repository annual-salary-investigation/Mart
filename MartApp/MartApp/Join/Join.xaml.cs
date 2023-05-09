using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MartApp.Logics;
using MartApp.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MartApp.Join
{
    /// <summary>
    /// Join.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Join : MetroWindow
    {
        public Join()
        {
            InitializeComponent();
        }

        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(txtId.Text))
            {
                await this.ShowMessageAsync("오류", "아이디를 입력하세요", MessageDialogStyle.Affirmative, null);
                return;
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                await this.ShowMessageAsync("오류", "이름을 입력하세요", MessageDialogStyle.Affirmative, null);
                return;
            }
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                await this.ShowMessageAsync("오류", "비밀번호를 입력하세요", MessageDialogStyle.Affirmative, null);
                return;
            }
            if (string.IsNullOrEmpty(txtPasswordCheck.Text))
            {
                await this.ShowMessageAsync("오류", "비밀번호 확인을 입력하세요", MessageDialogStyle.Affirmative, null);
                return;
            }
            if (string.IsNullOrEmpty(txtPhoneNum.Text))
            {
                await this.ShowMessageAsync("오류", "핸드폰 번호를 입력하세요", MessageDialogStyle.Affirmative, null);
                return;
            }

            try
            {
                
                using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                    var insQuery = @"INSERT INTO userdb
                                             (Id,
                                             Name,
                                             PassWord,
                                             PhoneNum)
                                     VALUES (
                                             @Id,
                                             @Name,
                                             @PassWord,
                                             @PhoneNum )";

                    MySqlCommand cmd = new MySqlCommand(insQuery, conn);
                    cmd.Parameters.AddWithValue("@Id", txtId.Text);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@PassWord", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@PhoneNum", txtPhoneNum.Text);

                    await this.ShowMessageAsync("성공", "성공!!!");

                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("로그인 오류", $"{ex.Message}", MessageDialogStyle.Affirmative, null);
            }
        }

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtPassword.Text == string.Empty)
            {
                PWNotice.Text = "비밀번호 일치 확인";
            }
            else if (txtPassword.Text.Equals(txtPasswordCheck.Text, StringComparison.Ordinal))
            {

                PWNotice.Text = "비밀번호가 일치합니다.";

            }
            else
            {
                PWNotice.Text = "비밀번호가 일치하지 않습니다.";

            }
        }

        private void txtId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtId.Text == string.Empty)
            {
                IdNotice.Text = "아이디를 입력하시오!";
            }
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void checkId_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                    var insQuery = @"SELECT Id FROM userdb";

                    MySqlCommand cmd = new MySqlCommand(insQuery, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();

                    adapter.Fill(ds, "userdb");

                    if (hasSpace(txtId.Text))
                    {
                        IdNotice.Text = "아이디에 공백이 있습니다!";
                    }
                    else
                    {
                        IdNotice.Text = "사용 가능한 아이디";
                    }

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Debug.WriteLine(row["Id"]);

                        if ( txtId.Text == row["Id"].ToString() )
                        {
                            IdNotice.Text = "※ 중복 아이디 ※";
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private bool hasSpace(string str)
        {
            foreach(char c in str)
            {
                if (c == ' ') return true;
            }
            return false;
        }
    }
}
