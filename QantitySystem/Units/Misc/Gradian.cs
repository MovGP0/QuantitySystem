using QuantitySystem.Attributes;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Units.Misc
{
    [Unit("grad", typeof(Angle<>))]
    [ReferenceUnit(9, 10, UnitType = typeof(ArcDegree))]
    public sealed class Gradian : Unit
    {
    }
}