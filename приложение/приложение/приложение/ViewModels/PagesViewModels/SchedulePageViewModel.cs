using GalaSoft.MvvmLight.Command;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using приложение.Model;
using приложение.Services;
using приложение.ViewModels.Base;
using приложение.Views.Pages;

namespace приложение.ViewModels.PagesViewModels
{
    public class SchedulePageViewModel : BaseViewModel
    {
        public SchedulePageViewModel(INavigation navigationService)
        {
            _navigationService = navigationService;
            ClassesList = new ObservableCollection<ClassViewModel>();
            ScheduleMonday = new ObservableCollection<ScheduleViewModel>();
            ScheduleTuesday = new ObservableCollection<ScheduleViewModel>();
            ScheduleWednesday = new ObservableCollection<ScheduleViewModel>();
            ScheduleThursday = new ObservableCollection<ScheduleViewModel>();
            ScheduleFriday = new ObservableCollection<ScheduleViewModel>();
            ScheduleSaturday = new ObservableCollection<ScheduleViewModel>();
            PageLoadedCommand = new RelayCommand(OnPageLoaded);
            AddButtonCommand = new RelayCommand<Button>(OnAddButtonClick);
        }
        private INavigation _navigationService;
        public ICommand PageLoadedCommand { get; }
        public ICommand AddButtonCommand { get; }
        public ObservableCollection<ClassViewModel> ClassesList { get; set; }
        public ObservableCollection<ScheduleViewModel> ScheduleMonday { get; set; }
        public ObservableCollection<ScheduleViewModel> ScheduleTuesday { get; set; }
        public ObservableCollection<ScheduleViewModel> ScheduleWednesday { get; set; }
        public ObservableCollection<ScheduleViewModel> ScheduleThursday { get; set; }
        public ObservableCollection<ScheduleViewModel> ScheduleFriday { get; set; }
        public ObservableCollection<ScheduleViewModel> ScheduleSaturday { get; set; }
        public ObservableCollection<ScheduleViewModel> SelectedScheduleDay { get; set; }
        private ClassViewModel _selectedClass;
        public ClassViewModel SelectedClass
        {
            get
            {
                return _selectedClass;
            }
            set
            {
                if(value != null)
                {
                    _selectedClass = value;
                    OnPropertyChanged(nameof(SelectedClass));
                    ReloadSchedule();
                }
            }
        }
        private void LoadClasses()
        {
            ClassesList.Clear();
            using (School_Entities _context = new School_Entities())
            {
                foreach(var classDB in _context.Classes)
                {
                    ClassesList.Add(new ClassViewModel
                    {
                        ID = classDB.ID,
                        ClassNumber = classDB.ClassNumber,
                        ClassLetter = classDB.ClassLetter
                    });
                }
            }
        }
        private void LoadSchedule()
        {
            ClearAll();
            if(SelectedClass != null)
            {
                using(School_Entities _context = new School_Entities())
                {
                    foreach(var scheduleDB in _context.Schedules.Where(schedule => schedule.ClassID == SelectedClass.ID).OrderBy(schedule => schedule.Time))
                    {
                        switch (scheduleDB.DayOfWeek)
                        {
                            case "Понедельник":
                                ScheduleMonday.Add(new ScheduleViewModel
                                {
                                    ID = scheduleDB.ID,
                                    Time = scheduleDB.Time,
                                    DisplayTeacher = $"{scheduleDB.Teachers.FirstName} {scheduleDB.Teachers.SecondName} {scheduleDB.Teachers.Surname}",
                                    SubjectTitle = scheduleDB.Subjects.SubjectTitle
                                });
                                break;
                            case "Вторник":
                                ScheduleTuesday.Add(new ScheduleViewModel
                                {
                                    ID = scheduleDB.ID,
                                    Time = scheduleDB.Time,
                                    DisplayTeacher = $"{scheduleDB.Teachers.FirstName} {scheduleDB.Teachers.SecondName} {scheduleDB.Teachers.Surname}",
                                    SubjectTitle = scheduleDB.Subjects.SubjectTitle
                                });
                                break;
                            case "Среда":
                                ScheduleWednesday.Add(new ScheduleViewModel
                                {
                                    ID = scheduleDB.ID,
                                    Time = scheduleDB.Time,
                                    DisplayTeacher = $"{scheduleDB.Teachers.FirstName} {scheduleDB.Teachers.SecondName} {scheduleDB.Teachers.Surname}",
                                    SubjectTitle = scheduleDB.Subjects.SubjectTitle
                                });
                                break;
                            case "Четверт":
                                ScheduleThursday.Add(new ScheduleViewModel
                                {
                                    ID = scheduleDB.ID,
                                    Time = scheduleDB.Time,
                                    DisplayTeacher = $"{scheduleDB.Teachers.FirstName} {scheduleDB.Teachers.SecondName} {scheduleDB.Teachers.Surname}",
                                    SubjectTitle = scheduleDB.Subjects.SubjectTitle
                                });
                                break;
                            case "Пятница":
                                ScheduleFriday.Add(new ScheduleViewModel
                                {
                                    ID = scheduleDB.ID,
                                    Time = scheduleDB.Time,
                                    DisplayTeacher = $"{scheduleDB.Teachers.FirstName} {scheduleDB.Teachers.SecondName} {scheduleDB.Teachers.Surname}",
                                    SubjectTitle = scheduleDB.Subjects.SubjectTitle
                                });
                                break;
                            case "Суббота":
                                ScheduleSaturday.Add(new ScheduleViewModel
                                {
                                    ID = scheduleDB.ID,
                                    Time = scheduleDB.Time,
                                    DisplayTeacher = $"{scheduleDB.Teachers.FirstName} {scheduleDB.Teachers.SecondName} {scheduleDB.Teachers.Surname}",
                                    SubjectTitle = scheduleDB.Subjects.SubjectTitle
                                });
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        private void ReloadSchedule()
        {
            LoadSchedule();
        }
        private void OnPageLoaded()
        {
            LoadSchedule();
            LoadClasses();
        }
        private void ClearAll()
        {
            ScheduleMonday.Clear();
            ScheduleTuesday.Clear();
            ScheduleWednesday.Clear();
            ScheduleThursday.Clear();
            ScheduleFriday.Clear();
            ScheduleSaturday.Clear();
        }
        private void OnAddButtonClick(Button sender)
        {            
            if(SelectedClass != null)
            {
                string DayOfWeek;
                Button senderButton = sender as Button;
                if (senderButton != null)
                {
                    using (School_Entities _context = new School_Entities())
                    {
                        switch (senderButton.Name)
                        {
                            case "AddToMondayButton":
                                SelectedScheduleDay = ScheduleMonday;
                                DayOfWeek = "Понедельник";
                                break;
                            case "AddToTuesdayButton":
                                SelectedScheduleDay = ScheduleTuesday;
                                DayOfWeek = "Вторник";
                                break;
                            case "AddToWednesdayButton":
                                SelectedScheduleDay = ScheduleWednesday;
                                DayOfWeek = "Среда";
                                break;
                            case "AddToThursdayButton":
                                SelectedScheduleDay = ScheduleThursday;
                                DayOfWeek = "Четверг";
                                break;
                            case "AddToFridayButton":
                                SelectedScheduleDay = ScheduleFriday;
                                DayOfWeek = "Пятница";
                                break;
                            case "AddToSaturdayButton":
                                SelectedScheduleDay = ScheduleSaturday;
                                DayOfWeek = "Суббота";
                                break;
                            default:
                                DayOfWeek = "";
                                break;
                        }
                        if (SelectedScheduleDay != null)
                        {
                            var addSchedulePage = new ScheduleAddPage();
                            addSchedulePage.DataContext = new ScheduleAddPageViewModel(_navigationService, SelectedClass, DayOfWeek);
                            var navigationService = App.ServiceProvider.GetService<INavigation>();
                            navigationService.NavigateToPage(addSchedulePage);
                        }
                    }
                }
            }         
        }
    }
}
