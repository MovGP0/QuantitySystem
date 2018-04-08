using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Currency
{
    [Unit("USD", typeof(Currency<>))]
    [ReferenceUnit(1, UnitType = typeof(Coin))]
    public sealed class UnitedStatesDollar : Unit
    {
    }
}