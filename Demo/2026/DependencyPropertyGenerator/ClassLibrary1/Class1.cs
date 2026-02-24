using System.Windows;

namespace ClassLibrary1;

public partial class Class1 : DependencyObject
{
    [DependencyPropertyGenerator.DependencyProperty]
    public partial int P { get; set; }
}
