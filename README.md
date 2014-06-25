AssemblyVersioning
==================

Tool to automatically manage versioning numbers of a C# project (assembly, file version, informational configuration info).

Project contains
* Dll implementing various version number generation algorithms
* MSBuild task and target to trigger automatically on every build
* MSBuild props file to configure the MSBuild task
* Nuget packaging for quick applying to your projects 

Licenced under Apache Licence v2.0.

Requirements
-------------
MSBuild task is built using Microsoft .Net Framework 4.0. 
Can be applied to projects of any .Net version as long as MsBuild 4.0 or newer is used (Visual Studio 2010+).

Functionality
-------------
Tool uses AssemblyVersionAttribute value as input and can automatically create the following attributes to project:
* AssemblyInformationalVersionAttribute - to control business-friendly version number
* FileVersionAttribute - to control version number shown in windows explorer.
* ConfigurationAttribute - to attach additional build information.

User can configure which pattern is used for each target attribute. Available patterns:
* HumanReadable2SlotTimestamp
  * 1.23.{5-number-date}.{time}.    Example: 1.23.30423.2059
* HumanReadable1SlotTimestamp
  * 1.23.67.{5-number-date}
* HumanReadableBuildInfo
  * {conf} (on UTC{datetime} by {user} at {machine}
* NugetSemanticVersion
  * 1.23.67[-{conf}-{date}-{time}]    Examples: 1.23.3 -or- 1.2.3-Debug-20130423-2059
* Skip
  * Attribute will not be generated

Getting started
---------------
To install the MsBuild task over <a href="https://nuget.org/packages/Nortal.Utilities.AssemblyVersioning.MsBuildTask/">Nuget</a>, run the following command in the  Package Manager Console 
```
PM> Install-Package Nortal.Utilities.AssemblyVersioning.MsBuildTask
```
On each build the file AssemblyInformationalVersion.gen.cs will be automatically populated with versioning info similar to this:

```
using System.Reflection;

// NB! this file is automatically generated. All manual changes will be lost on build.
// Check ~/_tools/Nortal.Utilities.AssemblyVersioning.props file for other available algorithms and supported attributes.

// Generated based on assembly version: 0.9.0.0
[assembly: AssemblyInformationalVersion("0.9.0")] // algorithm: NugetSemanticVersionGenerator
[assembly: AssemblyFileVersion("0.9.30708.1208")] // algorithm: HumanReadable2SlotTimestampGenerator
[assembly: AssemblyConfiguration("Release (on UTC2013-07-08T12:08:13 by imrep at DELL472)")] // algorithm: HumanReadableBuildInfoGenerator
```

To reference just the dll:
```
PM> Install-Package Nortal.Utilities.AssemblyVersioning.Dll
```
