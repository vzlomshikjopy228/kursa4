using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace приложение.Views.Pages
{
    public partial class ScheduleAddPage : Page
    {
        public ScheduleAddPage()
        {
            InitializeComponent();
            var scheduleAddPageViewModel = App.ServiceProvider.GetService<ScheduleAddPage>();
            this.DataContext = scheduleAddPageViewModel;
        }
    }
}
