using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using приложение.ViewModels.PagesViewModels;
using приложение.ViewModels.WindowsViewModels;

namespace приложение.Services
{
    public static class AppServices
    {
        public static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<INavigation, NavigationService>();
            serviceCollection.AddSingleton<LoginWindowViewModel>();
            serviceCollection.AddSingleton<LoginPageViewModel>();
            serviceCollection.AddSingleton<HomeWindowViewModel>();
            serviceCollection.AddTransient<HomeworkPageViewModel>();
            serviceCollection.AddSingleton<RegistrationPageViewModel>();
            serviceCollection.AddTransient<NewsPageViewModel>();
            serviceCollection.AddTransient<GradesPageViewModel>();
            serviceCollection.AddSingleton<WeekNavigator>();
            serviceCollection.AddSingleton<SchedulePageViewModel>();
            serviceCollection.AddSingleton<ScheduleAddPageViewModel>();
            serviceCollection.AddSingleton<IHashingService, HashingService>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
