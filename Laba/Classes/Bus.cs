
namespace Buses.Classes
{
    public class Bus:Transport
    {
        public float LuggageCompart {  get;  set; }
        public Bus() : base()
        {
            LuggageCompart = 0;
        }
        public Bus(int capacity, float luggageCompart):base(capacity)
        {
            LuggageCompart = luggageCompart;
        }
        public override string ToString()
        {
            return $"[Bus {Id}]\n" +
                   $"Capacity: {Capacity}\n" +
                   $"Luggage Compartment: {LuggageCompart} m^3";
        }
    }
}
