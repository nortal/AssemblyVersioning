using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nortal.Utilities.AssemblyVersioning;
using Nortal.Utilities.AssemblyVersioning.Generators;

namespace UnitTests
{
	[TestClass]
	public class GeneratorTests
	{

		[TestMethod]
		public void CheckSkipVersionGeneratorReturnsNull()
		{
			var generator = new SkipVersionGenerator();
			var context = new VersionGenerationContext();
			var version = generator.GenerateSystemVersion(context);
			Assert.IsNull(version, "Expected null, got: {0}.", version);
		}

		[TestMethod]
		public void CheckHumanReadable1SlotTimeStampGenerator()
		{
			var baseVersion = new Version(1, 2, 3, 0);
			var result = CalculateSystemVersion<HumanReadable1SlotTimestampGenerator>(baseVersion);

			//retains 3 slots:
			Assert.AreEqual(baseVersion.Major, result.Major, "Major version mismatch, expected {0}, got {1}", baseVersion.Major, result.Major);
			Assert.AreEqual(baseVersion.Minor, result.Minor, "Minor version mismatch, expected {0}, got {1}", baseVersion.Minor, result.Minor);
			Assert.AreEqual(baseVersion.Build, result.Build, "Build version mismatch, expected {0}, got {1}", baseVersion.Build, result.Build);
			Assert.IsTrue(result.Revision > 0, "Generated build number must be >0");
		}

		[TestMethod]
		public void CheckHumanReadable2SlotTimeStampGenerator()
		{
			var baseVersion = new Version(1, 2, 0, 0);
			var result = CalculateSystemVersion<HumanReadable2SlotTimestampGenerator>(baseVersion);
			//retains 2 slots:
			Assert.AreEqual(baseVersion.Major, result.Major, "Major version mismatch, expected {0}, got {1}", baseVersion.Major, result.Major);
			Assert.AreEqual(baseVersion.Minor, result.Minor, "Minor version mismatch, expected {0}, got {1}", baseVersion.Minor, result.Minor);
			Assert.IsTrue(result.Build > 0, "Generated build number must be >0");
			Assert.IsTrue(result.Revision > 0, "Generated build number must be >0");
		}


		private Version CalculateSystemVersion<TGenerator>(Version baseVersion)
			where TGenerator: SystemVersionGeneratorBase
		{
			var context = new VersionGenerationContext();
			context.BaseVersion = new Version(1, 2, 3, 4);
			var generator = Activator.CreateInstance<TGenerator>();
			return generator.GenerateSystemVersion(context);
		}
	}
}
