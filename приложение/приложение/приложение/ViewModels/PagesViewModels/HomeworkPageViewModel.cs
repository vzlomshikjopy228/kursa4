using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;
using приложение.Model;
using приложение.Services;
using приложение.ViewModels.Base;

namespace приложение.ViewModels.PagesViewModels
{
    public class HomeworkPageViewModel : BaseViewModel
    {
        public ObservableCollection<ClassViewModel> ClassViewModelList { get; set; }
        private string _inputDescription;
        public string InputDescription
        {
            get
            {
                return _inputDescription;
            }
            set
            {
                _inputDescription = value;
                OnPropertyChanged(nameof(InputDescription));
            }
        }
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                OnDateSelectionChanged();
            }
        }
        public ObservableCollection<Subjects> SubjectsList { get; set; }
        public ObservableCollection<DateTime> WeekDates { get; set; }
        public ObservableCollection<Quarter> Quarters { get; set; }
        public ObservableCollection<Homeworks> HomeworksList { get; set; }
        private Quarter selectedQuarter;
        public Quarter SelectedQuarter
        {
            get
            {
                return selectedQuarter;
            }
            set
            {
                selectedQuarter = value;
                WeekDates.Clear();
                OnPropertyChanged(nameof(SelectedQuarter));
                OnQuarterSelectionChanged();
            }
        }
        private void OnDateSelectionChanged()
        {
            LoadHomeworkForSelectedClassAndSubject();
        }
        private void OnQuarterSelectionChanged()
        {
            SelectedDate = _weekNavigator.GetFirstDateOfQuarter(SelectedQuarter);
            LoadWeekDates();
            LoadHomeworkForSelectedClassAndSubject();
        }
        private readonly WeekNavigator _weekNavigator;
        public ICommand LoadedCommand { get; set; }
        public ICommand ClassSelectionChangedCommand { get; set; }
        public ICommand SubjectSelectionChangedCommand { get; set; }
        public ICommand StudentSelectionChangedCommand { get; set; }
        public ICommand NextWeekCommand { get; set; }
        public ICommand PreviousWeekCommand { get; set; }
        public ICommand AddHomeworkCommand { get; set; }
        public ICommand DeleteHomeworkCommand { get; set; }
        private ClassViewModel _selectedClass;
        public ClassViewModel SelectedClass
        {
            get { return _selectedClass; }
            set
            {
                if (_selectedClass != value)
                {
                    _selectedClass = value;
                    OnPropertyChanged(nameof(SelectedClass));
                    ClassSelectionChangedCommand.Execute(null);
                }
            }
        }

        private Subjects _selectedSubject;
        public Subjects SelectedSubject
        {
            get { return _selectedSubject; }
            set
            {
                if (_selectedSubject != value)
                {
                    _selectedSubject = value;
                    OnPropertyChanged(nameof(SelectedSubject));
                    SubjectSelectionChangedCommand.Execute(null);
                }
            }
        }
        private Homeworks _selectedHomework;
        public Homeworks SelectedHomework
        {
            get { return _selectedHomework; }
            set
            {
                if (_selectedHomework != value)
                {
                    _selectedHomework = value;
                    OnPropertyChanged(nameof(SelectedHomework));
                }
            }
        }

        public HomeworkPageViewModel(WeekNavigator weekNavigator)
        {
            _weekNavigator = weekNavigator ?? throw new ArgumentNullException(nameof(weekNavigator));
            ClassViewModelList = new ObservableCollection<ClassViewModel>();
            SubjectsList = new ObservableCollection<Subjects>();
            Quarters = new ObservableCollection<Quarter>();   
            LoadedCommand = new RelayCommand(Page_Loaded);
            WeekDates = new ObservableCollection<DateTime>();
            HomeworksList = new ObservableCollection<Homeworks>();
            ClassSelectionChangedCommand = new RelayCommand(OnClassSelectionChanged);
            SubjectSelectionChangedCommand = new RelayCommand(OnSubjectSelectionChanged);
            StudentSelectionChangedCommand = new RelayCommand(OnStudentSelectionChanged);
            NextWeekCommand = new RelayCommand(NextWeek);
            PreviousWeekCommand = new RelayCommand(PreviousWeek);
            AddHomeworkCommand = new RelayCommand(OnHomeworkAddButtonClick);
            DeleteHomeworkCommand = new RelayCommand(OnHomeworkDeleteButtonClick);
            SelectedDate = _weekNavigator.GetFirstDateOfSeptember();
            using (School_Entities _context = new School_Entities())
            {
                var defaultClass = _context.Classes.First();
                SelectedClass = new ClassViewModel
                {
                    ID = defaultClass.ID,
                    ClassNumber = defaultClass.ClassNumber,
                    ClassLetter = defaultClass.ClassLetter
                };
                SelectedQuarter = Quarter.First;
                SelectedSubject = _context.Subjects.First();
                InputDescription = "";
            }
        }

        private void OnHomeworkAddButtonClick()
        {
            if (SelectedClass != null && SelectedSubject != null && SelectedDate != null && !string.IsNullOrWhiteSpace(InputDescription.ToString()))
            {
                using (School_Entities _context = new School_Entities())
                {
                    var currentTeacher = _context.Teachers.FirstOrDefault(t => t.AccountID == CurrentUser.ID);
                    var teacherSubjects = _context.Teachers_Subjects.Where(ts => ts.TeacherID == currentTeacher.ID);
                    if (teacherSubjects.Any(ts => ts.SubjectID == SelectedSubject.ID))
                    {
                        var newHomework = new Homeworks
                        {
                            Description = InputDescription,
                            SubjectID = SelectedSubject.ID,
                            ClassID = SelectedClass.ID,
                            TeacherID = (int?)currentTeacher.ID,
                            Date = SelectedDate
                        };
                        _context.Homeworks.Add(newHomework);
                        _context.News.Add(new News
                        {
                            ClassID = SelectedClass.ID,
                            TeacherID = currentTeacher.ID,
                            Description = $"Выложено домашнее задание по предмету {SelectedSubject.SubjectTitle}"
                        });
                        try
                        {
                            _context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        HomeworksList.Add(newHomework);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Вы не можете выдать домашнее задание по предмету, который не преподаете");
                    }
                    
                    
                }
            }
        } 
        private void OnHomeworkDeleteButtonClick()
        {
            using (School_Entities _context = new School_Entities())
            {
                var currentTeacher = _context.Teachers.FirstOrDefault(teacher => teacher.AccountID == CurrentUser.ID);
                if (SelectedHomework.TeacherID == currentTeacher.ID)
                {
                    _context.Homeworks.Remove(_context.Homeworks.Find(SelectedHomework.ID));
                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    HomeworksList.Remove(SelectedHomework);
                }
                else
                {
                    System.Windows.MessageBox.Show("Вы не можете удалить оценку, выставленную другим учителем");
                }
            }
        }
        private void NextWeek()
        {
            if (_weekNavigator.GetLastDateOfWeek(SelectedDate) <= _weekNavigator.GetLastDateOfQuarter(SelectedQuarter))
            {
                WeekDates.Clear();
                SelectedDate = _weekNavigator.GetFirstDateOfNextWeek(SelectedDate);
                LoadWeekDates();
            }
        }
        private void PreviousWeek()
        {
            if (_weekNavigator.GetFirstDateOfWeek(SelectedDate) >= _weekNavigator.GetFirstDateOfQuarter(SelectedQuarter))
            {
                WeekDates.Clear();
                SelectedDate = _weekNavigator.GetFirstDateOfPreviousWeek(SelectedDate);
                LoadWeekDates();
            }
        }
        private void Page_Loaded()
        {
            using (School_Entities _context = new School_Entities())
            {
                Clear();
                var classes = _context.Classes.ToList();
                foreach (var studentClass in classes)
                {
                    ClassViewModelList.Add(new ClassViewModel
                    {
                        ID = studentClass.ID,
                        ClassNumber = studentClass.ClassNumber,
                        ClassLetter = studentClass.ClassLetter
                    });
                }

                var subjects = _context.Subjects.ToList();
                foreach (var subject in subjects)
                {
                    SubjectsList.Add(subject);
                }

                Quarters.Add(Quarter.First);
                Quarters.Add(Quarter.Second);
                Quarters.Add(Quarter.Third);
                Quarters.Add(Quarter.Fourth);

                LoadWeekDates();
                SelectedClass = ClassViewModelList.First();
                SelectedQuarter = Quarters.First();
                SelectedSubject = SubjectsList.First();
            }
        }

        private void LoadWeekDates()
        {
            var (startOfWeek, endOfWeek) = _weekNavigator.GetWeekDatesRange(SelectedDate);
            for (var date = startOfWeek; date <= endOfWeek; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Sunday && date >= _weekNavigator.GetFirstDateOfSchoolYear() && date <= _weekNavigator.GetLastDateOfSchoolYear())
                {
                    WeekDates.Add(date);
                }
            }
            if (WeekDates.Count == 0)
            {
                NextWeek();
            }
        }

        private void OnClassSelectionChanged()
        {
            LoadHomeworkForSelectedClassAndSubject();
        }
        private void OnSubjectSelectionChanged()
        {
            LoadHomeworkForSelectedClassAndSubject();
        }

        private void OnStudentSelectionChanged()
        {
            LoadHomeworkForSelectedClassAndSubject();
        }

        private void LoadHomeworkForSelectedClassAndSubject()
        {
            if (SelectedClass != null && SelectedSubject != null)
            {
                using (School_Entities _context = new School_Entities())
                {
                    HomeworksList.Clear();
                    if (CurrentUser.Instance().Role == "Учитель")
                    {
                        var homeworksInDate = _context.Homeworks
                        .Where(grade => DbFunctions.TruncateTime(grade.Date) == SelectedDate.Date
                                       && grade.SubjectID == SelectedSubject.ID
                                       && grade.ClassID == SelectedClass.ID
                                       && grade.Teachers.AccountID == CurrentUser.ID)
                        .ToList();
                        foreach (var homework in homeworksInDate)
                        {
                            HomeworksList.Add(homework);
                        }
                    }
                    else
                    {
                        var homeworksInDate = _context.Homeworks
                        .Where(grade => DbFunctions.TruncateTime(grade.Date) == SelectedDate.Date
                                       && grade.SubjectID == SelectedSubject.ID
                                       && grade.ClassID == SelectedClass.ID)
                        .ToList();
                        foreach (var homework in homeworksInDate)
                        {
                            HomeworksList.Add(homework);
                        }
                    }
                }
            }
        }
        private void Clear()
        {
            ClassViewModelList.Clear();
            SubjectsList.Clear();
            Quarters.Clear();
        }
    }
}
