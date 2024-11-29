using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using приложение.Model;
using приложение.Services;
using приложение.Views.Pages;
using приложение.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;

namespace приложение.ViewModels.PagesViewModels
{
    public class ScheduleAddPageViewModel : BaseViewModel
    {
        public ScheduleAddPageViewModel(INavigation _navigationService, ClassViewModel classToAddSchedule, string _dayOfWeek)
        {
            SaveCommand = new RelayCommand(OnSaveButtonClick);
            CancelCommand = new RelayCommand(OnCancelButtonClick);
            LoadedCommand = new RelayCommand(OnPageLoaded);
            TeachersList = new ObservableCollection<Teachers>();
            SubjectsList = new ObservableCollection<Subjects>();
            _selectedClass = classToAddSchedule;
            navigationService = _navigationService;
            dayOfWeek = _dayOfWeek;
        }
        private INavigation navigationService;
        private string dayOfWeek;
        private ClassViewModel _selectedClass;
        private Teachers _selectedTeacher;
        public Teachers SelectedTeacher
        {
            get
            {
                return _selectedTeacher;
            }
            set
            {
                if(value != null)
                {
                    _selectedTeacher = value;
                    OnPropertyChanged(nameof(SelectedTeacher));
                    ReloadSubjects();
                }
            }
        }
        private Subjects _selectedSubject;
        public Subjects SelectedSubject
        {
            get
            {
                return _selectedSubject;
            }
            set 
            {
                if(value != null)
                {
                    _selectedSubject = value;
                    OnPropertyChanged(nameof(SelectedSubject));
                }
            }
        }
        public string InputTime { get; set; }
        public ObservableCollection<Teachers> TeachersList { get; set; }
        public ObservableCollection<Subjects> SubjectsList { get; set; }
        public ICommand LoadedCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        private void OnSaveButtonClick()
        {
            if(SelectedTeacher != null && SelectedSubject != null && !string.IsNullOrWhiteSpace(InputTime) && !string.IsNullOrWhiteSpace(dayOfWeek))
            {
                if (InputTime.Length == 1 && int.TryParse(InputTime, out int _))
                {
                    InputTime = $"{InputTime}:00";
                }
                TimeSpan parsedTime;
                if(TimeSpan.TryParse(InputTime, out parsedTime))
                {
                    using (School_Entities _context = new School_Entities())
                    {
                        _context.Schedules.Add(new Schedules
                        {
                            SubjectID = SelectedSubject.ID,
                            TeacherID = SelectedTeacher.ID,
                            ClassID = _selectedClass.ID,
                            Time = parsedTime,
                            DayOfWeek = dayOfWeek
                        });
                        try
                        {
                            _context.SaveChanges();
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    SchedulePage schedulePage = new SchedulePage();
                    schedulePage.DataContext = App.ServiceProvider.GetService<SchedulePageViewModel>();
                    navigationService.NavigateToPage(schedulePage);
                }
                else
                {
                    System.Windows.MessageBox.Show("Не удалось преобразовать строковое представление времени в TimeSpan");
                }
            }
        }
        private void OnCancelButtonClick()
        {

            SchedulePage schedulePage = new SchedulePage();
            schedulePage.DataContext = App.ServiceProvider.GetService<SchedulePageViewModel>();
            navigationService.NavigateToPage(schedulePage);
        }
        private void ReloadTeachers()
        {
            TeachersList.Clear();
            LoadTeachers();
        }
        private void ReloadSubjects()
        {
            SubjectsList.Clear();
            LoadSubjects();
        }
        private void LoadSubjects()
        {
            using (School_Entities _context = new School_Entities())
            {
                if(_context.Teachers.Count() > 0)
                {
                    var subjectsFromDB = _context.Teachers_Subjects.Where(subject => subject.TeacherID == SelectedTeacher.ID).ToList();
                    if (subjectsFromDB != null)
                    {
                        foreach (var subject in subjectsFromDB)
                        {
                            SubjectsList.Add(_context.Subjects.Find(subject.ID));
                        }
                    }
                }
            }
        }
        private void LoadTeachers()
        {
            using(School_Entities _context = new School_Entities())
            {
                foreach(var teacher in _context.Teachers)
                {
                    TeachersList.Add(teacher);
                }
            }
        }

        private void SetDefaultPropertyValue()
        {
            _selectedTeacher = TeachersList.FirstOrDefault();
        }
        private void OnPageLoaded()
        {
            LoadTeachers();
            SetDefaultPropertyValue();
            LoadSubjects();
        }
    }
}
