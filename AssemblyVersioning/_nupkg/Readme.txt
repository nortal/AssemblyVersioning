===========================================================
              Getting started with
    Nortal.Utilities.AssemblyVersioning.MsBuildTask
===========================================================

On each build the 'AssemblyInformationalVersion.gen.cs' will be automatically regenerated with extra versioning info and included in compilation. For example, the generated content may look like this:

	// Generated based on assembly version: 2.9.0.0
	[assembly: AssemblyInformationalVersion("2.9.0")] // algorithm: NugetSemanticVersionGenerator
	[assembly: AssemblyFileVersion("2.9.30708.1208")] // algorithm: HumanReadable2SlotTimestampGenerator
	[assembly: AssemblyConfiguration("Release (on UTC2014-07-08T12:08:13 by userX)")] // algorithm: HumanReadableBuildInfoGenerator

The generated file can be safely deleted at any time and should be marked as ignored for source control systems.

For more info and source code:
	https://github.com/nortal/AssemblyVersioning
For configuration options and examples:
	https://github.com/nortal/AssemblyVersioning/wiki/Generator-options
	https://github.com/nortal/AssemblyVersioning/wiki/Example-configuration-file

Good luck!