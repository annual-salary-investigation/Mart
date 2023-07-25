using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows;

namespace MartApp.Logics
{
    public class Commons
    {

        public static bool Islogin = false;
        public static bool isManager = false;
        public static string Id = string.Empty;
        public static bool Loging = false;
        // DB 연결 (MySQL)
        public static readonly string MyConnString = "Server=210.119.12.72;" +
                                                     "Port=3306;" +
                                                     "Database=mart;" +
                                                     "Uid=root;" + 
                                                     "Pwd=12345;";

        public static async Task<MessageDialogResult> ShowMessageAsync(string title, string message,
                                 MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            return await ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync(title, message, style, null);
        }

        public static void CartWindowShow(int productId)
        {
            var detailWindow = new Views.DetailWindow(productId);
            detailWindow.ShowDialog();
        }
    }
}
