using System;
using System.Windows.Input;
using приложение.Views.Pages;
using приложение.Commands;
using приложение.Services;

namespace приложение.ViewModels.WindowsViewModels
{
    public class HomeWindowViewModel
    {
        public string Title { get; set; }
        private readonly INavigation _navigationService;
        public ICommand NewsCommand { get; }
        public ICommand GradesCommand { get; }
        public ICommand ScheduleCommand { get; }
        public ICommand HomeworkCommand { get; }
        public ICommand HomeCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand LoadedCommand { get; }
        public HomeWindowViewModel(INavigation navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            NewsCommand = new RelayCommand(OpenNewsPage);
            GradesCommand = new RelayCommand(OpenGradesPage);
            ScheduleCommand = new RelayCommand(OpenSchedulePage);
            HomeworkCommand = new RelayCommand(OpenHomeworkPage);
            HomeCommand = new RelayCommand(OpenHomePage);
            LoadedCommand = new RelayCommand(Window_Loaded);
            RegisterCommand = new RelayCommand(OpenRegistrationPage);
        }

        private void OpenNewsPage()
        {
            var newsPage = new NewsPage();
            _navigationService.NavigateToPage(newsPage);
        }

        private void OpenGradesPage()
        {
            var gradesPage = new GradesPage();
            _navigationService.NavigateToPage(gradesPage);
        }

        private void OpenSchedulePage()
        {
            var schedulePage = new SchedulePage();
            _navigationService.NavigateToPage(schedulePage);
        }

        private void OpenHomeworkPage()
        {
            var homeworkPage = new HomeWorkPage();
            _navigationService.NavigateToPage(homeworkPage);
        }

        private void OpenHomePage()
        {
            var homePage = new HomePage();
            _navigationService.NavigateToPage(homePage);
        }
        
        private void OpenRegistrationPage()
        {
            var registrationPage = new RegistrationPage();
            _navigationService.NavigateToPage(registrationPage);
        }
        private void Window_Loaded()
        {
            OpenHomePage();
        }
    }
}
