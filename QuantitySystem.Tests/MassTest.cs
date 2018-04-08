using NUnit.Framework;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
using QuantitySystem.Units.Metric.SI.BaseUnits;

namespace QuantitySystem.Tests
{
    /// <summary>
    ///This is a test class for MassTest and is intended
    ///to contain all MassTest Unit Tests
    ///</summary>
    [TestFixture]
    public class MassTest
    {
        [Test]
        public void MassConstructorTest()
        {
            var target = new Mass<float>();
            var massDimension = new QuantityDimension(1, 0, 0);

            Assert.That(target.Dimension, Is.EqualTo(massDimension));
        }

        [Test]
        public void MassAddTest()
        {
            Mass<double> l1 = 50;
            Mass<double> l2 = 50;
            var l = l1 + l2;

            Assert.That(l.Value, Is.EqualTo(100));
        }

        [Test]
        public void UnitMassAddTest()
        {
            var l1 = MetricUnit.Kilo<Gram>(2);
            var l2 = MetricUnit.None<Gram>(500);

            var l = l1 + l2;
            var lr = l2 + l1;

            Assert.Multiple(() =>
            {
                Assert.That(l.Value, Is.EqualTo(2.5));
                Assert.That(lr.Value, Is.EqualTo(2500));
            });
        }

        [Test]
        public void MassSubtractTest()
        {
            Mass<double> l1 = 150;
            Mass<double> l2 = 70;

            var l = l1 - l2;

            Assert.That(l.Value, Is.EqualTo(80));
        }

        [Test]
        public void MassMultiplyTest()
        {
            Mass<double> m1 = 20;
            Mass<double> m2 = 20;

            var m = m1 * m2;

            Assert.Multiple(() =>
            {
                Assert.That(m.Value, Is.EqualTo(400));
                Assert.That(m.Dimension, Is.EqualTo(new QuantityDimension(2, 0, 0)));
            });
        }

        [Test]
        public void MassDivideTest()
        {
            Mass<double> m1 = 20;
            Mass<double> m2 = 20;

            var m = m1 / m2;

            Assert.Multiple(() =>
            {
                Assert.That(m.Value, Is.EqualTo(1));
                Assert.That(m.Dimension, Is.EqualTo(new QuantityDimension()));
            });
        }

        [Test]
        public void MassComplexAddTest()
        {
            Mass<ComplexNumber> m1 = new ComplexNumber { Real = 1, Imaginary = 10 };
            Mass<ComplexNumber> m2 = new ComplexNumber { Real = 2, Imaginary = 4 };

            var m = m1 + m2;

            var expected = new ComplexNumber { Real = 3, Imaginary = 14 };

            Assert.That(m.Value, Is.EqualTo(expected));
        }

        [Test]
        public void MassComplexSubtractTest()
        {
            Mass<ComplexNumber> m1 = new ComplexNumber { Real = 1, Imaginary = 10 };
            Mass<ComplexNumber> m2 = new ComplexNumber { Real = 2, Imaginary = 4 };

            var m = m1 - m2;

            var expected = new ComplexNumber { Real = -1, Imaginary = 6 };

            Assert.That(m.Value, Is.EqualTo(expected));
        }

        [Test]
        public void MassComplexMultiplyTest()
        {
            Mass<ComplexNumber> m1 = new ComplexNumber { Real = 1, Imaginary = 10 };
            Mass<ComplexNumber> m2 = new ComplexNumber { Real = 2, Imaginary = 4 };

            var m = m1 * m2;

            var expected = new ComplexNumber { Real = 2, Imaginary = 40 };

            Assert.Multiple(() =>
            {
                Assert.That(m.Value, Is.EqualTo(expected));
                Assert.That(m.Dimension, Is.EqualTo(new QuantityDimension(2, 0, 0)));
            });
        }

        [Test]
        public void MassComplexDivideTest()
        {
            Mass<ComplexNumber> m1 = new ComplexNumber { Real = 1, Imaginary = 10 };
            Mass<ComplexNumber> m2 = new ComplexNumber { Real = 2, Imaginary = 5 };

            var m = m1 / m2;
            var expected = new ComplexNumber { Real = 0.5, Imaginary = 2 };

            Assert.Multiple(() =>
            {
                Assert.That(m.Value, Is.EqualTo(expected));
                Assert.That(m.Dimension, Is.EqualTo(new QuantityDimension()));
            });
        }
    }
}
