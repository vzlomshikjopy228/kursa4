using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.InteropServices.ComTypes;
using System.Data.Entity.Core.Mapping;
using System.Globalization;

namespace приложение.Services
{
    public class WeekNavigator
    {
        private DateTime startDate;
        public WeekNavigator()
        {
            startDate = GetFirstDateOfSeptember();
        }
        public DateTime GetFirstDateOfSeptember()
        {
            DateTime firstDateOfSeptember;
            if(DateTime.Compare(DateTime.Now, new DateTime(DateTime.Now.Year, 9, 1)) > 0)
            {
                firstDateOfSeptember = new DateTime(DateTime.Now.Year, 9, 1);
            }
            else
            {
                firstDateOfSeptember = new DateTime(DateTime.Now.Year - 1, 9, 1);
            }
            return firstDateOfSeptember;
        }
        public ObservableCollection<DateTime> GetDatesOfWeek(DateTime selectedDate)
        {
            ObservableCollection<DateTime> datesOfWeek = new ObservableCollection<DateTime>();
            DateTime startDate = GetFirstDateOfWeek(selectedDate);
            for (int i = 1; i <= 5; i++)
            {
                datesOfWeek.Add(startDate);
                startDate = startDate.AddDays(i);
            }
            return datesOfWeek;
        }
        public DateTime GetFirstDateOfWeek(DateTime selectedDate)
        {
            DateTime startDate = new DateTime(selectedDate.Ticks);
            switch (selectedDate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return startDate.AddDays(0);
                case DayOfWeek.Tuesday:
                    return startDate.AddDays(-1);
                case DayOfWeek.Wednesday:
                    return startDate.AddDays(-2);
                case DayOfWeek.Thursday:
                    return startDate.AddDays(-3);
                case DayOfWeek.Friday:
                    return startDate.AddDays(-4);
                case DayOfWeek.Saturday:
                    return startDate.AddDays(-5);
                case DayOfWeek.Sunday:
                    return startDate.AddDays(-6);
                default:
                    throw new AccessViolationException();
            }
        }
        public DateTime GetLastDateOfWeek(DateTime selectedDate)
        {
            DateTime startDate = new DateTime(selectedDate.Ticks);
            switch (selectedDate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return startDate.AddDays(6);
                case DayOfWeek.Tuesday:
                    return startDate.AddDays(5);
                case DayOfWeek.Wednesday:
                    return startDate.AddDays(4);
                case DayOfWeek.Thursday:
                    return startDate.AddDays(3);
                case DayOfWeek.Friday:
                    return startDate.AddDays(2);
                case DayOfWeek.Saturday:
                    return startDate.AddDays(1);
                case DayOfWeek.Sunday:
                    return startDate.AddDays(0);
                default:
                    throw new AccessViolationException();
            }
        }
        public DateTime GetFirstDateOfNextWeek(DateTime selectedDate)
        {
            switch (selectedDate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return selectedDate.AddDays(7);
                case DayOfWeek.Tuesday:
                    return selectedDate.AddDays(6);
                case DayOfWeek.Wednesday:
                    return selectedDate.AddDays(5);
                case DayOfWeek.Thursday:
                    return selectedDate.AddDays(4);
                case DayOfWeek.Friday:
                    return selectedDate.AddDays(3);
                case DayOfWeek.Saturday:
                    return selectedDate.AddDays(2);
                case DayOfWeek.Sunday:
                    return selectedDate.AddDays(1);
                default:
                    throw new ArgumentException();
            }
        }
        public DateTime GetFirstDateOfPreviousWeek(DateTime selectedDate)
        {
            switch (selectedDate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return selectedDate.AddDays(-7);
                case DayOfWeek.Tuesday:
                    return selectedDate.AddDays(-8);
                case DayOfWeek.Wednesday:
                    return selectedDate.AddDays(-9);
                case DayOfWeek.Thursday:
                    return selectedDate.AddDays(-10);
                case DayOfWeek.Friday:
                    return selectedDate.AddDays(-11);
                case DayOfWeek.Saturday:
                    return selectedDate.AddDays(-12);
                case DayOfWeek.Sunday:
                    return selectedDate.AddDays(-13);
                default:
                    throw new ArgumentException();
            }
        }
        public DateTime GetFirstDateOfQuarter(Quarter selectedQuarter = Quarter.First)
        {
            DateTime result = GetFirstDateOfSeptember();

            switch (selectedQuarter)
            {
                case Quarter.First:
                    return result.AddDays(0);
                case Quarter.Second:
                    return result.AddDays(67); 
                case Quarter.Third:
                    return result.AddDays(67 * 2);
                case Quarter.Fourth:
                    return result.AddDays(67 * 3); 
                default:
                    throw new ArgumentException("Неверная четверть");
            }
        }
        public DateTime GetLastDateOfQuarter(Quarter selectedQuarter = Quarter.First)
        {
            DateTime result = GetFirstDateOfSeptember();
            switch (selectedQuarter)
            {
                case Quarter.First:
                    return result.AddDays(67 * 1);
                case Quarter.Second:
                    return result.AddDays(67 * 2);
                case Quarter.Third:
                    return result.AddDays(67 * 3);
                case Quarter.Fourth:
                    return result.AddDays(67 * 4);
                default:
                    throw new ArgumentException("Неверная четверть");
            }
        }
        public DateTime GetLastDateOfSchoolYear()
        {
            DateTime result = new DateTime(startDate.Ticks);
            return result.AddDays(67 * 4);
        }
        public DateTime GetFirstDateOfSchoolYear()
        {
            return startDate;
        }
        public bool IsDateRangeIncludedInQuarter(DateTime selectedDate, Quarter selectedQuarter = Quarter.First)
        {
            (DateTime, DateTime) weekDatesRange = GetWeekDatesRange(selectedDate);
            return GetFirstDateOfQuarter(selectedQuarter) <= weekDatesRange.Item1 && 
                weekDatesRange.Item2 <= GetLastDateOfQuarter(selectedQuarter); 
        }
        public (DateTime startDate, DateTime lastDate) GetWeekDatesRange(DateTime selectedDate) 
        {
            DateTime startDate = GetFirstDateOfWeek(selectedDate);
            DateTime lastDate = GetLastDateOfWeek(selectedDate);
            return (startDate, lastDate);
        }
    }
}
