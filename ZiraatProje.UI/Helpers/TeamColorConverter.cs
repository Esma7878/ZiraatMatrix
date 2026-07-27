using System;
using System.Globalization;
using System.Windows.Data;
using ZiraatProje.Business;

namespace ZiraatProje.UI.Helpers
{
    public class TeamColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string teamName = value?.ToString() ?? string.Empty;
            string param = parameter?.ToString()?.ToLower() ?? "bg";

            return param switch
            {
                "bg" => TeamColorHelper.GetBgColor(null, teamName),
                "border" => TeamColorHelper.GetBorderColor(null, teamName),
                "text" or "foreground" => TeamColorHelper.GetTextColor(null, teamName),
                "header" or "primary" => TeamColorHelper.GetHeaderColor(null, teamName),
                _ => TeamColorHelper.GetBgColor(null, teamName)
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
