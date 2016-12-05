using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CardBattle.Views
{
    public class PlayerColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var playerId = (byte)value;

            switch (playerId)
            {
                default:
                case 0: return Brushes.DarkRed;
                case 1: return Brushes.DarkBlue;
                case 2: return Brushes.DarkOliveGreen;
                case 3: return Brushes.DarkViolet;
                case 4: return Brushes.DarkGoldenrod;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
