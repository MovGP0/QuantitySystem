using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Metric.MTS
{
    [MetricUnit("mt", typeof(Mass<>), true)]
    [ReferenceUnit(1000)]
    public sealed class MetricTonne : MetricUnit
    {
    }
}