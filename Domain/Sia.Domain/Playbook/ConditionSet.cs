using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Domain.Playbook
{
    public class ConditionSet : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ConditionSetType Type { get; set; }
        public ICollection<Condition> Conditions { get; set; }
            = new List<Condition>();
    }

    public enum ConditionSetType
    {
        AnyOf,
        AllOf,
        NoneOf
    }
}
