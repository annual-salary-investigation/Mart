using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MartApp.Logics;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace MartApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        // 확인 버튼 클릭 이벤트
        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            string strtxtId = "";
            string strtxtPassword = "";
            bool IsLogined = true;
            string errorMsg = string.Empty;

            if (string.IsNullOrEmpty(txtId.Text))
            {
                IsLogined = false;
                errorMsg += "아이디가 입력되지 않았습니다.\n";
            }
            if (string.IsNullOrEmpty(txtPassword.Password))
            {
                IsLogined = false;
                errorMsg += "비밀번호가 입력되지 않았습니다.\n";
            }

            if (IsLogined == false)
            {
                await this.ShowMessageAsync("로그인", $"{errorMsg}", MessageDialogStyle.Affirmative, null);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.MyConnString)) // MySql 연결 (ConnString)으로
                {
                    // DB에 대한 현재 연결 상태 
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    // 실행할 쿼리문
                    string query = @"SELECT Id
                                          , PassWord
                                       FROM userdb
                                      WHERE Id = @Id
                                        AND Password = @Password;";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    // Parameters.Add => varchar 열인 경우 데이터의 길이와 타입을 명시하여 사용자입력을 엄격히 제한
                    // Parameters.AddValue => 데이터를 nvarchar 형태로 보냄
                    cmd.Parameters.AddWithValue("@Id", txtId.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Password);

                    // MySqlDataReader => 한줄씩 읽는 것
                    // MySqlDataAdapter => DataSet에 저장하는 방식이므로 여러개의 Table도 가능

                    // SqlCommand.ExecuteReader => CommandText(query)를 보내고, SQlDataReader을 반환함
                    // SqlDataReader => 데이터 베이스에서 행의 앞으로만 이동가능한 스트림을 읽을 수 있게함
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        strtxtId = reader["Id"].ToString();
                        strtxtPassword = reader["Password"].ToString();
                        Commons.Id = strtxtId;

                        // 로그인 할때 아무것도 넣지 않으면 안되게 처리해 놨기 때문에 필요 없음
                        //strtxtId = !string.IsNullOrEmpty(reader["Id"]?.ToString()) ? reader["Id"].ToString() : "-";
                        //strtxtPassword = !string.IsNullOrEmpty(reader["Password"]?.ToString()) ? reader["Password"].ToString() : "-";

                        Commons.isManager = true;
                        await this.ShowMessageAsync("로그인 성공!", "로그인 되었습니다.", MessageDialogStyle.Affirmative, null);
                        Commons.Islogin = true;
                        this.Close();
                                                                    }
                    else
                    {
                        txtId.Focus();
                        await this.ShowMessageAsync("로그인 실패", "회원 정보가 맞지 않습니다", MessageDialogStyle.Affirmative, null);
                        // txtId.Text = "";
                        txtPassword.Password = "";
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("로그인 오류", $"{ex.Message}", MessageDialogStyle.Affirmative, null);
            }
        }

        // 엔터 누르면 확인 버튼을 누름
        private void txtId_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnOk_Click(sender, e);
            }
        }

        // 엔터 누르면 확인 버튼을 누름
        private void txtPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnOk_Click(sender, e);
            }
        }

        // 화면 로드 되면 ID 입력란에 포커스가게 함
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtId.Focus();
        }

        // 회원가입 창으로 넘어가게 됨
        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            var join = new Join();
            join.Owner = this;
            join.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            join.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var adminLogin = new AdminLogin();
            adminLogin.Owner = this;
            adminLogin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            adminLogin.ShowDialog();
        }

     
    }
}


