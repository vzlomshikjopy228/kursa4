using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using приложение.ViewModels.PagesViewModels;

namespace приложение.Views.Pages
{
    public partial class HomeWorkPage : Page
    {
        public HomeWorkPage()
        {
            InitializeComponent();
            var homeworkWindowViewModel = App.ServiceProvider.GetService<HomeworkPageViewModel>();
            this.DataContext = homeworkWindowViewModel;
        }
    }
}
