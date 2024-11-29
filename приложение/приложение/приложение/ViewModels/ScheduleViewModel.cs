using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Input;
using приложение.Model;

namespace приложение.ViewModels
{
    public class ScheduleViewModel
    {
        public ScheduleViewModel()
        {
            DeleteButtonCommand = new RelayCommand<object>(OnDeleteButtonClick);
        }
        public int ID { get; set; }
        public string SubjectTitle { get; set; }
        public string DisplayTeacher { get; set; }
        public TimeSpan Time { get; set; }
        public ICommand DeleteButtonCommand { get; }
        private void OnDeleteButtonClick(object sender)
        {
            ScheduleViewModel scheduleForDelete = sender as ScheduleViewModel;
            if (scheduleForDelete != null)
            {
                using (School_Entities _context = new School_Entities())
                {
                    _context.Schedules.Remove(_context.Schedules.Find(scheduleForDelete.ID));
                    _context.SaveChanges();
                }
            }
        }
    }
}
