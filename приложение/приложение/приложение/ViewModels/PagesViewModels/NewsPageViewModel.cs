using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using приложение.Model;
using приложение.ViewModels.Base;

namespace приложение.ViewModels.PagesViewModels
{
    public class NewsPageViewModel : BaseViewModel
    {
        private ObservableCollection<News> _newsList;
        public ICommand MarkAsReadCommand { get; set; }

        public ObservableCollection<News> NewsList
        {
            get { return _newsList; }
            set
            {
                _newsList = value;
                OnPropertyChanged(nameof(NewsList));
            }
        }
        public NewsPageViewModel()
        {
            NewsList = new ObservableCollection<News>();
            MarkAsReadCommand = new RelayCommand(OnMarkAsReadButtonClick);
            LoadNews();
        }
        private void OnMarkAsReadButtonClick()
        {
            using(var _context = new School_Entities())
            {
                var currentStudent = _context.Students.FirstOrDefault(s => s.AccountID == CurrentUser.ID);
                var newsToDelete = _context.News.FirstOrDefault(n => n.ClassID == currentStudent.ClassID);
                _context.News.Remove(newsToDelete);
                _context.SaveChanges();
                NewsList.Remove(newsToDelete);
            }
        }
        private void LoadNews()
        {
            using (var _context = new School_Entities())
            {
                var currentStudent = _context.Students.FirstOrDefault(s => s.AccountID == CurrentUser.ID);
                foreach(var n in _context.News.ToList())
                {
                    if (n.ClassID == currentStudent.ID)
                    {
                        NewsList.Add(n);
                    }
                }
            }
        }
    }
}
