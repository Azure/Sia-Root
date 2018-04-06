using Sia.Shared.Data;
using System.Collections.Generic;

namespace Sia.Domain.Playbook
{
    public abstract class TemplateBase : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Pattern { get; set; }
        public ICollection<Source> Sources { get; set; }
            = new List<Source>();
    }
}
