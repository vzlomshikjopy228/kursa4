using System.Linq;
using System.Windows.Input;
using приложение.ViewModels.Base;
using приложение.Commands;
using приложение.Services;
using приложение.Views.Windows;
using приложение.Model;

namespace приложение.ViewModels.PagesViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private INavigation _navigationService;
        private readonly IHashingService _hashingService;
        public ICommand LoginCommand { get; }
        public ICommand PasswordBoxCommand { get; }
        public string Email { get; set; }
        public string Password { get; set; }
        public LoginPageViewModel(INavigation navigationService, IHashingService hashingService)
        {
            _navigationService = navigationService;
            _hashingService = hashingService;
            LoginCommand = new RelayCommand(OpenHomeWindow); 
            PasswordBoxCommand = new RelayCommandGenerics<string>(PasswordBox_Changed);
        }
        public void OpenHomeWindow()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                System.Windows.MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }
            byte[] hashedPassword = _hashingService.HashPassword(Password);
            using (var context = new School_Entities())
            {
                var user = context.Accounts.FirstOrDefault(u => u.Email == Email);
                if (user != null && _hashingService.VerifyPassword(user.Password, hashedPassword))
                {
                    CurrentUser.Instance(user.ID, user.Roles.RoleTitle);
                    var homeWindow = new HomeWindow();
                    App.Current.MainWindow.Close();
                    homeWindow.Show();
                }
                else
                {
                    System.Windows.MessageBox.Show("Неверный email или пароль.");
                }
            }
        }
        public void PasswordBox_Changed(string password)
        {
            Password = password;
        }
    }
}
