using NUnit.Framework;

namespace QuantitySystem.Tests
{
    /// <summary>
    ///This is a test class for QuantityDimensionTest and is intended
    ///to contain all QuantityDimensionTest Unit Tests
    ///</summary>
    [TestFixture]
    public class QuantityDimensionTest
    {
        /// <summary>
        ///A test for QuantityDimension Constructor
        ///</summary>
        [Test]
        public void QuantityDimensionConstructorTest1()
        {
            var target = new QuantityDimension();

            Assert.Multiple(() =>
            {
                Assert.That(target.Mass.Exponent, Is.Zero);
                Assert.That(target.Length.Exponent, Is.Zero);
                Assert.That(target.Time.Exponent, Is.Zero);
                Assert.That(target.Temperature.Exponent, Is.Zero);
                Assert.That(target.LuminousIntensity.Exponent, Is.Zero);
                Assert.That(target.ElectricCurrent.Exponent, Is.Zero);
                Assert.That(target.AmountOfSubstance.Exponent, Is.Zero);
            });
        }

        /// <summary>
        ///A test for QuantityDimension Constructor
        ///</summary>
        [Test]
        public void QuantityDimensionConstructorTest()
        {
            const int mass = 1;
            const int length = 2;
            const int time = 3;
            var target = new QuantityDimension(mass, length, time);

            Assert.Multiple(() =>
            {
                Assert.That(target.Mass.Exponent, Is.EqualTo(1));
                Assert.That(target.Length.Exponent, Is.EqualTo(2));
                Assert.That(target.Time.Exponent, Is.EqualTo(3));
                Assert.That(target.Temperature.Exponent, Is.EqualTo(0));
                Assert.That(target.LuminousIntensity.Exponent, Is.EqualTo(0));
                Assert.That(target.ElectricCurrent.Exponent, Is.EqualTo(0));
                Assert.That(target.AmountOfSubstance.Exponent, Is.EqualTo(0));
            });
        }
    }
}
