using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Lyrida.DataAccess")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Lyrida")]
[assembly: AssemblyProduct("Lyrida.Server")]
[assembly: AssemblyCopyright("Copyright ©  2023")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("a259c7d4-139c-4b56-aa87-bcc6541ca083")]

// make internal implementations visible to the testing project (circumvent inability to test private methods, without exposing them)
[assembly: InternalsVisibleTo("Lyrida.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]