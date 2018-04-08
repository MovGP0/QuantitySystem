using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuantitySystem.Tests
{
    /// <summary>
    ///This is a test class for QuantityDimensionTest and is intended
    ///to contain all QuantityDimensionTest Unit Tests
    ///</summary>
    [TestClass]
    public class QuantityDimensionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for QuantityDimension Constructor
        ///</summary>
        [TestMethod]
        public void QuantityDimensionConstructorTest1()
        {
            var target = new QuantityDimension();

            Assert.AreEqual(0, target.Mass.Exponent);
            Assert.AreEqual(0, target.Length.Exponent);
            Assert.AreEqual(0, target.Time.Exponent);
            Assert.AreEqual(0, target.Temperature.Exponent);
            Assert.AreEqual(0, target.LuminousIntensity.Exponent);
            Assert.AreEqual(0, target.ElectricCurrent.Exponent);
            Assert.AreEqual(0, target.AmountOfSubstance.Exponent);
        }

        /// <summary>
        ///A test for QuantityDimension Constructor
        ///</summary>
        [TestMethod]
        public void QuantityDimensionConstructorTest()
        {
            const int mass = 1;
            const int length = 2;
            const int time = 3;
            var target = new QuantityDimension(mass, length, time);

            Assert.AreEqual(1, target.Mass.Exponent);
            Assert.AreEqual(2, target.Length.Exponent);
            Assert.AreEqual(3, target.Time.Exponent);
            Assert.AreEqual(0, target.Temperature.Exponent);
            Assert.AreEqual(0, target.LuminousIntensity.Exponent);
            Assert.AreEqual(0, target.ElectricCurrent.Exponent);
            Assert.AreEqual(0, target.AmountOfSubstance.Exponent);
        }
    }
}
