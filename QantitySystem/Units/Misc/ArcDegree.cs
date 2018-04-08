using System;
using QuantitySystem.Attributes;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Units.Misc
{
    [DefaultUnit("deg", typeof(Angle<>))]
    [ReferenceUnit(Math.PI, 180)]
    public sealed class ArcDegree : Unit
    {
    }
}