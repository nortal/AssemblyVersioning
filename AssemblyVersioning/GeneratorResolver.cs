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

	This file is from project https://github.com/NortalLTD/AssemblyVersioning, Nortal.Utilities.AssemblyVersioning, file 'GeneratorResolver.cs'.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nortal.Utilities.AssemblyVersioning.Generators;

namespace Nortal.Utilities.AssemblyVersioning
{
	public static class GeneratorResolver
	{
		public static IEnumerable<Type> FindAllGeneratorTypes()
		{
			var generators = new List<IVersionGenerator>();

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

		public static IVersionGenerator ResolveByName(String name)
		{
			if (String.IsNullOrEmpty(name)) { return new SkipVersionGenerator(); }

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
			if (String.Compare(type.Name, name, ignoreCase: true) == 0
				|| String.Compare(type.Name, name + "Generator", ignoreCase: true) == 0
				|| String.Compare(type.Name, name + "VersionGenerator", ignoreCase: true) == 0)
			{
				return true;
			}
			return false;
		}
	}
}
