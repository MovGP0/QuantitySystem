using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Currency
{
    [DefaultUnit("¤", typeof(Currency<>))]
    public sealed class Coin : Unit  
    {
    }
}
