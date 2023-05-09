using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MartApp.Logics;
using MartApp.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
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
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                await this.ShowMessageAsync("오류", "패스워드를 입력하세요", MessageDialogStyle.Affirmative, null);
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

                    var insRes = 0;

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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
