﻿using Sia.Core.Data;

namespace Sia.Domain
{
    public class TicketingSystem : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
