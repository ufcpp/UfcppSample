using CardBattle.Models.GameRules;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CardBattle.Views
{
    public class SuitStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var suit = (Suit)value;

            switch (suit)
            {
                case Suit.Spades: return "♠";
                case Suit.Hearts: return "♥";
                case Suit.Diamonds: return "♦";
                case Suit.Clubs: return "♣";
                default:
                case Suit.Joker: return "🃏";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
