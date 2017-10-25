using Sia.Domain.Playbook;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Domain.ApiModels.Playbooks
{
    public class CreateCondition
    {
        public string Name { get; set; }
        public AssertionType AssertionType { get; set; }
            = AssertionType.IsOrDoes;
        public ConditionType ConditionType { get; set; }
        public DataFormat DataFormat { get; set; }
            = DataFormat.String;
        public string ComparisonValue { get; set; }
        public long? IntegerComparisonValue { get; set; }
        public DateTime DateTimeComparisonValue { get; set; }
        public long? ExistingConditionSourceId { get; set; }
    }
}
