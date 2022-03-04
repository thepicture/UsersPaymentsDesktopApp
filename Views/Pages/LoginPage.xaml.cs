using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using UserPaymentsDesktopApp.Models.Entities;

namespace UserPaymentsDesktopApp.Views.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            _ = LoadLoginsAsync();
        }

        /// <summary>
        /// Подгружает логины асинхронно.
        /// </summary>
        /// <returns></returns>
        private async Task LoadLoginsAsync()
        {
            ComboUsers.ItemsSource = await Task.Run(() =>
            {
                using (UserPaymentsBaseEntities context =
                new UserPaymentsBaseEntities())
                {
                    return context
                    .User
                    .AsNoTracking()
                    .ToList();
                }
            });
        }
    }
}
