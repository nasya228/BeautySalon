using System;
using System.Windows;
using BeautySalon.Data;
using BeautySalon.Models;

namespace BeautySalon.Views
{
    /// <summary>Окно записи клиента на услугу.</summary>
    public partial class AppointmentWindow : Window
    {
        private readonly ClientRepository _clientRepo = new ClientRepository();
        private readonly ServiceRepository _serviceRepo = new ServiceRepository();
        private readonly ClientServiceRepository _appointmentRepo = new ClientServiceRepository();

        public AppointmentWindow()
        {
            InitializeComponent();

            try
            {
                ClientBox.ItemsSource = _clientRepo.GetAll();
                ServiceBox.ItemsSource = _serviceRepo.GetAll();
                DatePicker.SelectedDate = DateTime.Today;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Не удалось загрузить данные для записи.\n\n{ex.Message}",
                    "Ошибка загрузки",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if ((ClientBox.SelectedItem is  Client client) == false)
            {
                MessageBox.Show("Выберите клиента.", "Проверьте ввод",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if ((ServiceBox.SelectedItem is  Service service) == false)
            {
                MessageBox.Show("Выберите услугу.", "Проверьте ввод",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (DatePicker.SelectedDate == null)
            {
                MessageBox.Show("Укажите дату записи.", "Проверьте ввод",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!TimeSpan.TryParse(TimeBox.Text, out var time))
            {
                MessageBox.Show("Время должно быть в формате ЧЧ:ММ.",
                    "Проверьте ввод",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var startTime = DatePicker.SelectedDate.Value.Date + time;

            try
            {
                _appointmentRepo.Add(new ClientService
                {
                    ClientID = client.Id,
                    ServiceID = service.Id,
                    StartTime = startTime,
                    Comment = string.IsNullOrWhiteSpace(CommentBox.Text)
                        ? null
                        : CommentBox.Text.Trim()
                });

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Не удалось сохранить запись.\n\n{ex.Message}",
                    "Ошибка сохранения",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}