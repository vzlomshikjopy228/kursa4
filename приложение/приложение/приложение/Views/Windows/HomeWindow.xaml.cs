using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using приложение.ViewModels.WindowsViewModels;
using приложение.Services;

namespace приложение.Views.Windows
{
    public partial class HomeWindow : Window
    {
        public HomeWindow() 
        {
            InitializeComponent();
            var homeWindowViewModel = App.ServiceProvider.GetService<HomeWindowViewModel>();
            this.DataContext = homeWindowViewModel;
            var navigationService = App.ServiceProvider.GetService<INavigation>();
            navigationService.SetFrame(MainFrame);
        }
    }
}
