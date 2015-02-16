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

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Nortal.Utilities.AssemblyVersioning
{
	/// <summary>
	/// MSBuild task to generate a code file with extended assembly versioning information.
	/// </summary>
	public class GenerateExtendedAssemblyInfoTask : Task
	{
		/// <summary>
		/// if not set assumes assembly info is declared in default file Properties/AssemblyInfo.cs
		/// </summary>
		[Required]
		public ITaskItem BaseVersionFile { get; set; }

		/// <summary>
		/// Target file to generate extra versioning info to.
		/// </summary>
		[Required]
		public ITaskItem OutputFile { get; set; }

		public String BuildConfiguration { get; set; }

		public String GeneratorForFileVersion { get; set; }
		public String GeneratorForInformationalVersion { get; set; }
		public String GeneratorForConfiguration { get; set; }

		/// <summary>
		/// The entry-point to task functionality.
		/// </summary>
		/// <returns></returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public override bool Execute()
		{
			Log.LogMessage(MessageImportance.Low, this.GetType().Name + " starting..");
			try
			{
				var context = InitializeContext();
				var generatedFileContent = GenerateInformationalVersionInfoFile(context);
				
				Log.LogMessage("Overwriting extended assembly version information in file: {0} ..", this.OutputFile.ItemSpec);
				File.WriteAllText(this.OutputFile.ItemSpec, generatedFileContent);
				return true;
			}
			catch (System.Exception exception)
			{
				Log.LogErrorFromException(exception, showStackTrace: true);
				return false;
			}
		}

		private VersionGenerationContext InitializeContext()
		{
			var context = new VersionGenerationContext();
			context.BaseVersion = BaseVersionExtractor.Extract(this.Log, this.BaseVersionFile);
			context.BuildConfiguration = this.BuildConfiguration;
			return context;
		}

		internal String GenerateInformationalVersionInfoFile(VersionGenerationContext context)
		{
			String contents = AssemblyInfoFileCreator.GetHeader(context)
				+ GenerateAttributeLine<AssemblyInformationalVersionAttribute>(context, GeneratorForInformationalVersion) 
				+ GenerateAttributeLine<AssemblyFileVersionAttribute>(context, GeneratorForFileVersion)
				+ GenerateAttributeLine<AssemblyConfigurationAttribute>(context, GeneratorForConfiguration);

			return contents;
		}

		internal String GenerateAttributeLine<TAttribute>(VersionGenerationContext context, String generatorName)
			where TAttribute: Attribute
		{
			this.Log.LogMessage("Generating content for '{0}' using algorithm '{1}'..", typeof(TAttribute).Name, generatorName);
			var generator = GeneratorResolver.ResolveWithArgument(generatorName, context);
			Debug.Assert(generator != null);

			String row = AssemblyInfoFileCreator.GenerateAttributeRow<TAttribute>(generator, context);
			return row;
		}
	}
}
