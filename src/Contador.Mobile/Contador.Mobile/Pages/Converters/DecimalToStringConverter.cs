using System;
using System.Globalization;

using Xamarin.Forms;

namespace Contador.Mobile.Pages.Converters
{
	public class DecimalToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (string.IsNullOrEmpty(value?.ToString() ?? string.Empty))
			{
				return "0";
			}
			else if (value is decimal)
			{
				return value.ToString();
			}

			return "0";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (string.IsNullOrEmpty(value?.ToString() ?? string.Empty))
				return 0;
			
			return decimal.Parse(value.ToString());
		}
	}
}
