using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantitySystem.DimensionDescriptors;

namespace QuantitySystem.Tests
{
    /// <summary>
    ///This is a test class for LengthDescriptorTest and is intended
    ///to contain all LengthDescriptorTest Unit Tests
    ///</summary>
    [TestClass]
    public class LengthDescriptorTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for LengthDescriptor Constructor
        ///</summary>
        [TestMethod]
        public void LengthDescriptorConstructorTest()
        {
            const float normalExponent = 1;
            const float radiusExponent = -1;

            var target = new LengthDescriptor(normalExponent, radiusExponent);

            Assert.AreEqual(0, target.Exponent);
        }

        /// <summary>
        ///A test for Subtract
        ///</summary>
        [TestMethod]
        public void LengthDescriptorSubtractTest()
        {
            var target = new LengthDescriptor(3,2);
            var dimensionDescriptor = new LengthDescriptor(1,1);

            var expected = new LengthDescriptor(2,1);

            var actual = target.Subtract(dimensionDescriptor);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Multiply
        ///</summary>
        [TestMethod]
        public void LengthDescriptorMultiplyTest()
        {
            var target = new LengthDescriptor(3,2);
            const int exponent = 2;
            var expected = new LengthDescriptor(6,4);
            LengthDescriptor actual;
            actual = target.Multiply(exponent);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod]
        public void LengthDescriptorAddTest()
        {
            var target = new LengthDescriptor(3,2);
            var dimensionDescriptor = new LengthDescriptor(1,1);
            var expected = new LengthDescriptor(4,3);
            var actual = target.Add(dimensionDescriptor);

            Assert.AreEqual(expected, actual);
        }
    }
}
