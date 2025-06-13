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
using Demo.Classes;
using Demo.Model;

namespace Demo.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditClientPage.xaml
    /// </summary>
    public partial class EditClientPage : Page
    {
        private readonly AdminPage _adminPage;
        public Clients CurrentClient { get; set; }

        public EditClientPage(Clients client, AdminPage adminPage)
        {
            InitializeComponent();
            _adminPage = adminPage;
            CurrentClient = client;
            DataContext = CurrentClient;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AppData.db.Entry(CurrentClient).State = EntityState.Modified;
                AppData.db.SaveChanges();

                _adminPage.CloseClientEditFrame();
                MessageBox.Show("Client updated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving client: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var entry = AppData.db.Entry(CurrentClient);
            if (entry.State == EntityState.Modified)
            {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
            }
            _adminPage.CloseClientEditFrame();
        }
    }
}
