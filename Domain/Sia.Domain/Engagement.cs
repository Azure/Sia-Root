using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Domain
{
    public class Engagement : IEntity
    {
        public long Id { get; set; }
        public long IncidentId { get; set; }
        public DateTime TimeEngaged { get; set; }
        public DateTime? TimeDisengaged { get; set; }
        public Participant Participant {get; set;}
    }
}
