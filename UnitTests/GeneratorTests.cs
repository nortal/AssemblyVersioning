using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nortal.Utilities.AssemblyVersioning;
using Nortal.Utilities.AssemblyVersioning.Generators;
using System;
using System.Fakes;

namespace UnitTests
{
	[TestClass]
	public class GeneratorTests
	{

		[TestMethod]
		[TestCategory("PreparedGenerators")]
		public void CheckSkipVersionGeneratorReturnsNull()
		{
			var generator = new SkipVersionGenerator();
			var context = new VersionGenerationContext();
			var version = generator.GenerateSystemVersion(context);
			Assert.IsNull(version, "Expected null, got: {0}.", version);
		}

		[TestMethod]
		[TestCategory("PreparedGenerators")]
		public void CheckHumanReadable1SlotTimeStampGenerator()
		{
			using (ShimsContext.Create())
			{
				ShimDateTime.UtcNowGet = () => new DateTime(2016, 12, 31, 10, 20, 30);

				var baseVersion = new Version(1, 2, 3, 0);
				var result = CalculateSystemVersion<HumanReadable1SlotTimestampGenerator>(baseVersion);

				//retains 3 slots:
				Assert.AreEqual(baseVersion.Major, result.Major, "Major version mismatch, expected {0}, got {1}", baseVersion.Major, result.Major);
				Assert.AreEqual(baseVersion.Minor, result.Minor, "Minor version mismatch, expected {0}, got {1}", baseVersion.Minor, result.Minor);
				Assert.AreEqual(baseVersion.Build, result.Build, "Build version mismatch, expected {0}, got {1}", baseVersion.Build, result.Build);
				Assert.AreEqual(61231, result.Revision, "Generated revision number must match date");
			}
		}

		[TestMethod]
		[TestCategory("PreparedGenerators")]
		public void CheckHumanReadable2SlotTimeStampGenerator()
		{
			using (ShimsContext.Create())
			{
				ShimDateTime.UtcNowGet = () => new DateTime(2016, 12, 31, 10, 20, 30);

				var baseVersion = new Version(1, 2, 3, 4);
				var result = CalculateSystemVersion<HumanReadable2SlotTimestampGenerator>(baseVersion);
				//retains 2 slots:
				Assert.AreEqual(baseVersion.Major, result.Major, "Major version mismatch, expected {0}, got {1}", baseVersion.Major, result.Major);
				Assert.AreEqual(baseVersion.Minor, result.Minor, "Minor version mismatch, expected {0}, got {1}", baseVersion.Minor, result.Minor);
				Assert.AreEqual(61231, result.Build, "Generated build number must match date");
				Assert.AreEqual(1020, result.Revision, "Generated revision number must match time");
			}
		}

		/// <summary>
		/// Tests that year part restarts correctly in 2017 and avoids exceeding Int16.Maxvalue.
		/// </summary>
		[TestMethod]
		[TestCategory("PreparedGenerators")]
		public void CheckHumanReadable2SlotTimeStampGenerator_For2017()
		{
			using (ShimsContext.Create())
			{
				ShimDateTime.UtcNowGet = () => new DateTime(2017, 1, 1);

				var baseVersion = new Version(1, 2, 3, 4);
				var result = CalculateSystemVersion<HumanReadable2SlotTimestampGenerator>(baseVersion);
				Assert.AreEqual(00101, result.Build, "Generated build number must match date");
			}
		}

		private static Version CalculateSystemVersion<TGenerator>(Version baseVersion)
			where TGenerator : SystemVersionGeneratorBase
		{
			var context = new VersionGenerationContext();
			context.BaseVersion = baseVersion;
			var generator = Activator.CreateInstance<TGenerator>();
			return generator.GenerateSystemVersion(context);
		}
	}
}
