using Sia.Domain.Playbook;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Domain.ApiModels.Playbooks
{
    public class CreateAction
    {
        public bool IsUrl { get; set; }
        public CreateActionTemplate NewActionTemplate { get; set; }
        public long? ExistingActionTemplateId { get; set; }
        public ICollection<CreateConditionSet> NewConditionSets { get; set; }
            = new List<CreateConditionSet>();
        public ICollection<long> ExistingConditionSetIds { get; set; }
            = new List<long>();
    }
}
