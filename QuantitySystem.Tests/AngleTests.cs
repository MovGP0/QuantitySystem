using QuantitySystem.Quantities.DimensionlessQuantities;
using NUnit.Framework;

namespace QuantitySystem.Tests
{
    /// <summary>
    ///This is a test class for AngleTest and is intended
    ///to contain all AngleTest Unit Tests
    ///</summary>
    [TestFixture]
    public sealed class AngleTests
    {
        [Test]
        public void AngleConstructorTest()
        {
            var target = new Angle<float>();

            var angleDimension = new QuantityDimension
            {
                Length = new DimensionDescriptors.LengthDescriptor(1, -1)
            };

            Assert.That(angleDimension, Is.EqualTo(target.Dimension));
        }
    }
}
