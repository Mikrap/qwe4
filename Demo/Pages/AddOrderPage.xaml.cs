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
    /// Логика взаимодействия для AddOrderPage.xaml
    /// </summary>
    public partial class AddOrderPage : Page
    {
        private readonly AdminPage _adminPage;

        public AddOrderPage(AdminPage adminPage)
        {
            InitializeComponent();
            _adminPage = adminPage;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Загрузка клиентов
                ClientComboBox.ItemsSource = AppData.db.Clients.ToList();

                // Загрузка доступных комнат (статус "Чистый")
                RoomComboBox.ItemsSource = AppData.db.Rooms
                    .Where(r => r.RoomStatus == "Чистый")
                    .ToList();

                // Установка дат по умолчанию
                CheckInDatePicker.SelectedDate = DateTime.Today;
                CheckOutDatePicker.SelectedDate = DateTime.Today.AddDays(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка выбора клиента
                if (ClientComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Выберите клиента");
                    return;
                }

                // Проверка выбора комнаты
                if (RoomComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Выберите комнату");
                    return;
                }

                // Проверка дат
                if (CheckInDatePicker.SelectedDate == null ||
                    CheckOutDatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Укажите даты заезда и выезда");
                    return;
                }

                if (CheckInDatePicker.SelectedDate >= CheckOutDatePicker.SelectedDate)
                {
                    MessageBox.Show("Дата выезда должна быть позже даты заезда");
                    return;
                }

                // Получаем выбранные данные
                var client = (Clients)ClientComboBox.SelectedItem;
                var room = (Rooms)RoomComboBox.SelectedItem;
                var checkIn = CheckInDatePicker.SelectedDate.Value;
                var checkOut = CheckOutDatePicker.SelectedDate.Value;

                // Проверяем доступность комнаты
                bool isRoomAvailable = !AppData.db.Order.Any(o =>
                    o.RoomID == room.RoomID &&
                    ((checkIn >= o.CheckIn && checkIn < o.CheckOut) ||
                     (checkOut > o.CheckIn && checkOut <= o.CheckOut) ||
                     (checkIn <= o.CheckIn && checkOut >= o.CheckOut)));

                if (!isRoomAvailable)
                {
                    MessageBox.Show("Комната уже занята на выбранные даты");
                    return;
                }

                // Создаем новый заказ
                var newOrder = new Order
                {
                    ClientID = client.ClientID,
                    RoomID = room.RoomID,
                    CheckIn = checkIn,
                    CheckOut = checkOut,
                    UserID = 5, // Укажите актуальный ID пользователя
                    CardID = 1  // Укажите актуальный ID карты или оставьте 0 если не требуется
                };

                // Обновляем статус комнаты
                room.RoomStatus = "Занят";

                // Сохраняем изменения
                AppData.db.Order.Add(newOrder);
                AppData.db.Entry(room).State = EntityState.Modified;
                AppData.db.SaveChanges();

                MessageBox.Show("Заказ успешно создан");
                _adminPage.CloseOrderEditFrame();
            }
            catch (Exception ex)
            {
                // Выводим полное сообщение об ошибке
                string errorMessage = ex.Message;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    errorMessage += "\n" + ex.Message;
                }
                MessageBox.Show($"Ошибка при создании заказа:\n{errorMessage}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _adminPage.CloseOrderEditFrame();
        }
    }
}

