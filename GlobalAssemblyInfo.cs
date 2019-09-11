using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: CLSCompliant(true)]

[assembly: AssemblyProduct("Common.FileTransfer")]
[assembly: AssemblyCompany("Isogeo")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCopyright("Copyright © 2012-2019 Isogeo")]
[assembly: AssemblyTrademark("")]
