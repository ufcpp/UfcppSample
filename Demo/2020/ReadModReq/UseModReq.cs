using LibModReq;

class Program
{
    static void Main()
    {
        var c = new Class1();
        c.UnamanagedConstraint<int>();
        c.InParameter(1);
    }
}
