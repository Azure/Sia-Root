using Sia.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Domain.Playbook
{
    public class Action : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ActionTemplate ActionTemplate { get; set; }
        public ICollection<ConditionSet> ConditionSets { get; set; }
            = new List<ConditionSet>();
    }
}
