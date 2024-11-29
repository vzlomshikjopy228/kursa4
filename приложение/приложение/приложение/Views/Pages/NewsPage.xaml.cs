using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using приложение.ViewModels.PagesViewModels;

namespace приложение.Views.Pages
{
    public partial class NewsPage : Page
    {
        public NewsPage()
        {
            InitializeComponent();
            var newsPageViewModel = App.ServiceProvider.GetService<NewsPageViewModel>();
            this.DataContext = newsPageViewModel;
        }
    }
}
