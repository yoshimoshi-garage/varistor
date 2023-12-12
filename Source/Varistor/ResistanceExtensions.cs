using Meadow.Units;

namespace Varistor;

public static class ResistanceExtensions
{
    public static Resistance Ohms(this int v)
    {
        return new Resistance(v, Resistance.UnitType.Ohms);
    }

    public static Resistance Ohms(this double v)
    {
        return new Resistance(v, Resistance.UnitType.Ohms);
    }
}
