
using System.ComponentModel.DataAnnotations;

namespace Buses.Classes
{
    public class Ticket
    {
        int _number;
        decimal _price;
        string _description;
        Passenger _passenger;
        Journey _journey;
        [Key]
        public int Number { get { return _number; } set { _number = value; } }
        public decimal Price { get { return _price; } set { _price = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        public virtual int PassengerId { get; set; }
        public virtual Passenger Passenger { get { return _passenger; } set { _passenger = value; } }
        public virtual int JourneyId { get; set; }
        public virtual Journey Journey {  get { return _journey; } set { _journey = value; } }
        public Ticket()
        {
            _price = 0;
            _description = string.Empty;
            _journey= new Journey();
            _passenger = new Passenger();
        }
        public Ticket(decimal price, string description,Journey journey,Passenger passenger)
        {
            _price = price;
            _description = description;
            _journey = journey;
            _passenger = passenger;
        }
        public override string ToString()
        {
            return $"[Ticket]№{_number}\n" +
                   $"{_price}$\n" +
                   $"{_description}";
        }
    }
}
