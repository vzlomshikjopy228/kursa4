using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using приложение.ViewModels.PagesViewModels;

namespace приложение.Views.Pages
{
    public partial class GradesPage : Page
    {
        public GradesPage()
        {
            InitializeComponent();
            var gradesPageViewModel = App.ServiceProvider.GetService<GradesPageViewModel>();
            this.DataContext = gradesPageViewModel;
        }
    }
}
