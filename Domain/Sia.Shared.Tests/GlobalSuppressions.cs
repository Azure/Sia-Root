
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming", 
    "CA1707:Identifiers should not contain underscores", 
    Justification = "Test methods are more readable with underscores", 
    Scope = "module"
)]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1716: Identifiers should not match keywords",
    Justification = "This rule is triggering on the namespace 'Shared'. This namespacing isn't particularly confusing to consumers of this library. Also, 'Shared' is a keyword in VB, not C#.",
    Scope = "module"
)]