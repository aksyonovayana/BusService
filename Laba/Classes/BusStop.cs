
namespace Buses.Classes
{
    public class BusStop
    {
        string _name;
        string _location;
        public int Id {  get; set; }
        public string Name {  get { return _name; } set { _name = value; } }
        public string Location { get { return _location; } set { _location = value; } }
        public virtual List<Route>? Routes { get; set; }
        public BusStop(string name, string location)
        {
            _name = name;
            _location = location;
        }
        public override string ToString()
        {
            return $"[Bus Stop]\n" +
                   $"{_name}";
        }
    }
}
