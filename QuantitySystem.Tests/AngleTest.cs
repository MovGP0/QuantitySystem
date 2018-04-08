using QuantitySystem.Quantities.DimensionlessQuantities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuantitySystem.Tests
{
    /// <summary>
    ///This is a test class for AngleTest and is intended
    ///to contain all AngleTest Unit Tests
    ///</summary>
    [TestClass]
    public class AngleTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for Angle`1 Constructor
        ///</summary>
        public void AngleConstructorTestHelper<T>()
        {
            var target = new Angle<T>();
            var angleDimension = new QuantityDimension
            {
                Length = new DimensionDescriptors.LengthDescriptor(1, -1)
            };

            Assert.AreEqual(angleDimension, target.Dimension);
        }

        [TestMethod]
        public void AngleConstructorTest()
        {
            AngleConstructorTestHelper<GenericParameterHelper>();
        }
    }
}
