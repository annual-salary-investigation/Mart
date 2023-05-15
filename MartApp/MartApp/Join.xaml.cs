using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MartApp.Logics;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace MartApp
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
            bool isTrue = true;
            string errorMsg = string.Empty;

            if (string.IsNullOrEmpty(txtId.Text))
            {
                isTrue = false;
                errorMsg += "아이디를 입력하세요\n";
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                isTrue = false;
                errorMsg += "이름을 입력하세요\n";
            }
            if (string.IsNullOrEmpty(txtPassword.Password))
            {
                isTrue = false;
                errorMsg += "비밀번호를 입력하세요\n";
            }
            if (string.IsNullOrEmpty(txtPasswordCheck.Password))
            {
                isTrue = false;
                errorMsg += "비밀번호확인을 입력하세요\n";
            }
            if (string.IsNullOrEmpty(txtPhoneNum.Text))
            {
                isTrue = false;
                errorMsg += "핸드폰번호를 입력하세요\n";                
            }

            if (isTrue == false)
            {
                await this.ShowMessageAsync("오류", errorMsg, MessageDialogStyle.Affirmative, null);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

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
                    cmd.Parameters.AddWithValue("@PassWord", txtPassword.Password);
                    cmd.Parameters.AddWithValue("@PhoneNum", txtPhoneNum.Text);

                    // 명령객체 실행구문
                    // 영향을 받은 행수를 반환함 => 회원가입이니까 1밖에 나올 수 없음
                    if (cmd.ExecuteNonQuery() == 1) {
                        await this.ShowMessageAsync("회원가입", "회원가입 성공!!!");
                        this.Close();
                    }
                    else
                    {
                        await this.ShowMessageAsync("회원가입", "실패!");
                    }
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("로그인 오류", $"{ex.Message}", MessageDialogStyle.Affirmative, null);
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

        // 중복 아이디 확인
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

                    adapter.Fill(ds, "userdb"); // userdb의 값을 ds에 채움

                    if (hasSpace(txtId.Text))
                    {
                        IdNotice.Text = "아이디에 공백이 있습니다!";
                    }
                    else if (string.IsNullOrEmpty(txtId.Text))
                    {
                        IdNotice.Text = "아이디를 입력하세요!";
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
                this.ShowMessageAsync("오류", $"오류 : {ex.Message}");
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


        private void txtPasswordCheck_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password == string.Empty)
            {
                PWNotice.Text = "비밀번호 일치 확인";
            }
            else if (txtPassword.Password.Equals(txtPasswordCheck.Password, StringComparison.Ordinal))
            {
                PWNotice.Text = "비밀번호가 일치합니다.";
            }
            else
            {
                PWNotice.Text = "비밀번호가 일치하지 않습니다.";
            }
        }
    }
}
