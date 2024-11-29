using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace приложение.Helpers
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached(
                "Password",
                typeof(string),
                typeof(PasswordBoxHelper),
                new PropertyMetadata(string.Empty, OnPasswordChanged));
        public static void SetPassword(UIElement element, string value)
        {
            element.SetValue(PasswordProperty, value);
        }
        public static string GetPassword(UIElement element)
        {
            return (string)element.GetValue(PasswordProperty);
        }
        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;
            if (passwordBox != null)
            {
                passwordBox.PasswordChanged += (sender, args) =>
                {
                    SetPassword(passwordBox, passwordBox.Password);
                };
            }
        }
    }
}
