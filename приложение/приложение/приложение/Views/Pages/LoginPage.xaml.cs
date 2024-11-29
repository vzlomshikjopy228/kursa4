using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using приложение.Services;
using приложение.ViewModels.PagesViewModels;
namespace приложение.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            var loginPageViewModel = App.ServiceProvider.GetService<LoginPageViewModel>();
            this.DataContext = loginPageViewModel;
            var navigationService = App.ServiceProvider.GetService<INavigation>();
        }
    }
}
