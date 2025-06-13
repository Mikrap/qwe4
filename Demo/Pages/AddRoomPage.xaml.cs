using System;
using System.Windows;
using System.Windows.Controls;
using Demo.Classes;
using Demo.Model;

namespace Demo.Pages
{
    public partial class AddRoomPage : Page
    {
        private readonly AdminPage _adminPage; // Объявляем поле

        public AddRoomPage(AdminPage adminPage)
        {
            InitializeComponent();
            _adminPage = adminPage; // Инициализируем поле
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CategoryTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите категорию");
                    return;
                }

                if (!int.TryParse(RoomNumberTextBox.Text, out int roomNumber))
                {
                    MessageBox.Show("Номер комнаты должен быть числом");
                    return;
                }

                if (!decimal.TryParse(PriceTextBox.Text, out decimal price))
                {
                    MessageBox.Show("Цена должна быть числом");
                    return;
                }
                if (!Int32.TryParse(FloorTextBox.Text, out Int32 Floor))
                {
                    MessageBox.Show("Введите номер этажа");
                    return;
                }

                var newRoom = new Rooms
                {
                    RoomNumber = roomNumber,
                    Floor =Convert.ToInt32(FloorTextBox.Text),
                    RoomCategory = CategoryTextBox.Text,
                    RoomStatus = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    Price = price
                };

                AppData.db.Rooms.Add(newRoom);
                AppData.db.SaveChanges();

                _adminPage.CloseEditFrame();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении комнаты: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _adminPage.CloseEditFrame();
        }
    }
}