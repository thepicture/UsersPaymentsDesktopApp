using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UserPaymentsDesktopApp.Models.Entities;

namespace UserPaymentsDesktopApp.Views.Windows
{
    /// <summary>
    /// Interaction logic for AddPaymentWindow.xaml
    /// </summary>
    public partial class AddPaymentWindow : Window
    {
        public AddPaymentWindow()
        {
            InitializeComponent();
            _ = LoadCategoriesAsync();
        }

        /// <summary>
        /// Подгружает категории платежей.
        /// </summary>
        private async Task LoadCategoriesAsync()
        {
            ComboCategories.ItemsSource = await Task.Run(() =>
            {
                using (UserPaymentsBaseEntities context =
                    new UserPaymentsBaseEntities())
                {
                    return context.PaymentType.ToList();
                }
            });
        }
    }
}
