using Sia.Domain.Playbook;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Domain.ApiModels.Playbooks
{
    public class CreateConditionSet
    {
        public string Name { get; set; }
        public ConditionSetType Type { get; set; }
        public ICollection<CreateCondition> NewConditions { get; set; }
            = new List<CreateCondition>();
        public ICollection<long> ExistingConditionIds { get; set; }
            = new List<long>();
    }
}
