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
		public void CheckGeneratorsFound()
		{
			var generators = GeneratorResolver.FindAllGeneratorTypes().ToArray();
			Assert.IsTrue(generators.Length > 0, "Found {0} generators.", generators.Length);
		}

		[TestMethod]
		public void CheckResolveByTypeName()
		{
			Type testType = typeof(SkipVersionGenerator);
			var generator = GeneratorResolver.ResolveByName(testType.Name);

			Assert.IsNotNull(generator);
			Assert.IsInstanceOfType(generator, testType, "Resolved type does not match: {0}", generator.GetType().Name);
		}

		[TestMethod]
		public void CheckResolveByShortName()
		{
			String testShortName = "SkipVersion";
			var generator = GeneratorResolver.ResolveByName(testShortName);

			Assert.IsNotNull(generator);
			Assert.IsInstanceOfType(generator, typeof(SkipVersionGenerator), "Resolved type does not match: {0}", generator.GetType().Name);
		}

		[TestMethod]
		public void CheckResolveByDifferentlyCasedName()
		{
			String testShortName = "skipversion";
			var generator = GeneratorResolver.ResolveByName(testShortName);

			Assert.IsNotNull(generator);
			Assert.IsInstanceOfType(generator, typeof(SkipVersionGenerator), "Resolved type does not match: {0}", generator.GetType().Name);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CheckResolvingUsingInvalidName()
		{
			var generator = GeneratorResolver.ResolveByName("DummyNoSuchClassExistsType");
			//asserting exception
		}
	}
}
