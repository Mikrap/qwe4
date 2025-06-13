using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            LoadData();
        }
        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            // Показываем форму добавления клиента
            ClientEditFrame.Navigate(new AddClientPage(this));
            ClientEditFrame.Visibility = Visibility.Visible;
            ClientsDataGrid.Visibility = Visibility.Collapsed;
        }

        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem is Clients selectedClient)
            {
                // Показываем форму редактирования клиента
                ClientEditFrame.Navigate(new EditClientPage(selectedClient, this));
                ClientEditFrame.Visibility = Visibility.Visible;
                ClientsDataGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Please select a client to edit", "Warning",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem is Clients selectedClient)
            {
                if (MessageBox.Show($"Are you sure you want to delete {selectedClient.ClientFullName}?",
                                  "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        AppData.db.Clients.Remove(selectedClient);
                        AppData.db.SaveChanges();
                        LoadData(); // Обновляем данные
                        MessageBox.Show("Client deleted successfully", "Success",
                                      MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting client: {ex.Message}", "Error",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a client to delete", "Warning",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void CloseClientEditFrame()
        {
            ClientEditFrame.Visibility = Visibility.Collapsed;
            ClientsDataGrid.Visibility = Visibility.Visible;
            LoadData(); // Обновляем данные после закрытия формы
        }
        private void LoadData()
        {
            try
            {


                // Загрузка комнат с именами клиентов
                var roomsWithClients = AppData.db.Rooms
                    .Select(r => new
                    {
                        Room = r,
                        CurrentOrder = r.Order.FirstOrDefault() // Берем первый заказ
                    })
                    .ToList()
                    .Select(x => new
                    {
                        x.Room.RoomID,
                        x.Room.Floor,
                        x.Room.RoomNumber,
                        x.Room.RoomCategory,
                        x.Room.RoomStatus,
                        x.Room.Price,
                        ClientName = x.CurrentOrder?.Clients?.ClientFullName ?? "Нет клиента" // Имя клиента
                    })
                    .ToList();
                // Загрузка заказов с клиентами и комнатами (используем строковые параметры)
                var orders = AppData.db.Order
                    .Include("Clients")  // Используем строку вместо лямбды
                    .Include("Rooms")
                    .ToList();
                OrdersDataGrid.ItemsSource = orders;
              

                RoomsDataGrid.ItemsSource = roomsWithClients;
                // Загрузка комнат с именами клиентов
                var RoomsInfo = AppData.db.Rooms
                    .Select(r => new
                    {
                        Room = r,
                        CurrentOrder = r.Order.FirstOrDefault() // Берем первый заказ
                    })
                    .ToList()
                    .Select(x => new
                    {
                        x.Room.RoomID,
                        x.Room.Floor,
                        x.Room.RoomNumber,
                        x.Room.RoomCategory,
                        x.Room.RoomStatus,
                        x.Room.Price,
                    })
                    .ToList();

                RoomsEditorDataGrid.ItemsSource = RoomsInfo;


                // Загрузка клиентов
                var clients = AppData.db.Clients.ToList();
                ClientsDataGrid.ItemsSource = clients;

               
                // Загрузка карт
                var cards = AppData.db.Cards.ToList();
                CardsDataGrid.ItemsSource = cards;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}\n\n{ex.InnerException?.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            // Показываем форму добавления заказа
            OrderEditFrame.Navigate(new AddOrderPage(this));
            OrderEditFrame.Visibility = Visibility.Visible;
            OrdersDataGrid.Visibility = Visibility.Collapsed;
        }

        public void CloseOrderEditFrame()
        {
            OrderEditFrame.Visibility = Visibility.Collapsed;
            OrdersDataGrid.Visibility = Visibility.Visible;
            LoadData(); // Обновляем данные после закрытия формы
        }
        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            // Показываем форму добавления во Frame
            EditFrame.Navigate(new AddRoomPage(this));
            EditFrame.Visibility = Visibility.Visible;
            RoomsEditorDataGrid.Visibility = Visibility.Collapsed;
        }

        private void EditRoom_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int roomId)
            {
                var room = AppData.db.Rooms.FirstOrDefault(r => r.RoomID == roomId);
                if (room != null)
                {
                    // Показываем форму редактирования во Frame
                    EditFrame.Navigate(new EditRoomPage(room, this));
                    EditFrame.Visibility = Visibility.Visible;
                    RoomsEditorDataGrid.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void RefreshRooms_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        public void CloseEditFrame()
        {
            EditFrame.Visibility = Visibility.Collapsed;
            RoomsEditorDataGrid.Visibility = Visibility.Visible;
            LoadData(); // Обновляем данные после закрытия формы
        }
    }
}
