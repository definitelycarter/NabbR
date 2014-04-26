using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NabbR.Helpers
{
    static class DialogHelper
    {
        public static Boolean? GetDialogResult(DependencyObject obj)
        {
            return (Boolean?)obj.GetValue(DialogResultProperty);
        }

        public static void SetDialogResult(DependencyObject obj, Boolean? value)
        {
            obj.SetValue(DialogResultProperty, value);
        }

        // Using a DependencyProperty as the backing store for DialogResult.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached("DialogResult", typeof(Boolean?), typeof(DialogHelper), new PropertyMetadata(null, OnDialogResultChanged));

        private static void OnDialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window window = d as Window;
            window.Close();
        }
    }
}
