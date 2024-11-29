using System;
using System.Collections.Generic;
using System.Globalization;
using приложение.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace приложение.Converters
{
    public class SelectedRoleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Roles selectedRole)
            {
                if (parameter != null && parameter.ToString() == "ClassComboBox")
                {
                    return selectedRole.RoleTitle == "Ученик" ? Visibility.Visible : Visibility.Collapsed;
                }
                else if (parameter != null && parameter.ToString() == "SubjectsStackPanel")
                {
                    return selectedRole.RoleTitle == "Учитель" ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
