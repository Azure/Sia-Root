﻿
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design", 
    "CA1054:Uri parameters should not be strings", 
    Justification = "Causes breaking changes and the potential to require additional work around serialization/deserialization would significantly expand the scope of the effort to apply FxCop rules", 
    Scope = "module"
)]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design", 
    "CA1055:Uri return values should not be strings", 
    Justification = "Causes breaking changes and the potential to require additional work around serialization/deserialization would significantly expand the scope of the effort to apply FxCop rules", 
    Scope = "module"
)]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design", 
    "CA1032:Implement standard exception constructors", 
    Justification = "Further review revealed that it's not worth keeping these constructors in the code given that we'll never explicitly invoke them", 
    Scope = "module"
)]



