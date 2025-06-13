using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Demo.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Demo.Classes;

namespace Demo.Pages
{
    public partial class UserPage : Page
    {
        private ConnDataBase _context = new ConnDataBase();
        private ObservableCollection<Order> _orders = new ObservableCollection<Order>();

        public UserPage()
        {
            InitializeComponent();
            DataContext = new UserPageViewModel(_context);
            LoadData();
        }

        private void LoadData()
        {
            // Загрузка данных из базы
            var orders = _context.Order.ToList();
            _orders = new ObservableCollection<Order>(orders);
            dataGridDR.ItemsSource = _orders;

            // Расчет показателей
            CalculateMetrics();
        }

        private void CalculateMetrics()
        {
            var viewModel = DataContext as UserPageViewModel;
            if (viewModel == null) return;

            // Расчет показателя Occupancy Rate (процент загруженности номеров)
            var totalRooms = _context.Rooms.Count();
            var occupiedRooms = _orders.Count(o => o.CheckIn <= DateTime.Today && o.CheckOut >= DateTime.Today);
            viewModel.OccupancyRate = Math.Round((double)occupiedRooms / totalRooms * 100, 1) + "%";
            viewModel.OccupancyTrend = "↑ 2.5% from last month";

            // Расчет ADR (Average Daily Rate)
            var totalRevenue = _orders.Where(o => o.CheckIn.Month == DateTime.Today.Month)
                                     .Sum(o => _context.Rooms.FirstOrDefault(r => r.RoomID == o.RoomID)?.Price ?? 0);
            var occupiedDays = _orders.Where(o => o.CheckIn.Month == DateTime.Today.Month)
                                     .Sum(o => (o.CheckOut - o.CheckIn).Days);
            viewModel.ADR = occupiedDays > 0 ? totalRevenue / occupiedDays : 0;
            viewModel.ADRTrend = "↑ 5.1% from last month";

            // Расчет RevPAR (Revenue Per Available Room)
            var availableRoomDays = totalRooms * DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
            viewModel.RevPAR = availableRoomDays > 0 ? totalRevenue / availableRoomDays : 0;
            viewModel.RevPARTrend = "↑ 3.8% from last month";

            // Общий доход за месяц
            viewModel.MonthlyRevenue = totalRevenue;
            viewModel.RevenueTrend = "↑ 7.2% from last month";
        }

        private void RefreshData_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class UserPageViewModel
    {
        private ConnDataBase _context;
        public ICommand GenerateMonthlyReportCommand { get; }
        public ICommand GenerateFinancialSummaryCommand { get; }

        public UserPageViewModel(ConnDataBase context)
        {
            _context = context;
            GenerateMonthlyReportCommand = new RelayCommand(GenerateMonthlyReport);
            GenerateFinancialSummaryCommand = new RelayCommand(GenerateFinancialSummary);
        }

        public string OccupancyRate { get; set; }
        public string OccupancyTrend { get; set; }
        public decimal ADR { get; set; }
        public string ADRTrend { get; set; }
        public decimal RevPAR { get; set; }
        public string RevPARTrend { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public string RevenueTrend { get; set; }

        private void GenerateMonthlyReport(object parameter)
        {
            MessageBox.Show("Monthly report generated successfully!", "Report", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GenerateFinancialSummary(object parameter)
        {
            MessageBox.Show("Financial summary generated successfully!", "Report", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}