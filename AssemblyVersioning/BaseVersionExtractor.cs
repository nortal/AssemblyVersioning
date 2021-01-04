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

	This file is from project https://github.com/NortalLTD/AssemblyVersioning, Nortal.Utilities.AssemblyVersioning, file 'BaseVersionExtractor.cs'.
*/

using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Nortal.Utilities.AssemblyVersioning
{
	internal static class BaseVersionExtractor
	{

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AssemblyVersionAttribute", Justification = "type name")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AssemblyInfo", Justification = "Part of folder path")]
		internal static Version Extract(GenerateExtendedAssemblyInfoTask task)
		{
			if (!String.IsNullOrEmpty(task.BaseVersion))
			{
				task.Log.LogMessage(MessageImportance.Low, "Found base version from project: " + task.BaseVersion);
				return new Version(task.BaseVersion);
			}

			var pathToBaseVersionFile = task.BaseVersionFile.ItemSpec;
			if (!File.Exists(pathToBaseVersionFile))
			{
				task.Log.LogError("", "NV0000", "", pathToBaseVersionFile, 0, 0, 0, 0,
					$"'Could not find AssemblyVersion from '{task.BaseVersionFile}'. This is required to set base version used for generating other version attributes. See https://github.com/nortal/AssemblyVersioning/wiki/Generator-options for more info.");
				return new Version();
			}

			// find assemblyVersion value from file: simplification, but since it is generated file, should be sufficient:
			string[] fileContents = File.ReadAllLines(pathToBaseVersionFile)
				.Where(line => !line.TrimStart().StartsWith("//"))
				.ToArray();

			string versionLineCandidate = fileContents.FirstOrDefault(line => line.Contains("AssemblyVersion"));
			if (versionLineCandidate == null)
			{
				task.Log.LogError("", "NV0001", "", pathToBaseVersionFile, 0, 0, 0, 0,
					$"'Could not find AssemblyVersion from '{task.BaseVersionFile}'. This is required to set base version used for generating other version attributes. See https://github.com/nortal/AssemblyVersioning/wiki/Generator-options for more info.");
				return new Version();
			}
			var baseVersion = ParseVersionFromCandidate(task.Log, pathToBaseVersionFile, versionLineCandidate);

			RequireFileVersionAttributeNotDuplicated(task, pathToBaseVersionFile, fileContents);
			RequireConfigurationAttributeNotDuplicated(task, pathToBaseVersionFile, fileContents);
			return baseVersion;
		}

		private static void RequireFileVersionAttributeNotDuplicated(GenerateExtendedAssemblyInfoTask task, string pathToBaseVersionFile, string[] fileContents)
		{
			string fileVersion = fileContents.FirstOrDefault(line => line.Contains("[assembly: AssemblyFileVersion("));
			if (fileVersion != null && task.GeneratorForFileVersion != "Skip")
			{
				task.Log.LogError("", "NV0002", "", pathToBaseVersionFile, 0, 0, 0, 0,
					$"'{task.BaseVersionFile}' contains FileVersionAttribute. This is in conflict with auto-generated attribute. Please remove this conflicting attribute or omit FileVersionAttribute from automatic generation. See https://github.com/nortal/AssemblyVersioning/wiki/Generator-options for more info.");
			}
		}

		private static void RequireConfigurationAttributeNotDuplicated(GenerateExtendedAssemblyInfoTask task, string pathToBaseVersionFile, string[] fileContents)
		{
			string configuration = fileContents.FirstOrDefault(line => line.Contains("[assembly: AssemblyConfiguration("));
			if (configuration != null && task.GeneratorForConfiguration != "Skip")
			{
				task.Log.LogError("", "NV0003", "", pathToBaseVersionFile, 0, 0, 0, 0,
					$"'{task.BaseVersionFile}' contains AssemblyConfigurationAttribute. This is in conflict with auto-generated attribute. Please remove this conflicting attribute or omit AssemblyConfigurationAttribute from automatic generation. See https://github.com/nortal/AssemblyVersioning/wiki/Generator-options for more info.");
			}
		}

		private static Version ParseVersionFromCandidate(TaskLoggingHelper logger, string pathToBaseVersionFile, string versionLineCandidate)
		{
			try
			{
				//keep it simple. no regex or other fancy ways would be overkill.
				var assemblyVersion = versionLineCandidate.Split('"')[1];

				logger.LogMessage(MessageImportance.Normal, "Using base version: {0}.", assemblyVersion);
				var version = new Version(assemblyVersion);
				return version;
			}
			catch (Exception exception)
			{
				throw new FormatException(pathToBaseVersionFile + " Assembly version declaration is in invalid format: '" + versionLineCandidate + "'.", exception);
			}
		}
	}
}
