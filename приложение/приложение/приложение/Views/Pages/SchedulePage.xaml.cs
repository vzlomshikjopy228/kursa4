using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using приложение.ViewModels.PagesViewModels;
using приложение.Services;

namespace приложение.Views.Pages
{
    public partial class SchedulePage : Page 
    {
        public SchedulePage()
        {
            InitializeComponent();
            var schedulePageViewModel = App.ServiceProvider.GetService<SchedulePageViewModel>();
            this.DataContext = schedulePageViewModel;
            var navigationService = App.ServiceProvider.GetService<INavigation>();
        }
    }
}
