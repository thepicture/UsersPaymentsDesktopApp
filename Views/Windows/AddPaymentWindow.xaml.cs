using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UserPaymentsDesktopApp.Models.Entities;
using UserPaymentsDesktopApp.Services;

namespace UserPaymentsDesktopApp.Views.Windows
{
    /// <summary>
    /// Interaction logic for AddPaymentWindow.xaml
    /// </summary>
    public partial class AddPaymentWindow : Window
    {
        public PaymentOfUser CurrentPayment { get; set; }
            = new PaymentOfUser();
        public PaymentType CurrentType { get; set; }
        public AddPaymentWindow()
        {
            InitializeComponent();
            DataContext = this;
            _ = LoadCategoriesAsync();
        }

        /// <summary>
        /// Подгружает категории платежей.
        /// </summary>
        private async Task LoadCategoriesAsync()
        {
            List<PaymentType> categories = await Task.Run(() =>
            {
                using (UserPaymentsBaseEntities context =
                    new UserPaymentsBaseEntities())
                {
                    return context.PaymentType.ToList();
                }
            });
            ComboCategories.ItemsSource = categories;
            ComboCategories.SelectedItem = categories.First();
        }

        /// <summary>
        /// Сохраняет платёж в базе данных.
        /// </summary>
        private async void SavePaymentAsync(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (CurrentPayment == null)
            {
                _ = errors.AppendLine("Укажите тип платежа");
            }
            if (string.IsNullOrWhiteSpace(PaymentName.Text))
            {
                _ = errors.AppendLine("Укажите наименование платежа");
            }
            if (string.IsNullOrWhiteSpace(PaymentCount.Text)
                || !int.TryParse(PaymentCount.Text, out _)
                || int.Parse(PaymentCount.Text) <= 0)
            {
                _ = errors.AppendLine("Количество - это положительное целое число");
            }
            if (string.IsNullOrWhiteSpace(Price.Text)
              || !decimal.TryParse(Price.Text, out _)
              || decimal.Parse(Price.Text) <= 0)
            {
                _ = errors.AppendLine("Цена - это десятичное положительное число");
            }
            if (errors.Length > 0)
            {
                MessageBoxService.Warn(errors.ToString());
                return;
            }
            CurrentPayment.UserId = (App.Current as App)
                .CurrentUser
                .Id;
            CurrentPayment.PaymentTypeId = CurrentType.Id;
            CurrentPayment.PaymentDate = DateTime.Now;

            try
            {
                await Task.Run(() =>
                {
                    using (UserPaymentsBaseEntities context =
                    new UserPaymentsBaseEntities())
                    {
                        _ = context.PaymentOfUser.Add(CurrentPayment);
                        _ = context.SaveChanges();
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                MessageBoxService.Inform("Не удалось сохранить платёж. " +
                    "Проверьте подключение к базе данных");
                return;
            }
            MessageBoxService.Inform("Платёж успешно сохранён!");
            Close();
        }

        /// <summary>
        /// Отменяет создание платежа.
        /// </summary>
        private void Cancel(object sender, RoutedEventArgs e)
        {
            if (MessageBoxService.Ask("Отменить создание " +
                "платежа?"))
            {
                Close();
            }
        }
    }
}
