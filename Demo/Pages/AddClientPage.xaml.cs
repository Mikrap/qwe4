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
    /// Логика взаимодействия для AddClientPage.xaml
    /// </summary>
    public partial class AddClientPage : Page
    {
        private readonly AdminPage _adminPage;

        public AddClientPage(AdminPage adminPage)
        {
            InitializeComponent();
            _adminPage = adminPage;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
            {
                MessageBox.Show("Please enter full name");
                return;
            }
            if (!Int32.TryParse(PassportTextBox.Text, out Int32 Floor))
            {
                MessageBox.Show("Введите номер паспорта");
                return;
            }

            try
            {
                var newClient = new Clients
                {
                    ClientFullName = FullNameTextBox.Text,
                    Address = AddressTextBox.Text,
                    Password = Convert.ToInt32(PassportTextBox.Text)
                };

                AppData.db.Clients.Add(newClient);
                AppData.db.SaveChanges();

                _adminPage.CloseClientEditFrame();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding client: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _adminPage.CloseClientEditFrame();
        }
    }
}
