using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UserPaymentsDesktopApp.Models.Entities;
using UserPaymentsDesktopApp.Services;
using UserPaymentsDesktopApp.Views.Windows;

namespace UserPaymentsDesktopApp.Views.Pages
{
    /// <summary>
    /// Interaction logic for PaymentsPage.xaml
    /// </summary>
    public partial class PaymentsPage : Page
    {
        public PaymentsPage()
        {
            InitializeComponent();
            Title = "Страница платежей " +
                $"пользователя {(App.Current as App).CurrentUser.FullName}";
            _ = LoadCategoriesAsync();
            _ = LoadPaymentsAsync();
        }

        /// <summary>
        /// Загружает категории асинхронно.
        /// </summary>
        /// <returns>Задача.</returns>
        private async Task LoadCategoriesAsync()
        {
            List<PaymentType> categories = await Task.Run(() =>
            {
                using (UserPaymentsBaseEntities context =
                    new UserPaymentsBaseEntities())
                {
                    return context
                    .PaymentType
                    .ToList();
                }
            });
            categories.Insert(0, new PaymentType
            {
                Name = "Все категории"
            });
            ComboCategories.ItemsSource = categories;
        }

        /// <summary>
        /// Загружает платежи асинхронно.
        /// </summary>
        /// <returns>Задача.</returns>
        private async Task LoadPaymentsAsync()
        {
            int userId = (App.Current as App).CurrentUser.Id;
            IEnumerable<PaymentOfUser> payments = await Task.Run(() =>
            {
                using (UserPaymentsBaseEntities context =
                    new UserPaymentsBaseEntities())
                {
                    return context
                    .PaymentOfUser
                    .Include(p => p.PaymentType)
                    .Where(p => p.UserId == userId)
                    .ToList();
                }
            });
            if (FromDate?.SelectedDate <= ToDate?.SelectedDate)
            {
                payments = payments.Where(p =>
                {
                    return p.PaymentDate > FromDate.SelectedDate
                    && p.PaymentDate < ToDate.SelectedDate;
                });
            }
            if (ComboCategories.SelectedIndex != 0)
            {
                payments = payments.Where(p =>
                {
                    return p.PaymentTypeId
                           == (ComboCategories.SelectedItem as PaymentType).Id;
                });
            }
            PaymentsGrid.ItemsSource = payments;
        }

        /// <summary>
        /// Перезагружает платежи.
        /// </summary>
        private async void ReloadPaymentsAsync(object sender,
                                         SelectionChangedEventArgs e)
        {
            await LoadPaymentsAsync();
        }

        /// <summary>
        /// Добавляет платёж.
        /// </summary>
        private async void OnAddPayment(object sender, RoutedEventArgs e)
        {
            AddPaymentWindow addPaymentWindow = new AddPaymentWindow
            {
                Owner = App.Current.MainWindow
            };
            if (addPaymentWindow.ShowDialog() != null)
            {
                await LoadPaymentsAsync();
            }
        }

        /// <summary>
        /// Удаляет платёж.
        /// </summary>
        private async void RemovePayment(object sender, RoutedEventArgs e)
        {
            if (PaymentsGrid.SelectedItem is null)
            {
                MessageBoxService.Warn("В таблице не выбран платёж " +
                    "для удаления. Выберите платёж");
                return;
            }
            PaymentOfUser payment = PaymentsGrid.SelectedItem as PaymentOfUser;
            if (!MessageBoxService.Ask($"Удалить платёж {payment.Name}?"))
            {
                return;
            }
            try
            {
                await Task.Run(() =>
                {
                    using (UserPaymentsBaseEntities context =
                    new UserPaymentsBaseEntities())
                    {
                        context.Entry(payment).State = EntityState.Deleted;
                        _ = context.SaveChanges();
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                MessageBoxService.InformError("Проверьте подключение к базе данных. " +
                    "Платёж не удалён");
                return;
            }
            MessageBoxService.Inform("Платёж успешно удалён");
            await LoadPaymentsAsync();
        }

        private async void ClearFiltersAsync(object sender, RoutedEventArgs e)
        {
            FromDate.SelectedDate = null;
            ToDate.SelectedDate = null;
            ComboCategories.SelectedIndex = 0;
            await LoadPaymentsAsync();
        }
    }
}
