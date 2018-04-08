using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Metric.SI.BaseUnits
{
    [MetricUnit("g", typeof(Mass<>), SiPrefix =  MetricPrefixes.Kilo)]
    public sealed class Gram : MetricUnit
    {
    }
}
