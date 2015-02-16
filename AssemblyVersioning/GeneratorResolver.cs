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

	This file is from project https://github.com/NortalLTD/AssemblyVersioning, Nortal.Utilities.AssemblyVersioning, file 'GeneratorResolver.cs'.
*/

using System.Globalization;
using Nortal.Utilities.AssemblyVersioning.Generators;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nortal.Utilities.AssemblyVersioning
{
	/// <summary>
	/// Responsible for findin a generator class instance to use based on user configuration input.
	/// </summary>
	public static class GeneratorResolver
	{
		/// <summary>
		/// Lists all supported generators automatically found in this assembly.
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<Type> FindAllGeneratorTypes()
		{
			Type searchInterface = typeof(IVersionGenerator);
			foreach (var type in typeof(GeneratorResolver).Assembly.GetTypes())
			{
				if (type.IsAbstract || type.IsInterface) { continue; }
				if (searchInterface.IsAssignableFrom(type))
				{
					yield return type;
				}
			}
		}

		/// <summary>
		/// Scans included generators and tries to find a match for provided generator name.
		/// </summary>
		/// <param name="nameWithArgument"></param>
		/// <returns></returns>
		public static IVersionGenerator ResolveWithArgument(String nameWithArgument, VersionGenerationContext context)
		{
			if (String.IsNullOrEmpty(nameWithArgument)) { return new SkipVersionGenerator(); }
			// argument is separated from name by colon. Name cannot contain special symbols:
			Regex regex = new Regex("^(?<namePart>[a-zA-Z]+)(:(?<argumentPart>.+))?$");
			var match = regex.Match(nameWithArgument);
			if (match.Success)
			{
				context.VersionGenerationArgument = match.Groups["argumentPart"].Value;
				var namePart = match.Groups["namePart"].Value;
				return ResolveByName(namePart);
			}

			context.VersionGenerationArgument = nameWithArgument; // generator name was omitted, everything is context.
			return new CustomVersionGenerator(); // input not recognized. Assume custom context.
		}

		private static IVersionGenerator ResolveByName(String name)
		{
			foreach (var type in FindAllGeneratorTypes())
			{
				if (CheckNameMatchesType(name, type))
				{
					return (IVersionGenerator)Activator.CreateInstance(type);
				}
			}
			throw new InvalidOperationException(String.Format("No generator was found by name {0}.", name));
		}

		private static bool CheckNameMatchesType(String name, Type type)
		{
			//try names with a fallback logic.
			return CheckNameMatchesTypeNameExactly(name, type)
				|| CheckNameMatchesTypeNameExactly(name + "Generator", type) // if suffix was omitted
				|| CheckNameMatchesTypeNameExactly(name + "VersionGenerator", type); // if suffix was omitted
		}

		private static bool CheckNameMatchesTypeNameExactly(String name, Type type)
		{
			return String.Compare(type.Name, name, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase) == 0; // be lenient on case - user does not care much about case in configuration.
		}
	}
}
