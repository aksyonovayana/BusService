using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Buses.Classes
{
    public delegate void TicketNotifHandler(Ticket ticket);
    public class Passenger
    {
        string _fullName;
        List<Ticket> _tickets;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {  get; set; }
        [DefaultValue("MissingNAME")]
        [MaxLength(30)]
        public string FullName {  get { return _fullName; } set { _fullName = value; } }
        public virtual List<Ticket> Tickets { get {  return _tickets; } set { _tickets = value; } }
        public event TicketNotifHandler OnDeparture;
        public Passenger()
        {
            _fullName = string.Empty;
            _tickets = new List<Ticket>();
        }
        public Passenger(string fullName)
        {
            _fullName = fullName;
            _tickets = new List<Ticket>();
        }
        public int BuyTicket(Journey journey, string description)
        {
            decimal price = journey.Route.Stops.Count * 5.65m;
            Ticket ticket = new Ticket(price, description,journey,this);
            _tickets.Add(ticket);
            journey.Tickets.Add(ticket);
            return 0;
        }
        public override string ToString()
        {
            return $"[Passenger]\n" +
                   $"{_fullName}\n" +
                   $"Tickets: {_tickets.Count}";
        }
        public void CheckTickets()
        {
            foreach (Ticket ticket in _tickets)
            {
                if (ticket.Journey.Departure.AddHours(-2)>=DateTime.Now)
                {
                    OnDeparture?.Invoke(ticket);
                }
            }

        }
    }
}
