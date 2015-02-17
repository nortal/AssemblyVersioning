===========================================================
              Getting started with
    Nortal.Utilities.AssemblyVersioning.MsBuildTask
===========================================================

On each build the file 'Properties\AssemblyInformationalVersion.gen.cs' will be automatically populated with extra versioning info and compiled. For example, the content may look like this:

	// Generated based on assembly version: 0.9.0.0
	[assembly: AssemblyInformationalVersion("0.9.0")] // algorithm: NugetSemanticVersionGenerator
	[assembly: AssemblyFileVersion("0.9.30708.1208")] // algorithm: HumanReadable2SlotTimestampGenerator
	[assembly: AssemblyConfiguration("Release (on UTC2014-07-08T12:08:13 by userX)")] // algorithm: HumanReadableBuildInfoGenerator

Check the file 'Properties\Nortal.Utilities.AssemblyVersioning.MsBuildTask.props' for other available algorithms.

For more info and source code:
	https://github.com/nortal/AssemblyVersioning

Good luck!
