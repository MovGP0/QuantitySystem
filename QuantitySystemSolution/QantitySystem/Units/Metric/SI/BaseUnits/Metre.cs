using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Metric.SI.BaseUnits
{
    [MetricUnit("m", typeof(Length<>), CgsPrefix = MetricPrefixes.Centi)]
    public sealed class Metre : MetricUnit
    {

    }
}
