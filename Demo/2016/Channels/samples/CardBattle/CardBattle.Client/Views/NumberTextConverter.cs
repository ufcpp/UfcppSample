using System;
using System.Globalization;
using System.Windows.Data;

namespace CardBattle.Views
{
    public class NumberTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var num = (int)value;

            switch (num)
            {
                default: return num.ToString();
                case 1: return "A";
                case 11: return "J";
                case 12: return "Q";
                case 13: return "K";
                case 15: return ""; // Joker
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
