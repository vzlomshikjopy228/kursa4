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
    public class GradesPageViewModel : BaseViewModel
    {
        public ObservableCollection<ClassViewModel> ClassViewModelList { get; set; }
        private int _inputGrade;
        public int InputGrade
        {
            get
            {
                return _inputGrade;
            }
            set
            {
                _inputGrade = value;
                OnPropertyChanged(nameof(InputGrade));
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
        public ObservableCollection<StudentViewModel> StudentsList { get; set; }
        public ObservableCollection<DateTime> WeekDates { get; set; }
        public ObservableCollection<Grades> GradesList { get; set; }
        public ObservableCollection<Quarter> Quarters { get; set; }
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
            LoadGradesForSelectedStudentAndSubject();
        }
        private void OnQuarterSelectionChanged()
        {
            SelectedDate = _weekNavigator.GetFirstDateOfQuarter(SelectedQuarter);         
            LoadWeekDates();
            LoadGradesForSelectedStudentAndSubject();
        }
        private readonly WeekNavigator _weekNavigator;
        public ICommand LoadedCommand { get; set; }
        public ICommand ClassSelectionChangedCommand { get; set; }
        public ICommand SubjectSelectionChangedCommand { get; set; }
        public ICommand StudentSelectionChangedCommand { get; set; }
        public ICommand NextWeekCommand { get; set; }
        public ICommand PreviousWeekCommand { get; set; }
        public ICommand AddGradeCommand { get; set; }
        public ICommand DeleteGradeCommand { get; set; }
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
        private Grades _selectedGrade;
        public Grades SelectedGrade
        {
            get { return _selectedGrade; }
            set
            {
                if (_selectedGrade != value)
                {
                    _selectedGrade = value;
                    OnPropertyChanged(nameof(SelectedGrade));
                }
            }
        }

        private StudentViewModel _selectedStudent;
        public StudentViewModel SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                if (_selectedStudent != value)
                {
                    _selectedStudent = value;
                    OnPropertyChanged(nameof(SelectedStudent));
                    StudentSelectionChangedCommand.Execute(null);
                }
            }
        }

        public GradesPageViewModel(WeekNavigator weekNavigator)
        {
            _weekNavigator = weekNavigator ?? throw new ArgumentNullException(nameof(weekNavigator));
            ClassViewModelList = new ObservableCollection<ClassViewModel>();
            SubjectsList = new ObservableCollection<Subjects>();
            StudentsList = new ObservableCollection<StudentViewModel>();
            Quarters = new ObservableCollection<Quarter>();
            GradesList = new ObservableCollection<Grades>();
            LoadedCommand = new RelayCommand(Page_Loaded);      
            WeekDates = new ObservableCollection<DateTime>();
            ClassSelectionChangedCommand = new RelayCommand(OnClassSelectionChanged);
            SubjectSelectionChangedCommand = new RelayCommand(OnSubjectSelectionChanged);
            StudentSelectionChangedCommand = new RelayCommand(OnStudentSelectionChanged);
            NextWeekCommand = new RelayCommand(NextWeek);
            PreviousWeekCommand = new RelayCommand(PreviousWeek);
            AddGradeCommand = new RelayCommand(OnGradeAddButtonClick);
            DeleteGradeCommand = new RelayCommand(OnGradeDeleteButtonClick);
            SelectedDate = _weekNavigator.GetFirstDateOfSeptember();
            using (School_Entities _context = new School_Entities())
            {
                if(_context.Students.Count() > 0)
                {
                    var defaultStudent = _context.Students.First();
                    SelectedStudent = new StudentViewModel
                    {
                        ID = defaultStudent.ID,
                        SecondName = defaultStudent.SecondName,
                        FirstName = defaultStudent.FirstName,
                        SurName = defaultStudent.Surname
                    };
                    var defaultClass = _context.Classes.First();
                    SelectedClass = new ClassViewModel
                    {
                        ID = defaultClass.ID,
                        ClassNumber = defaultClass.ClassNumber,
                        ClassLetter = defaultClass.ClassLetter
                    };
                    SelectedQuarter = Quarter.First;
                    SelectedSubject = _context.Subjects.First();
                }
            }
        }

        private void OnGradeAddButtonClick()
        {
            if (SelectedStudent != null && SelectedSubject != null && SelectedDate != null && !string.IsNullOrWhiteSpace(InputGrade.ToString()))
            {
                using(School_Entities _context = new School_Entities())
                {
                    var currentTeacher = _context.Teachers.FirstOrDefault(t => t.AccountID == CurrentUser.ID);
                    var teacherSubjects = _context.Teachers_Subjects.Where(ts => ts.TeacherID == currentTeacher.ID);
                    if (teacherSubjects.Any(ts => ts.SubjectID == SelectedSubject.ID))
                    {
                        var newGrade = new Grades
                        {
                            GradeValue = (byte)InputGrade,
                            SubjectID = SelectedSubject.ID,
                            StudentID = SelectedStudent.ID,
                            TeacherID = (int?)currentTeacher.ID,
                            Date = SelectedDate
                        };
                        _context.Grades.Add(newGrade);
                        try
                        {
                            _context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        GradesList.Add(newGrade);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Вы не можете поставить оценку по предмету, который не преподаете");
                    }
                }
            }
        }
        private void OnGradeDeleteButtonClick()
        {
            using (School_Entities _context = new School_Entities())
            {
                var currentTeacher = _context.Teachers.FirstOrDefault(teacher => teacher.AccountID == CurrentUser.ID);
                if(SelectedGrade.TeacherID == currentTeacher.ID)
                {
                    _context.Grades.Remove(_context.Grades.Find(SelectedGrade.ID));
                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    GradesList.Remove(SelectedGrade);
                }
                else
                {
                    System.Windows.MessageBox.Show("Вы не можете удалить оценку, выставленную другим учителем");
                }
            }
        }
        private void NextWeek()
        {
            if(_weekNavigator.GetLastDateOfWeek(SelectedDate) <= _weekNavigator.GetLastDateOfQuarter(SelectedQuarter))
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

                var students = _context.Students.ToList();
                foreach (var student in students)
                {
                    StudentsList.Add(new StudentViewModel
                    {
                        ID = student.ID,
                        FirstName = student.FirstName,
                        SecondName = student.SecondName,
                        SurName = student.Surname
                    });
                }
                Quarters.Add(Quarter.First);
                Quarters.Add(Quarter.Second);
                Quarters.Add(Quarter.Third);
                Quarters.Add(Quarter.Fourth);

                LoadWeekDates();
                SelectedStudent = StudentsList.First();
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
                if(date.DayOfWeek != DayOfWeek.Sunday && date >= _weekNavigator.GetFirstDateOfSchoolYear() && date <= _weekNavigator.GetLastDateOfSchoolYear())
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
            GradesList.Clear();
            if (SelectedClass != null)
            {
                using (var context = new School_Entities())
                {
                    var studentsInClass = context.Students.Where(s => s.ClassID == SelectedClass.ID).ToList();
                    StudentsList.Clear();
                    foreach (var student in studentsInClass)
                    {
                        StudentsList.Add(new StudentViewModel
                        {
                            ID = student.ID,
                            FirstName = student.FirstName,
                            SecondName = student.SecondName,
                            SurName = student.Surname
                        });
                    }
                }
            }
        }
        private void OnSubjectSelectionChanged()
        {
            LoadGradesForSelectedStudentAndSubject();
        }

        private void OnStudentSelectionChanged()
        {
            LoadGradesForSelectedStudentAndSubject();
        }

        private void LoadGradesForSelectedStudentAndSubject()
        {
            if (SelectedStudent != null && SelectedSubject != null)
            {
                using (School_Entities _context = new School_Entities())
                {
                    GradesList.Clear();
                    if(CurrentUser.Instance().Role == "Учитель")
                    {
                        var gradesInDate = _context.Grades
                        .Where(grade => DbFunctions.TruncateTime(grade.Date) == SelectedDate.Date
                                       && grade.SubjectID == SelectedSubject.ID
                                       && grade.StudentID == SelectedStudent.ID
                                       && grade.Teachers.AccountID == CurrentUser.ID)
                        .ToList();
                        foreach (var grade in gradesInDate)
                        {
                            GradesList.Add(grade);
                        }
                    }
                    else
                    {
                        var gradesInDate = _context.Grades
                        .Where(grade => DbFunctions.TruncateTime(grade.Date) == SelectedDate.Date
                                       && grade.SubjectID == SelectedSubject.ID
                                       && grade.StudentID == SelectedStudent.ID)
                        .ToList();
                        foreach (var grade in gradesInDate)
                        {
                            GradesList.Add(grade);
                        }
                    }
                }
            }
        }
        private void Clear()
        {
            StudentsList.Clear();
            ClassViewModelList.Clear();
            SubjectsList.Clear();
            Quarters.Clear();
        }
    }
}
