using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Demo.Classes;
using Demo.Model;

namespace Demo.Pages
{
    public partial class EditRoomPage : Page
    {
        private readonly AdminPage _adminPage;
        public Rooms CurrentRoom { get; set; }

        public EditRoomPage(Rooms room, AdminPage adminPage)
        {
            InitializeComponent();
            _adminPage = adminPage;
            CurrentRoom = room;
            DataContext = CurrentRoom;
            LoadStatusComboBox();
        }

        private void LoadStatusComboBox()
        {
            try
            {
                // Получаем уникальные статусы из базы данных
                var statuses = AppData.db.Rooms
                    .Select(r => r.RoomStatus)
                    .Distinct()
                    .ToList();

                // Очищаем ComboBox перед заполнением
                StatusComboBox.Items.Clear();

                // Добавляем статусы из базы данных
                foreach (var status in statuses)
                {
                    StatusComboBox.Items.Add(new ComboBoxItem { Content = status });
                }

                // Устанавливаем выбранный статус текущей комнаты
                foreach (ComboBoxItem item in StatusComboBox.Items)
                {
                    if (item.Content.ToString() == CurrentRoom.RoomStatus)
                    {
                        StatusComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке статусов: {ex.Message}",
                               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Обновляем статус из ComboBox
                if (StatusComboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    CurrentRoom.RoomStatus = selectedItem.Content.ToString();
                }

                AppData.db.Entry(CurrentRoom).State = EntityState.Modified;
                AppData.db.SaveChanges();

                _adminPage.CloseEditFrame();
                MessageBox.Show("Данные комнаты успешно обновлены", "Успех",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Откатываем изменения
            var entry = AppData.db.Entry(CurrentRoom);
            if (entry.State == EntityState.Modified)
            {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
            }
            _adminPage.CloseEditFrame();
        }
    }
}