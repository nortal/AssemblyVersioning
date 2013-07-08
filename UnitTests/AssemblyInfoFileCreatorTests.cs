using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nortal.Utilities.AssemblyVersioning;
using Nortal.Utilities.AssemblyVersioning.Generators;

namespace UnitTests
{
	[TestClass]
	public class AssemblyInfoFileCreatorTests
	{
		[TestMethod]
		public void CheckSkipGeneratorGeneratesNothing()
		{
			var context = new VersionGenerationContext();
			String row = AssemblyInfoFileCreator.GenerateAttributeRow<AssemblyFileVersionAttribute>(new SkipVersionGenerator(), context);
			Assert.IsNull(row);
		}
	}
}
