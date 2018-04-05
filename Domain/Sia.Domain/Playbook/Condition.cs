using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Domain.Playbook
{
    public class Condition : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public AssertionType AssertionType { get; set; }
            = AssertionType.IsOrDoes;
        public ConditionType ConditionType { get; set; }
        public DataFormat DataFormat { get; set; }
            = DataFormat.String;
        public string ComparisonValue { get; set; }
        public long? IntegerComparisonValue { get; set; }
        public DateTime DateTimeComparisonValue { get; set; }
        public Source ConditionSource { get; set; }
    }

    public enum AssertionType
    {
        IsOrDoes,
        IsNotOrDoesNot
    }

    public enum ConditionType
    {
        Equal,
        Contain,
        HaveValue,
        GreaterThan,
        LessThan
    }

#pragma warning disable CA1720 // Identifier contains type name
    public enum DataFormat
    {
        String,
        DateTime,
        Integer
    }
#pragma warning restore CA1720 // Identifier contains type name
}
