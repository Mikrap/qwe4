using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Demo.Classes
{
    public class DateSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime checkOut && parameter is string param)
            {
                if (param == "CheckIn" && checkOut != default(DateTime))
                {
                    var checkIn = (DateTime)System.Convert.ChangeType(parameter, typeof(DateTime));
                    return (checkOut - checkIn).Days;
                }
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
