using System;
using System.Windows;
using BeautySalon.Data;
using BeautySalon.Models;

namespace BeautySalon.Views
{
    /// <summary>Окно добавления/редактирования услуги.</summary>
    public partial class ServiceEditWindow : Window
    {
        private readonly ServiceRepository _repository = new ServiceRepository();
        private readonly Service _service;
        private readonly bool _isNew;

        public ServiceEditWindow(Service service)
        {
            InitializeComponent();
            _isNew = service == null;
            _service = service ?? new Service();

            Title = _isNew ? "Услуга — добавление" : "Услуга — редактирование";

            if (!_isNew)
            {
                TitleBox.Text = _service.Title;
                CostBox.Text = _service.Cost.ToString("0.##");
                DurationBox.Text = _service.DurationInSeconds.ToString();
                DiscountBox.Text = _service.Discount?.ToString("0.##") ?? string.Empty;
                ImagePathBox.Text = _service.MainImagePath;
                UpdatePreview();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs(out string error))
            {
                MessageBox.Show(error, "Проверьте ввод",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _service.Title = TitleBox.Text.Trim();
            _service.Cost = decimal.Parse(CostBox.Text);
            _service.DurationInSeconds = int.Parse(DurationBox.Text);
            _service.Discount = string.IsNullOrWhiteSpace(DiscountBox.Text)
                ? (double?)null
                : double.Parse(DiscountBox.Text);
            _service.MainImagePath = string.IsNullOrWhiteSpace(ImagePathBox.Text)
                ? null
                : ImagePathBox.Text.Trim();

            try
            {
                if (_isNew)
                    _repository.Add(_service);
                else
                    _repository.Update(_service);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Не удалось сохранить услугу.\n\n{ex.Message}",
                    "Ошибка сохранения",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>Проверяет корректность введённых значений.</summary>
        private bool ValidateInputs(out string error)
        {
            error = null;

            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                error = "Введите наименование услуги.";
                return false;
            }
            if (TitleBox.Text.Length > 100)
            {
                error = "Наименование не должно превышать 100 символов.";
                return false;
            }
            if (!decimal.TryParse(CostBox.Text, out var cost) || cost <= 0)
            {
                error = "Стоимость должна быть положительным числом.";
                return false;
            }
            if (!int.TryParse(DurationBox.Text, out var dur) || dur <= 0)
            {
                error = "Длительность должна быть положительным числом секунд.";
                return false;
            }
            if (!string.IsNullOrWhiteSpace(DiscountBox.Text))
            {
                if (!double.TryParse(DiscountBox.Text, out var d) || d < 0 || d > 100)
                {
                    error = "Скидка должна быть числом от 0 до 100.";
                    return false;
                }
            }
            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        /// <summary>Обновляет превью изображения по пути.</summary>
        private void UpdatePreview()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(ImagePathBox.Text))
                {
                    PreviewImage.Source = new System.Windows.Media.Imaging.BitmapImage(
                        new Uri(ImagePathBox.Text, UriKind.Relative));
                }
            }
            catch
            {
                PreviewImage.Source = null;
            }
        }
    }
}