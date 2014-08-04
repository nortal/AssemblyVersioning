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
	public sealed class VersionGenerationContext
	{
		public Version BaseVersion { get; internal set; }
		public String BuildConfiguration { get; internal set; }

		public Boolean IsPrerelease
		{
			get
			{
				//convention-based:
				//		*Release* -> NOT prerelease
				//		everything else (including Debug) IS prerelease.
				return !(BuildConfiguration ?? "").ToLower().Contains("release");
			}
		}
	}
}
