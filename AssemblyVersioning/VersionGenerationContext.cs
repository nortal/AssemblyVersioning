/*
	Copyright 2013 Imre Pühvel, AS Nortal
	
	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

		http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.

	This file is from project https://github.com/NortalLTD/AssemblyVersioning, Nortal.Utilities.AssemblyVersioning, file 'VersionGenerationContext.cs'.
*/

using System;

namespace Nortal.Utilities.AssemblyVersioning
{
	/// <summary>
	/// Contains all available data that can be used as input for generating version numbers.
	/// </summary>
	public sealed class VersionGenerationContext
	{
		/// <summary>
		/// Version as parsed from AssemblyVersion attribute of target project AssemblyInfo.cs file.
		/// </summary>
		public Version BaseVersion { get; internal set; }

		/// <summary>
		/// Visual studio build configuration name (ex: Debug/Release/..).
		/// </summary>
		public String BuildConfiguration { get; internal set; }

		/// <summary>
		/// Indicator if given package is production-ready or prerelease (if supported by generator).
		/// </summary>
		public Boolean IsPrerelease {get; internal set;}

		/// <summary>
		/// IVersionGenerator type dependent argument provided from user configuration.
		/// </summary>
		public String VersionGenerationArgument { get; set; }
	}
}
