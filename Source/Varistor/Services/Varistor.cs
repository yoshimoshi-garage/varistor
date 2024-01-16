using Meadow.Foundation.ICs.DigiPots;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Collections;
using System.Collections.Generic;

namespace VaristorDisplay;

public class Varistor : IEnumerable<IRheostat>
{
    private readonly List<IRheostat> _stats;

    public enum Chip
    {
        Mcp4132,
        Mcp4142,
        Mcp4152,
        Mcp4162
    }

    public IRheostat this[int index] => _stats[index];

    public Varistor(Chip chip, Resistance maxResistance, ISpiBus bus, params IDigitalOutputPort[] chipSelects)
    {
        _stats = new();

        for (var i = 0; i < chipSelects.Length; i++)
        {
            Mcp4xx2 part = chip switch
            {
                Chip.Mcp4132 => new Mcp4132(
                                        bus,
                                        chipSelects[i],
                                        maxResistance
                                        ),
                Chip.Mcp4142 => new Mcp4142(
                                        bus,
                                        chipSelects[i],
                                        maxResistance
                                        ),
                Chip.Mcp4152 => new Mcp4152(
                                        bus,
                                        chipSelects[i],
                                        maxResistance
                                        ),
                Chip.Mcp4162 => new Mcp4162(
                                        bus,
                                        chipSelects[i],
                                        maxResistance
                                        ),
                _ => throw new NotSupportedException(),
            };
            _stats.Add(part.Rheostats[0]);
        }
    }

    public IEnumerator<IRheostat> GetEnumerator()
    {
        return _stats.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
