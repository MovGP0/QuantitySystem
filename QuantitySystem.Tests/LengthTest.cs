using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Tests
{
    /// <summary>
    ///This is a test class for LengthTest and is intended
    ///to contain all LengthTest Unit Tests
    ///</summary>
    [TestClass]
    public class LengthTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for Dimension
        ///</summary>
        public void DimensionTestHelper<T>()
        {
            var target = new Length<T>();
            var lengthDimension = new QuantityDimension(0, 1, 0);
            Assert.AreEqual(lengthDimension, target.Dimension);
        }

        [TestMethod]
        public void DimensionTest()
        {
            DimensionTestHelper<GenericParameterHelper>();
        }
    }
}
