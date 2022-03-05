using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UserPaymentsDesktopApp.Models.Entities;
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
    }
}
