using Sia.Shared.Data;
using System.Collections.Generic;
using System.Linq;

namespace Sia.Domain
{
    public class Incident : IEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }

        public Ticket PrimaryTicket
        {
            get
            {
                return Tickets.FirstOrDefault(ticket => ticket.IsPrimary);
            }
            set
            {
                if (Tickets == null) Tickets = new List<Ticket>();
                foreach (var ticket in Tickets.Where(ticket => ticket.IsPrimary))
                {
                    ticket.IsPrimary = false;
                }

                if (value == null) return;

                if (!Tickets.Contains(value)) Tickets.Add(value);
                value.IsPrimary = true;
            }
        }
        public ICollection<Ticket> Tickets { get; set; }
            = new List<Ticket>();
        public ICollection<Event> Events { get; set; }
            = new List<Event>();
        public ICollection<Engagement> Engagements { get; set; }
            = new List<Engagement>();
    }
}
