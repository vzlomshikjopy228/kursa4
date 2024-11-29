using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using приложение.ViewModels.PagesViewModels;

namespace приложение.Views.Pages
{
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
            var registrationPageViewModel = App.ServiceProvider.GetService<RegistrationPageViewModel>();
            this.DataContext = registrationPageViewModel;
        }
    }
}
