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

	This file is from project https://github.com/NortalLTD/AssemblyVersioning, Nortal.Utilities.AssemblyVersioning.Generators, file 'SystemVersionGeneratorBase.cs'.
*/

using System;
using Nortal.Utilities.AssemblyVersioning;

namespace Nortal.Utilities.AssemblyVersioning.Generators
{
	/// <summary>
	/// Base class for generators outputting traditional 4-part versions (for example 1.2.3.4).
	/// </summary>
	public abstract class SystemVersionGeneratorBase : IVersionGenerator
	{
		public abstract Version GenerateSystemVersion(VersionGenerationContext context);

		#region IVersionGenerator Members
		String IVersionGenerator.GenerateVersionInfoFrom(VersionGenerationContext context)
		{
			Version version = GenerateSystemVersion(context);
			return version != null ? version.ToString() : null;
		}
		#endregion
	}
}
