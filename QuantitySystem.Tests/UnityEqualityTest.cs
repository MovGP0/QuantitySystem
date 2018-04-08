using System.Collections.Generic;
using NUnit.Framework;
using QuantitySystem.Units;
using QuantitySystem.Units.English;
using QuantitySystem.Units.Metric.SI.BaseUnits;

namespace QuantitySystem.Tests
{
    [TestFixture]
    public class UnityEqualityTest
    {
        [Test]
        public void ListTest()
        {
            Unit millimetre = new Metre { UnitPrefix = MetricPrefix.Milli };
            Unit centimetre = new Metre { UnitPrefix = MetricPrefix.Centi };
            Unit decimetre = new Metre { UnitPrefix = MetricPrefix.Deci };
            Unit inch = new Inch();
            var units = new List<Unit> { millimetre, centimetre, decimetre, inch };

            //Theese should work, of course
            Assert.Multiple(() =>
            {
                Assert.IsTrue(units.Contains(millimetre));
                Assert.IsTrue(units.Contains(centimetre));
                Assert.IsTrue(units.Contains(decimetre));
                Assert.IsTrue(units.Contains(inch));
            });

            Unit newMillimetre = new Metre { UnitPrefix = MetricPrefix.Milli };
            Unit newInch = new Inch();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(units.Contains(newMillimetre));
                Assert.IsTrue(units.Contains(newInch));
                Assert.IsTrue(units.Contains(newMillimetre));
                Assert.IsTrue(units.Contains(newInch));
            });
        }

        [Test]
        public void DictionaryTest()
        {
            Unit millimetre = new Metre { UnitPrefix = MetricPrefix.Milli };
            Unit centimetre = new Metre { UnitPrefix = MetricPrefix.Centi };
            Unit decimetre = new Metre { UnitPrefix = MetricPrefix.Deci };
            Unit inch = new Inch();
            var units = new Dictionary<Unit, string>
            {
                {millimetre, millimetre.Symbol},
                {centimetre, centimetre.Symbol},
                {decimetre, decimetre.Symbol},
                {inch, inch.Symbol}
            };

            Assert.Multiple(() =>
            {
                Assert.IsTrue(units.ContainsKey(millimetre));
                Assert.IsTrue(units.ContainsKey(centimetre));
                Assert.IsTrue(units.ContainsKey(decimetre));
                Assert.IsTrue(units.ContainsKey(inch));
            });

            Unit newMillimetre = new Metre { UnitPrefix = MetricPrefix.Milli };
            Unit newInch = new Inch();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(units.ContainsKey(newMillimetre));
                Assert.IsTrue(units.ContainsKey(newInch));
                Assert.IsTrue(units.ContainsKey(newMillimetre));
                Assert.IsTrue(units.ContainsKey(newInch));
                Assert.IsTrue(units[newMillimetre] == millimetre.Symbol);
                Assert.IsTrue(units[newInch] == inch.Symbol);
            });
        }

        private static bool AreNotEqual(Unit first, Unit other)
        {
            return first != other;
        }

        [Test]
        public void MoreEqualityTestsTest()
        {
            Unit millimetre = new Metre { UnitPrefix = MetricPrefix.Milli };
            Unit centimetre = new Metre { UnitPrefix = MetricPrefix.Centi };
            Unit decimetre = new Metre { UnitPrefix = MetricPrefix.Deci };
            Unit inch = new Inch();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(millimetre != centimetre);
                Assert.IsTrue(AreNotEqual(millimetre, centimetre));
            });
        }
    }
}
