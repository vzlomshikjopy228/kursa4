using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using приложение.Model;
using приложение.Services;
using приложение.ViewModels.Base;

namespace приложение.ViewModels.PagesViewModels
{
    public class RegistrationPageViewModel : BaseViewModel
    {
        private string email;
        private string password;
        private string login;
        private string name;
        private string secondName;
        private string surName;
        private Roles selectedRole;
        public Roles SelectedRole
        {
            get => selectedRole;
            set
            {
                if (selectedRole != value)
                {
                    selectedRole = value;
                    OnPropertyChanged(nameof(SelectedRole));
                }
            }
        }
        public ObservableCollection<ClassViewModel> ClassesViewModelList { get; set; }
        public ClassViewModel SelectedClass { get; set; }
        public ObservableCollection<Roles> RolesList { get; set; }
        public ObservableCollection<Subjects> SubjectsCollection { get; set; }
        public ObservableCollection<Subjects> SelectedSubjectsCollection { get; set; }
        private readonly IHashingService hashingService;
        public string Email { get { return email; } set { email = value; } }
        public string Password { get { return password; } set { password = value; } }
        public string Login { get { return login; } set { login = value; } }
        public string Name { get { return name; } set { name = value; } }
        public string SecondName { get { return secondName; } set { secondName = value; } }
        public string SurName { get { return surName; } set { surName = value; } }
        public ICommand RegisterCommand { get; set; }
        public ICommand PageLoadedCommand { get; set; }
        public ICommand SelectedCommand { get; set; }
        public ICommand DeselectedCommand { get; set; }
        public RegistrationPageViewModel(IHashingService _hashingService)
        {
            hashingService = _hashingService ?? throw new ArgumentNullException(nameof(_hashingService));
            RolesList = new ObservableCollection<Roles>();
            ClassesViewModelList = new ObservableCollection<ClassViewModel>();
            SubjectsCollection = new ObservableCollection<Subjects>();
            SelectedSubjectsCollection = new ObservableCollection<Subjects>();
            RegisterCommand = new RelayCommand(Register);
            PageLoadedCommand = new RelayCommand(Page_Loaded);
            SelectedCommand = new RelayCommand<Subjects>(Select);
            DeselectedCommand = new RelayCommand<Subjects>(Deselect);
        }
        private void Register()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Login) ||
                string.IsNullOrWhiteSpace(Password) || SelectedRole == null)
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!IsValidEmail(Email))
            {
                MessageBox.Show("Неверный формат электронной почты. Пожалуйста, введите корректный email.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidLogin(Login))
            {
                MessageBox.Show("Логин должен содержать только латинские буквы и цифры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidPassword(Password))
            {
                MessageBox.Show("Пароль должен содержать только латинские буквы и цифры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidName(Name))
            {
                MessageBox.Show("Имя должно быть на кириллице и начинаться с заглавной буквы. Цифры в имени не допускаются.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidSurname(SurName))
            {
                MessageBox.Show("Фамилия должна быть на кириллице и начинаться с заглавной буквы. Цифры в фамилии не допускаются.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidSecondName(SecondName))
            {
                MessageBox.Show("Отчество должно быть на кириллице и начинаться с заглавной буквы. Цифры в отчестве не допускаются.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            byte[] hashedPassword = hashingService.HashPassword(Password);
            using (var context = new School_Entities())
            {
                var existingAccount = context.Accounts.FirstOrDefault(a => a.Email == Email || a.Login == Login);
                Students newStudent;
                Teachers newTeacher;
                if (existingAccount != null)
                {
                    MessageBox.Show("Аккаунт с таким email или логином уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newAccount = new Accounts
                {
                    Email = Email,
                    Login = Login,
                    Password = hashedPassword,
                    RoleID = SelectedRole.ID
                };

                context.Accounts.Add(newAccount);
                context.SaveChanges();

                if (SelectedRole.RoleTitle == "Ученик")
                {
                    newStudent = new Students
                    {
                        FirstName = Name,
                        SecondName = SecondName,
                        Surname = SurName,
                        AccountID = newAccount.ID,
                        ClassID = SelectedClass.ID
                    };
                    context.Students.Add(newStudent);
                }
                else if (SelectedRole.RoleTitle == "Учитель")
                {
                    if(SelectedSubjectsCollection.Count > 0)
                    {
                        newTeacher = new Teachers
                        {
                            FirstName = Name,
                            SecondName = SecondName,
                            Surname = SurName,
                            AccountID = newAccount.ID
                        };
                        context.Teachers.Add(newTeacher);
                        foreach (var subject in SelectedSubjectsCollection)
                        {
                            var newTeacher_Subjects = new Teachers_Subjects
                            {
                                TeacherID = newTeacher.ID,
                                SubjectID = subject.ID
                            };
                            context.Teachers_Subjects.Add(newTeacher_Subjects);
                        }
                    }
                }
                else if (SelectedRole.RoleTitle == "Руководство")
                {
                    var newLeadership = new Leadership
                    {
                        FirstName = Name,
                        SecondName = SecondName,
                        Surname = SurName,
                        AccountID = newAccount.ID
                    };
                    context.Leadership.Add(newLeadership);
                }
                context.SaveChanges();
                MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(email) && !Regex.IsMatch(email, @"[а-яА-Я]");
        }

        private void Clear()
        {
            RolesList.Clear();
            ClassesViewModelList.Clear();
            SubjectsCollection.Clear();
            SelectedSubjectsCollection.Clear();
        }

        private bool IsValidLogin(string login)
        {
            var loginRegex = new Regex(@"^[a-zA-Z0-9]+$");
            return loginRegex.IsMatch(login) && !Regex.IsMatch(login, @"[а-яА-Я]");
        }

        private bool IsValidPassword(string password)
        {
            var passwordRegex = new Regex(@"^[a-zA-Z0-9]+$");
            return passwordRegex.IsMatch(password) && !Regex.IsMatch(password, @"[а-яА-Я]");
        }

        private bool IsValidName(string name)
        {
            var nameRegex = new Regex(@"^[а-яА-ЯЁё]+$");
            return nameRegex.IsMatch(name) && !name.Any(char.IsDigit) && char.IsUpper(name[0]);
        }
        private bool IsValidSecondName(string secondName)
        {
            var patronymicRegex = new Regex(@"^[а-яА-ЯЁё]+$");
            return patronymicRegex.IsMatch(secondName) && !secondName.Any(char.IsDigit) && char.IsUpper(secondName[0]);
        }
        private bool IsValidSurname(string surname)
        {
            var surnameRegex = new Regex(@"^[а-яА-ЯЁё]+$");
            return surnameRegex.IsMatch(surname) && !surname.Any(char.IsDigit) && char.IsUpper(surname[0]);
        }
        private void LoadRoles()
        {
            using (School_Entities _context = new School_Entities())
            {
                foreach (var role in _context.Roles)
                {
                    RolesList.Add(role);
                }
            }
        }
        private void Select(Subjects selectedSubject)
        {
            if(selectedSubject != null && SelectedSubjectsCollection.All(element => !element.Equals(selectedSubject)) == true)
            {
                SelectedSubjectsCollection.Add(selectedSubject);
            }
        }
        private void Deselect(Subjects deselectedSubject) 
        {
            if (deselectedSubject != null && SelectedSubjectsCollection.FirstOrDefault(element => element.Equals(deselectedSubject)) != null)
            {
                SelectedSubjectsCollection.Remove(deselectedSubject);
            }
        }
        private void LoadClasses()
        {
            using (School_Entities _context = new School_Entities())
            {
                foreach (var studentClass in _context.Classes)
                {
                    ClassesViewModelList.Add(new ClassViewModel()
                    {
                        ID = studentClass.ID,
                        ClassNumber = studentClass.ClassNumber,
                        ClassLetter = studentClass.ClassLetter
                    });
                }
            }
        }
        private void LoadSubjects()
        {
            using (School_Entities _context = new School_Entities())
            {
                foreach (var subject in _context.Subjects)
                {
                    SubjectsCollection.Add(subject);
                }
            }
        }
        private void Page_Loaded()
        {
            Clear();
            LoadSubjects();
            LoadRoles();
            LoadClasses();
        }
    }
}
