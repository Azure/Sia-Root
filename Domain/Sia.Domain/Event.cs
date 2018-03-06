using Sia.Shared.Data;
using System;

namespace Sia.Domain
{
    public class Event : IEntity, IJsonDataObject
    {
        public long Id { get; set; }
        public long? IncidentId { get; set; }
        public long EventTypeId { get; set; }
        public DateTime Occurred { get; set; }
        public DateTime EventFired { get; set; }
        public object Data { get; set; }
        public string PrimaryTicketId { get; set; }
    }
}