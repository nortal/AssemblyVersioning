using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nortal.Utilities.AssemblyVersioning;
using Nortal.Utilities.AssemblyVersioning.Generators;
using System;
using System.Fakes;

namespace UnitTests
{
	/// <summary>
	/// Tests various custom formats and supported tags.
	/// </summary>
	[TestClass]
	public class CustomGeneratorTests
	{

		/// <summary>
		/// Verifies that texttemplate logic replaces values using format.
		/// </summary>
		[TestMethod]
		[TestCategory("CustomVersionGenerator")]
		public void CheckCustomFields_BaseVersion()
		{
			var baseVersion = new Version(1, 2, 3, 4);
			String format = @"My custom {Major} {Minor} {Build} {Revision}";
			var result = CalculateVersion<CustomVersionGenerator>(baseVersion, format);

			Assert.AreEqual("My custom 1 2 3 4", result); // all 4 components successfully filled
		}

		/// <summary>
		/// Verifies that template fields accept and respect format strings.
		/// </summary>
		[TestMethod]
		[TestCategory("CustomVersionGenerator")]
		public void CheckCustomFields_NowWithCustomFormat()
		{
			using (ShimsContext.Create())
			{
				ShimDateTime.NowGet = () => new DateTime(2016, 12, 31, 10, 20, 30);
				String format = @"prefix {Now:yyyy|MM|dd-hh|mm|ss} suffix";
				var result = CalculateVersion<CustomVersionGenerator>(new Version(), format);
				Assert.AreEqual("prefix 2016|12|31-10|20|30 suffix", result);
			}
		}

		/// <summary>
		/// Verifies that template fields accept and respect format strings.
		/// </summary>
		[TestMethod]
		[TestCategory("CustomVersionGenerator")]
		public void CheckCustomFields_UtcNowWithCustomFormat()
		{
			using (ShimsContext.Create())
			{
				ShimDateTime.UtcNowGet = () => new DateTime(2016, 12, 31, 10, 20, 30);
				String format = @"prefix {UtcNow:yyyy|MM|dd-hh|mm|ss} suffix";
				var result = CalculateVersion<CustomVersionGenerator>(new Version(), format);
				Assert.AreEqual("prefix 2016|12|31-10|20|30 suffix", result);
			}
		}

		private static String CalculateVersion<TGenerator>(Version baseVersion, String generatorArgument)
			where TGenerator : IVersionGenerator
		{
			var context = new VersionGenerationContext();
			context.BaseVersion = baseVersion;
			context.BuildConfiguration = "MyBuildConfiguration";
			context.VersionGenerationArgument = generatorArgument;

			var generator = Activator.CreateInstance<TGenerator>();
			return generator.GenerateVersionInfoFrom(context);
		}
	}
}
