﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Project.Infrastructure")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("The Fibre Manager")]
[assembly: AssemblyProduct("Project.Client")]
[assembly: AssemblyCopyright("Copyright ©  2023")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("8cdca7e5-127f-488e-8391-650c80845176")]

// make internal implementations visible to the testing project (circumvent inability to test private methods, without exposing them)
[assembly: InternalsVisibleTo("Project.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]