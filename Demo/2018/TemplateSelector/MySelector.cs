using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
    public enum SelectorMode
    {
        Summary,
        Detail,
    }

    public class MySelector : DataTemplateSelector
    {
        class Pair
        {
            public DataTemplate Summary { get; }
            public DataTemplate Detail { get; }
            public Pair(DataTemplate summary, DataTemplate detail) => (Summary, Detail) = (summary, detail);
        }

        Dictionary<Type, Pair> _templates = new Dictionary<Type, Pair>();
        Pair _intTemplate;
        Pair _stringTemplate;

        Pair GetPair(object item)
        {
            switch (item)
            {
                case byte _:
                case sbyte _:
                case short _:
                case ushort _:
                case int _:
                case uint _:
                case long _:
                case ulong _:
                    if (_intTemplate == null) _intTemplate = Load(new Uri("/WpfApp2;component/Templates/Integer.xaml", UriKind.Relative));
                    return _intTemplate;
                case string _:
                    if (_stringTemplate == null) _stringTemplate = Load(new Uri("/WpfApp2;component/Templates/String.xaml", UriKind.Relative));
                    return _stringTemplate;
                case null: return null;
            }

            var t = item.GetType();

            //todo: enum
            //todo: ObservableCollection の分岐もこれでやる？

            if (!_templates.TryGetValue(t, out var p))
            {
                //todo: assembly name は外から受け取る
                var path = "/WpfApp2;component/Templates/" + string.Join("/", t.Namespace.Split('.')) + "/" + t.Name + ".xaml";
                p = Load(new Uri(path, UriKind.Relative));
                //todo: なかった場合
                _templates[t] = p;
            }

            return p;
        }

        private static Pair Load(Uri uri)
        {
            var dic = (ResourceDictionary)Application.LoadComponent(uri);
            var pair = new Pair(dic["Summary"] as DataTemplate, dic["Detail"] as DataTemplate);
            return pair;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return base.SelectTemplate(item, container);

            var pair = GetPair(item);

            switch (container.GetValue(ModeProperty))
            {
                default:
                case SelectorMode.Detail: return pair.Detail;
                case SelectorMode.Summary: return pair.Summary;
            }
        }

        public static void SetMode(Control obj, SelectorMode value) => obj.SetValue(ModeProperty, value);
        public static SelectorMode GetMode(Control obj) => (SelectorMode)obj.GetValue(ModeProperty);

        public static readonly DependencyProperty ModeProperty = DependencyProperty.RegisterAttached(
            "Mode",
            typeof(SelectorMode),
            typeof(MySelector),
            new FrameworkPropertyMetadata(SelectorMode.Detail) { Inherits = true });
    }
}
