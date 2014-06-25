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

	This file is from project https://github.com/NortalLTD/AssemblyVersioning, Nortal.Utilities.AssemblyVersioning.Generators, file 'NugetSemanticVersionGenerator.cs'.
*/

using System;

namespace Nortal.Utilities.AssemblyVersioning.Generators
{
	public class NugetSemanticVersionGenerator : IVersionGenerator
	{
		public string GenerateVersionInfoFrom(VersionGenerationContext context)
		{
			if (context == null) { throw new ArgumentNullException("context"); }

			var now = DateTime.UtcNow;

			String prereleasePart = null;
			if (context.IsPrerelease)
			{
				//NB! nuget limits prerelease version string to 20 chars, must start with a letter, contrary to semantic versioning standard no dot, no + allowed.
				//ex: 1.2.3-Debug-20130423-1459
				//ex: 1.2.3-RC-20130423-1459
				// note: we cannot use simple iterator for prerelease (alpha-1, alpha-2, etc) as code has not and should not have any information about previous builds.
				prereleasePart = String.Format("-{0}-{1}-{2}",
					TrimToLength(context.BuildConfiguration, 6), //up to 6 chars
					//1 char from pattern
				now.ToString("yyyyMMdd"), //8 chars
					//1 char from pattern
				now.ToString("HHmm") //4 chars
				);
			} //else release version without prerelease part. Ex: "1.2.3"

			return String.Format("{0}.{1}.{2}{3}",
				context.BaseVersion.Major,
				context.BaseVersion.Minor,
				context.BaseVersion.Build,
				prereleasePart
			);
		}

		private static String TrimToLength(String input, int lengthLimit)
		{
			if (input == null || input.Length <= lengthLimit) { return input; }
			return input.Substring(0, lengthLimit);
		}
	}
}
