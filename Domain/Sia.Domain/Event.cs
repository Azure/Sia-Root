using Sia.Domain;
using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;

namespace Sia.Domain
{
    public class Event : IEntity, IJsonDataObject
    {
        public long Id { get; set; }
        public long? IncidentId { get; set; }

        public Incident Incident { get; set; }
        public long EventTypeId { get; set; }
        public DateTime Occurred { get; set; }
        public DateTime EventFired { get; set; }
        public object Data { get; set; }
        public Ticket PrimaryTicket
        {
            get
            {
                return Incident?.Tickets?.FirstOrDefault(ticket => ticket.IsPrimary);
            }
        }
        public string PrimaryTicketId
        {
            get
            {
                return PrimaryTicket?.OriginId;
            }
        }
    }
}
