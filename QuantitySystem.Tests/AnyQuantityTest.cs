using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantitySystem.Quantities;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Tests
{
    /// <summary>
    ///This is a test class for AnyQuantityTest and is intended
    ///to contain all AnyQuantityTest Unit Tests
    ///</summary>
    [TestClass]
    public class AnyQuantityTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void VelocityQuantityTest()
        {
            Length<double> l = 20;
            Time<double> t = 10;

            var q = l / t;

            var expected = new Velocity<double> { Value = 2 };

            Assert.AreEqual(expected, q);
            Assert.AreEqual(expected.Value, q.Value);
        }

        [TestMethod]
        public void AccelerationQuantityTest()
        {
            Velocity<double> l = 20;
            Time<double> t = 10;

            var q = l / t;

            var expected = new Acceleration<double> { Value = 2 };

            Assert.AreEqual(expected, q);
            Assert.AreEqual(expected.Value, q.Value);
        }

        [TestMethod]
        public void ForceQuantityTest()
        {
            Acceleration<double> a = 20;
            Mass<double> m = 10;

            var q = m * a;

            var expected = new Force<double> { Value = 200 };

            Assert.AreEqual(expected, q);
            Assert.AreEqual(expected.Value, q.Value);
        }

        [TestMethod]
        public void PressureQuantityTest()
        {
            Force<double> f = 20;
            Area<double> a = 10;

            var q = f / a;

            var expected = new Pressure<double> { Value = 2 };

            Assert.AreEqual(expected, q);
            Assert.AreEqual(expected.Value, q.Value);
        }

        [TestMethod]
        public void EnergyQuantityTest()
        {
            Force<double> f = 20;
            Length<double> l = 10;

            var q = f * l;

            var expected = new Energy<double> { Value = 200 };

            Assert.AreEqual(expected, q);
            Assert.AreEqual(expected.Value, q.Value);
        }

        [TestMethod]
        public void PowerQuantityTest()
        {
            Energy<double> e = 20;
            Time<double> t = 10;

            var q = e / t;

            var expected = new Power<double> { Value = 2 };

            Assert.AreEqual(expected, q);
            Assert.AreEqual(expected.Value, q.Value);
        }

        /// <summary>
        /// Testing to reach power after several calculation
        /// </summary>
        [TestMethod]
        public void PowerIncrementalQuantityTest()
        {
            Length<double> l = 100;
            Time<double> t = 2;

            var v = l / t; // 50    
            var a = v / t; // 25 m/s^2

            Mass<double> m = 10;

            var f = m * a; // 250 Newton
            var work = f * l; // 25000 joule
            var power = work / t; // 12500 watt

            var expected = new Power<double> { Value = 12500 };

            Assert.AreEqual(expected, power);
            Assert.AreEqual(expected.Value, power.Value);
        }

        /// <summary>
        /// Testing to reach power but in one line calculation.
        /// </summary>
        [TestMethod]
        public void PowerInOneLineQuantityTest()
        {
            Length<double> l = 100;
            Time<double> t = 2;
            Mass<double> m = 10;

            var power = m * (l / t / t) * l / t;

            var expected = new Power<double> { Value = 12500 };

            Assert.AreEqual(expected, power);
            Assert.AreEqual(expected.Value, power.Value);
        }
    }
}
