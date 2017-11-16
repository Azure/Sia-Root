using Sia.Shared.Data;
using System.Collections.Generic;


namespace Sia.Domain.Playbook
{
    public class EventType : IEntity, IJsonDataObject
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public object Data { get; set; }
        public ICollection<Action> Actions { get; set; }
            = new List<Action>();

        public object displayTemplate { get; set; }
    }
}
