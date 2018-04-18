using Sia.Core.Data;
using System;

namespace Sia.Domain
{
#pragma warning disable CA1716 // Identifiers should not match keywords (changing the name of this entity is out of the scope of this work)
    public class Event : IEntity, IJsonDataObject
#pragma warning restore CA1716 // Identifiers should not match keywords
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
