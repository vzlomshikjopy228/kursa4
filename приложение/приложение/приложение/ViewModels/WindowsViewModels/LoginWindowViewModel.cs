using System.Windows.Input;
using приложение.Commands;
using приложение.Services;
using приложение.Views.Pages;

namespace приложение.ViewModels.WindowsViewModels
{
    public class LoginWindowViewModel
    {
        private INavigation _navigationService;
        public ICommand LoadedCommand { get; }
        public LoginWindowViewModel(INavigation navigationService)
        {
            _navigationService = navigationService;
            LoadedCommand = new RelayCommand(WindowLoaded);
        }
        public void OpenLoginPage()
        {
            var loginPage = new LoginPage();
            _navigationService.NavigateToPage(loginPage);
        }
        public void WindowLoaded()
        {
            OpenLoginPage();
        }
    }
}
