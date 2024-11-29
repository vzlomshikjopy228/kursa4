using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using приложение.Services;
using приложение.ViewModels.WindowsViewModels;
namespace приложение.Views.Windows
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            var loginWindowViewModel = App.ServiceProvider.GetService<LoginWindowViewModel>();
            this.DataContext = loginWindowViewModel;
            var navigationService = App.ServiceProvider.GetService<INavigation>();
            navigationService?.SetFrame(MainFrame);
        }
    }
}
