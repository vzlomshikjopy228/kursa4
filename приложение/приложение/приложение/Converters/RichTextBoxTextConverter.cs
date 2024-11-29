using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;

namespace приложение.Converters
{
    public class RichTextBoxTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                return new TextRange(new FlowDocument().ContentStart, new FlowDocument().ContentEnd) { Text = text };
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TextRange textRange)
            {
                return textRange.Text;
            }
            return null;
        }
    }
}
