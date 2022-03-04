using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using UserPaymentsDesktopApp.Models.Entities;

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
            _ = LoadPaymentsAsync();
        }

        /// <summary>
        /// Загружает платежи асинхронно.
        /// </summary>
        /// <returns>Задача.</returns>
        private async Task LoadPaymentsAsync()
        {
            int userId = (App.Current as App).CurrentUser.Id;
            PaymentsGrid.ItemsSource = await Task.Run(() =>
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
        }
    }
}
