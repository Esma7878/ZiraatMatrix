using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ZiraatProje.UI.Views
{
    public class LeaveEditVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3) return Visibility.Collapsed;

            var leaveUserFullName = values[0] as string;
            var currentUserName = values[1] as string;
            var isAdmin = values[2] is bool b && b;

            if (isAdmin) return Visibility.Visible;

            if (!string.IsNullOrWhiteSpace(leaveUserFullName) &&
                !string.IsNullOrWhiteSpace(currentUserName) &&
                leaveUserFullName.Equals(currentUserName, StringComparison.OrdinalIgnoreCase))
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public partial class LeavesView : UserControl
    {
        public LeavesView()
        {
            InitializeComponent();
        }
    }
}
