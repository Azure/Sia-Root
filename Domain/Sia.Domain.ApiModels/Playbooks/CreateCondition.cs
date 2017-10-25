using Sia.Domain.Playbook;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Domain.ApiModels.Playbooks
{
    public class CreateCondition
    {
        public string Name { get; set; }
        public ConditionType ConditionType { get; set; }
        public string ComparisonValue { get; set; }
        public CreateConditionSource NewConditionSource { get; set; }
        public long? ExistingConditionSourceId { get; set; }
    }
}
