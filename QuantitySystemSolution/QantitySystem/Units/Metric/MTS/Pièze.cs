using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.Metric.MTS
{
    [MetricUnit("pz", typeof(Pressure<>), true)]
    [ReferenceUnit(1000)]
    public sealed class Pièze : MetricUnit
    {
    }
}