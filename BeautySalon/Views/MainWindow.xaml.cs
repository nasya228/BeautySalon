using System;
using System.Windows;
using System.Windows.Controls;
using BeautySalon.Data;
using BeautySalon.Models;

namespace BeautySalon.Views
{
    /// <summary>Главное окно — управление услугами и записями.</summary>
    public partial class MainWindow : Window
    {
        private readonly ServiceRepository _serviceRepository = new ServiceRepository();
        private readonly ClientServiceRepository _appointmentRepository = new ClientServiceRepository();

        public MainWindow()
        {
            InitializeComponent();
            LoadServices();
            LoadUpcomingAppointments();
        }

        /// <summary>Загружает список услуг из БД.</summary>
        private void LoadServices()
        {
            try
            {
                ServicesList.ItemsSource = _serviceRepository.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Не удалось загрузить список услуг.\n\n{ex.Message}",
                    "Ошибка загрузки",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>Загружает ближайшие записи клиентов.</summary>
        private void LoadUpcomingAppointments()
        {
            try
            {
                UpcomingGrid.ItemsSource = _appointmentRepository.GetUpcoming();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Не удалось загрузить ближайшие записи.\n\n{ex.Message}",
                    "Ошибка загрузки",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ServiceEditWindow(null) { Owner = this };
            if (dialog.ShowDialog() == true)
                LoadServices();
        }

        private void EditServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var service = (Service)((Button)sender).Tag;
            var dialog = new ServiceEditWindow(service) { Owner = this };
            if (dialog.ShowDialog() == true)
                LoadServices();
        }

        private void DeleteServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var service = (Service)((Button)sender).Tag;
            var result = MessageBox.Show(
                $"Удалить услугу «{service.Title}»?\n\nДействие необратимо.",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _serviceRepository.Delete(service.Id);
                    LoadServices();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Не удалось удалить услугу.\n\n{ex.Message}",
                        "Ошибка удаления",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void AddAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AppointmentWindow { Owner = this };
            if (dialog.ShowDialog() == true)
                LoadUpcomingAppointments();
        }

        private void DeleteAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            var cs = (ClientService)((Button)sender).Tag;
            var result = MessageBox.Show(
                $"Удалить запись клиента «{cs.ClientFullName}» на «{cs.ServiceTitle}»?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _appointmentRepository.Delete(cs.Id);
                LoadUpcomingAppointments();
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadServices();
            LoadUpcomingAppointments();
        }
    }
}