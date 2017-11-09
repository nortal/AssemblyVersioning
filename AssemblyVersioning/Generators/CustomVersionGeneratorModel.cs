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

using System;

namespace Nortal.Utilities.AssemblyVersioning.Generators
{
	public sealed class CustomizedVersionModel
	{
		public CustomizedVersionModel(VersionGenerationContext context)
		{
			this.Context = context ?? throw new ArgumentNullException(nameof(context));
		}

		// storage for model properties
		private VersionGenerationContext Context { get; set; }
		private DateTime FixedNow = DateTime.Now; // to ensure it does not change between multiple calls
		private DateTime FixedUtcNow = DateTime.UtcNow; // to ensure it does not change between multiple calls

		// BaseVersion parts:
		public int Major => this.Context.BaseVersion.Major;
		public int Minor => this.Context.BaseVersion.Minor;
		public int Build => this.Context.BaseVersion.Build;
		public int Revision => this.Context.BaseVersion.Revision;

		// date components:
		public DateTime Now => this.FixedNow;
		public int DateNumber => DateToVersionNumberCalculation.BuildDatePart(this.FixedNow);
		public int TimeNumber => DateToVersionNumberCalculation.BuildTimePart(this.FixedNow);

		public DateTime UtcNow => this.FixedUtcNow;
		public int UtcDateNumber => DateToVersionNumberCalculation.BuildDatePart(this.FixedUtcNow);
		public int UtcTimeNumber => DateToVersionNumberCalculation.BuildTimePart(this.FixedUtcNow);

		// user context:
		public String BuildConfiguration => this.Context.BuildConfiguration;

		// Environment
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		public String Domain => Environment.UserDomainName;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		public String UserName => Environment.UserName;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		public String MachineName => Environment.MachineName;

		//Custom fields that are filled from MsBuild side
		public String CustomField1 => this.Context.CustomField1;
		public String CustomField2 => this.Context.CustomField2;
		public String CustomField3 => this.Context.CustomField3;
	}
}
