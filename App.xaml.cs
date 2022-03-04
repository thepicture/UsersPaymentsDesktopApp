using System.Windows;
using UserPaymentsDesktopApp.Models.Entities;

namespace UserPaymentsDesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public User CurrentUser { get; set; }
    }
}
