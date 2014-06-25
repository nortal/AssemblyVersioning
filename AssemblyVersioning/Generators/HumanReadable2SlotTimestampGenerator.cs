/*
	Copyright 2013 Imre P�hvel, AS Nortal
	
	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

		http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.

	This file is from project https://github.com/NortalLTD/AssemblyVersioning, Nortal.Utilities.AssemblyVersioning.Generators, file 'HumanReadable2SlotTimestampGenerator.cs'.
*/

using System;

namespace Nortal.Utilities.AssemblyVersioning.Generators
{
	/// <summary>
	/// Generator to retain 2 left-most version parts and adds a build date and time stamp for 3-th and 4th slots.
	/// <example>1.2.41231.2359</example>
	/// </summary>
	public class HumanReadable2SlotTimestampGenerator : SystemVersionGeneratorBase
	{

		internal static int BuildDatePart(DateTime forTime)
		{
			//last digit mod 6 (max value in version component is 65536). Ex: 2016 -> 6; 2017 -> 0
			int yearpart = forTime.Year % 10 % 7;
			return yearpart * 10000 + 100 * forTime.Month + forTime.Day;
		}

		internal static int BuildTimePart(DateTime forTime)
		{
			return forTime.Hour * 100 + forTime.Minute;
		}

		public override Version GenerateSystemVersion(VersionGenerationContext context)
		{
			if (context == null) { throw new ArgumentNullException("context"); }

			DateTime buildDate = DateTime.UtcNow;
			var build = BuildDatePart(buildDate);
			var revision = BuildTimePart(buildDate);

			var baseVersion = context.BaseVersion;
			return new Version(baseVersion.Major, baseVersion.Minor, build, revision);
		}
	}
}
