using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.Metric.MTS
{
    [MetricUnit("sn", typeof(Force<>), true)]
    [ReferenceUnit(1000)]
    public sealed class Sthène : MetricUnit
    {
    }
}