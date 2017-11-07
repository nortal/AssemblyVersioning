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

	This file is from project https://github.com/NortalLTD/AssemblyVersioning, Nortal.Utilities.AssemblyVersioning.Generators, file 'HumanReadable1SlotTimestampGenerator.cs'.
*/

using System;

namespace Nortal.Utilities.AssemblyVersioning.Generators
{
	/// <summary>
	/// Generator to retain 3 left-most version parts and adds a build date stamp for 4-th slot.
	/// <example>1.2.3.40131</example>
	/// </summary>
	public class HumanReadable1SlotTimestampGenerator : HumanReadable2SlotTimestampGenerator
	{
		public override Version GenerateSystemVersion(VersionGenerationContext context)
		{
			if (context == null) { throw new ArgumentNullException(nameof(context)); }

			DateTime now = DateTime.UtcNow;
			var baseVersion = context.BaseVersion;

			//replace just the revision
			int revision = DateToVersionNumberCalculation.BuildDatePart(now); //reuse the logic already built for HumanReadable2SlotTimestampGenerator

			return new Version(baseVersion.Major, baseVersion.Minor, baseVersion.Build, revision);
		}
	}
}
