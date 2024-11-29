using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using приложение.ViewModels.WindowsViewModels;
using приложение.Services;
using приложение.Views.Windows;
using приложение.ViewModels.PagesViewModels;
using приложение.Views.Pages;

namespace приложение
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            ServiceProvider = AppServices.ConfigureServices();
        }
    }
}
