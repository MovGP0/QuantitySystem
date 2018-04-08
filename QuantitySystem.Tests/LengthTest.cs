using NUnit.Framework;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Tests
{
    /// <summary>
    ///This is a test class for LengthTest and is intended
    ///to contain all LengthTest Unit Tests
    ///</summary>
    [TestFixture]
    public class LengthTest
    {
        [Test]
        public void DimensionTest()
        {
            var target = new Length<double>();
            var lengthDimension = new QuantityDimension(0, 1, 0);
            Assert.That(target.Dimension, Is.EqualTo(lengthDimension));
        }
    }
}
