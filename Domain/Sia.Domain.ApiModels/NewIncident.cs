using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sia.Domain.ApiModels
{
    public class NewIncident
    {
        [Required]
        public string Title { get; set; }
        [Required]
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
        public IList<Ticket> Tickets { get; set; }
            = new List<Ticket>();
        public IList<NewEvent> Events { get; set; }
            = new List<NewEvent>();
        public IList<Participant> Engagements { get; set; }
            = new List<Participant>();
    }
}
