using System.Windows;
using UserPaymentsDesktopApp.Views.Pages;

namespace UserPaymentsDesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class NavigationWindow : Window
    {
        public NavigationWindow()
        {
            InitializeComponent();
            _ = MainFrame.Navigate(new LoginPage());
        }
    }
}
