using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nortal.Utilities.AssemblyVersioning;
using Nortal.Utilities.AssemblyVersioning.Generators;

namespace UnitTests
{
	[TestClass]
	public class GeneratorResolverTests
	{
		[TestMethod]
		[TestCategory("Resolver")]
		public void CheckGeneratorsFound()
		{
			var generators = GeneratorResolver.FindAllGeneratorTypes().ToArray();
			Assert.IsTrue(generators.Length > 0, "Found {0} generators.", generators.Length);
		}

		[TestMethod]
		[TestCategory("Resolver")]
		public void CheckResolveByName_TypeName()
		{
			Type testType = typeof(SkipVersionGenerator);
			var context = new VersionGenerationContext();
			var generator = GeneratorResolver.ResolveWithArgument(testType.Name, context);

			Assert.IsNotNull(generator);
			Assert.IsInstanceOfType(generator, testType, "Resolved type does not match: {0}", generator.GetType().Name);
		}

		[TestMethod]
		[TestCategory("Resolver")]
		public void CheckResolveByName_Generator()
		{
			const String testShortName = "Skip";
			var context = new VersionGenerationContext();
			var generator = GeneratorResolver.ResolveWithArgument(testShortName, context);

			Assert.IsNotNull(generator);
			Assert.IsInstanceOfType(generator, typeof(SkipVersionGenerator), "Resolved type does not match: {0}", generator.GetType().Name);
		}

		[TestMethod]
		[TestCategory("Resolver")]
		public void CheckResolveByName_Shortest()
		{
			const String testShortName = "Skip";
			var context = new VersionGenerationContext();
			var generator = GeneratorResolver.ResolveWithArgument(testShortName, context);

			Assert.IsNotNull(generator);
			Assert.IsInstanceOfType(generator, typeof(SkipVersionGenerator), "Resolved type does not match: {0}", generator.GetType().Name);
		}

		[TestMethod]
		[TestCategory("Resolver")]
		public void CheckResolveByName_DifferentCasing()
		{
			const String testShortName = "sKiP";
			var context = new VersionGenerationContext();
			var generator = GeneratorResolver.ResolveWithArgument(testShortName, context);

			Assert.IsNotNull(generator);
			Assert.IsInstanceOfType(generator, typeof(SkipVersionGenerator), "Resolved type does not match: {0}", generator.GetType().Name);
		}

		[TestMethod]
		[TestCategory("Resolver")]
		public void CheckResolveByName_GeneratorShortNameNoArgument()
		{
			const String name = "HumanReadable2SlotTimestamp";
			var context = new VersionGenerationContext();
			var generator = GeneratorResolver.ResolveWithArgument(name, context);

			Assert.IsNotNull(generator);
			Assert.IsInstanceOfType(generator, typeof(HumanReadable2SlotTimestampGenerator), "Resolved type does not match: {0}", generator.GetType().Name);
		}

		[TestMethod]
		[TestCategory("Resolver")]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CheckResolvingUsingInvalidGeneratorName()
		{
			var context = new VersionGenerationContext();
			var generator = GeneratorResolver.ResolveWithArgument("NosuchgeneratorExists:argument", context);
			
			//asserting exception
		}

		[TestMethod]
		[TestCategory("Resolver")]
		public void CheckResolvingUsingCustomGeneratorWithArgument()
		{
			var context = new VersionGenerationContext();
			var generator = GeneratorResolver.ResolveWithArgument("Custom:My argument with colon : here", context);

			Assert.IsInstanceOfType(generator, typeof(CustomVersionGenerator), "Resolved type does not match: {0}", generator.GetType().Name);
			Assert.AreEqual(context.VersionGenerationArgument, "My argument with colon : here");

		}

		[TestMethod]
		[TestCategory("Resolver")]
		public void CheckResolvingUsingCustomGeneratorArgumentOnly()
		{
			var context = new VersionGenerationContext();
			var generator = GeneratorResolver.ResolveWithArgument("My template here {Now:someformat}", context);

			Assert.IsInstanceOfType(generator, typeof(CustomVersionGenerator), "Resolved type does not match: {0}", generator.GetType().Name);
			Assert.AreEqual(context.VersionGenerationArgument, "My template here {Now:someformat}");
		}
	}
}
