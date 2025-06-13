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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Demo.Classes;
using Demo.Model;

namespace Demo.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var CurrentUser = AppData.db.Users.FirstOrDefault(u => u.UserName == txtLogin.Text && u.Password == txtPassword.Password);
            if (CurrentUser == null)
            {
                MessageBox.Show("Такого пользователя нет в базе данных");
            }
            else
            {
                switch (CurrentUser.RoleID)
                {
                    case 1:
                        NavigationService.Navigate(new AdminPage());
                        break;
                    case 2:
                        NavigationService.Navigate(new UserPage());
                        break;
                }
            }
        }
    }
}
