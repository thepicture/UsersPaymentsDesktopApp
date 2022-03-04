using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UserPaymentsDesktopApp.Models.Entities;
using UserPaymentsDesktopApp.Services;

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
        /// <returns>Задача.</returns>
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

        /// <summary>
        /// Авторизует пользователя.
        /// </summary>
        private void Authorize(object sender,
                               RoutedEventArgs e)
        {
            if (ComboUsers.SelectedItem is User currentUser)
            {
                if (currentUser.Password == Password.Password)
                {
                    MessageBoxService.Inform("Вы успешно авторизованы, " +
                        $"{currentUser.FullName}");
                }
                else
                {
                    MessageBoxService.Warn("Неверный пароль");
                }
            }
            else
            {
                MessageBoxService.Warn("Не выбран пользователь в " +
                    "выпадающем списке");
            }
        }

        /// <summary>
        /// Выключает текущее приложение.
        /// </summary>
        private void Exit(object sender,
                          RoutedEventArgs e)
        {
            if (!MessageBoxService.Ask("Действительно выключить приложение?"))
            {
                return;
            }
            App.Current.Shutdown();
        }
    }
}
