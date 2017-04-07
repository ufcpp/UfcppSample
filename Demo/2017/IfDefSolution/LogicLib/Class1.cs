using DataLib;

namespace LogicLib
{
    public class Class1User
    {
        private readonly Class1 _data;
        public Class1User(int id) => _data = new Class1 { Id = id};
        public int Id => _data.Id;
    }
}
