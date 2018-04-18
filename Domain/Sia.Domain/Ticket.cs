using Sia.Core.Data;

namespace Sia.Domain
{
    public class Ticket : IEntity,
        IJsonDataObject
    {
        public long Id { get; set; }
        public long IncidentId { get; set; }
        public string OriginId { get; set; }
        public long TicketingSystemId { get; set; }
        public string OriginUri { get; set; }
        public bool IsPrimary { get; set; }
        public object Data { get; set; }
    }
}
