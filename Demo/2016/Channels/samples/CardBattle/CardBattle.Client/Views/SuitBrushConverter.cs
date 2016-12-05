using CardBattle.Models.GameRules;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CardBattle.Views
{
    public class SuitBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var suit = (Suit)value;

            switch (suit)
            {
                default:
                case Suit.Spades:
                case Suit.Clubs:
                case Suit.Joker:
                    return Brushes.Black;
                case Suit.Hearts:
                case Suit.Diamonds:
                    return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
