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
		internal static Version Extract(TaskLoggingHelper logger, ITaskItem baseVersionItem)
		{
			if (baseVersionItem == null) { throw new ArgumentNullException("baseVersionItem"); }

			var pathToBaseVersionFile = baseVersionItem.ItemSpec;
			if (!File.Exists(pathToBaseVersionFile))
			{
				logger.LogMessage(MessageImportance.Low, "Working in directory: " + Environment.CurrentDirectory);
				throw new InvalidOperationException("No base assembly info file exists with name " + pathToBaseVersionFile + ".");
			}

			// find assemblyVersion value from file: simplification, but since it is generated file, should be sufficient:
			string versionLineCandidate = File.ReadAllLines(pathToBaseVersionFile)
				.Where(line => !line.StartsWith("//"))
				.FirstOrDefault(line => line.Contains("AssemblyVersion"));

			if (versionLineCandidate == null) { throw new InvalidOperationException(pathToBaseVersionFile + " did not contain expected AssemblyVersionAttribute declaration."); }
			return ParseVersionFromCandidate(logger, pathToBaseVersionFile, versionLineCandidate);

		}

		private static Version ParseVersionFromCandidate(TaskLoggingHelper logger, string pathToBaseVersionFile, string versionLineCandidate)
		{
			try
			{
				//keep it simple. no regex or other fancy ways would be overkill.
				var assemblyVersion = versionLineCandidate.Split('"')[1];

				logger.LogMessage(MessageImportance.Normal, "Using base version: {0}.", assemblyVersion);
				Version version = new Version(assemblyVersion);
				return version;
			}
			catch (Exception exception)
			{
				throw new FormatException(pathToBaseVersionFile + " Assembly version declaration is in invalid format: '" + versionLineCandidate + "'.", exception);
			}
		}
	}
}
