using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Common.Extensions.WPF.Converters
{
    /// <summary>
    /// 算术运算转换器（加减运算）
    /// </summary>
    public class CalculateConverter : IValueConverter, IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value?.ToString(), out double d) && double.TryParse(parameter?.ToString(), out double p))
            {
                return d + p;
            }
            return value;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double result = 0;
            if (values != null)
            {
                foreach (var value in values)
                {
                    if (double.TryParse(value?.ToString(), out double d))
                    {
                        result += d;
                    }
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value?.ToString(), out double d) && double.TryParse(parameter?.ToString(), out double p))
            {
                return d - p;
            }
            return value;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
