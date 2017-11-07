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

	This file is from project https://github.com/NortalLTD/AssemblyVersioning, Nortal.Utilities.AssemblyVersioning, file 'GenerateExtendedAssemblyInfoTask.cs'.
*/

using Nortal.Utilities.TextTemplating;
using System;

namespace Nortal.Utilities.AssemblyVersioning.Generators
{
	/// <summary>
	/// Version generator which takes a custom format from context argument and injects the placeholders in format with values from model.
	/// </summary>
	public sealed class CustomVersionGenerator : IVersionGenerator
	{
		public string GenerateVersionInfoFrom(VersionGenerationContext context)
		{
			if (context == null) { throw new ArgumentNullException(nameof(context)); }

			String customFormat = context.VersionGenerationArgument;
			if (String.IsNullOrEmpty(customFormat)) { throw new ArgumentException("Custom format requested but no format was provided in VersionGenerationArgument.", "context"); }

			var model = new CustomizedVersionModel(context);

			var settings = new SyntaxSettings()
			{
				BeginTag = "{",
				EndTag = "}",
			};

			var templateEngine = new TemplateProcessingEngine(settings);
			String result = templateEngine.Process(customFormat, model);
			return result;
		}
	}
}
