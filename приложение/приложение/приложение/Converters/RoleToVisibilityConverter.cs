using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using приложение.ViewModels;

namespace приложение.Converters
{
    public class RoleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedRole = CurrentUser.Instance().Role;
            var targetElement = parameter as string;

            if (selectedRole == null || targetElement == null)
                return Visibility.Collapsed;

            switch (targetElement)
            {
                case "NewsPageButton":
                case "ClassComboBox":
                    return selectedRole == "Ученик" ? Visibility.Visible : Visibility.Collapsed;

                case "InputTextBox":
                case "InputTextBlock":
                case "AddGradeButton":
                case "NextWeekButton":
                case "PreviousWeekButton":
                case "DeleteGradeButton":
                    return selectedRole == "Учитель" ? Visibility.Visible : Visibility.Collapsed;

                case "AddToMondayButton":
                case "AddToTuesdayButton":
                case "AddToWednesdayButton":
                case "AddToThursdayButton":
                case "AddToFridayButton":
                case "AddToSaturdayButton":
                case "DeleteToMondayButton":
                case "DeleteToTuesdayButton":
                case "DeleteToWednesdayButton":
                case "DeleteToThursdayButton":
                case "DeleteToFridayButton":
                case "DeleteToSaturdayButton":
                case "RegisterButton":
                    return selectedRole == "Руководство" ? Visibility.Visible : Visibility.Collapsed;
                default:
                    return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
