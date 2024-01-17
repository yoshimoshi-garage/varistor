using Meadow.Hardware;
using Meadow.Units;
using System;

namespace VaristorDisplay;

public class AppLogic
{
    private readonly Resistance StepAmount = 50.0d.Ohms();

    private int _selectedIndex = 0;


    private IVaristorHardware Hardware { get; }
    private IDisplayService Display { get; }
    private IInputService Inputs { get; }
    private IRheostat SeletedRheostat => Hardware.GetRheostats()[_selectedIndex];

    public AppLogic(IVaristorHardware hardware, IDisplayService display, IInputService inputService)
    {
        Hardware = hardware;
        Display = display;
        Inputs = inputService;

        var stats = Hardware.GetRheostats();

        Display.UpdateRheostat(0, stats[0]);
        Display.UpdateRheostat(1, stats[1]);

        Inputs.IncreaseRequested += OnIncreaseRequested;
        Inputs.DecreaseRequested += OnDecreaseRequested;
        Inputs.PreviousStatRequested += OnPreviousStatRequested;
        Inputs.NextStatRequested += OnNextStatRequested;
    }

    private void OnNextStatRequested(object sender, EventArgs e)
    {
        if (_selectedIndex == Hardware.GetRheostats().Length - 1) return;
        _selectedIndex++;
        Display.SelectRheostat(_selectedIndex);
    }

    private void OnPreviousStatRequested(object sender, EventArgs e)
    {
        if (_selectedIndex == 0) return;
        _selectedIndex--;
        Display.SelectRheostat(_selectedIndex);
    }

    private void OnDecreaseRequested(object sender, EventArgs e)
    {
        if (SeletedRheostat.Resistance <= Resistance.Zero) return;

        var current = SeletedRheostat.Resistance;
        var target = current - StepAmount;
        if (target < Resistance.Zero)
        {
            target = Resistance.Zero;
        }

        SeletedRheostat.Resistance = target;

        Display.UpdateRheostat(_selectedIndex, SeletedRheostat);
    }

    private void OnIncreaseRequested(object sender, EventArgs e)
    {
        if (SeletedRheostat.Resistance >= SeletedRheostat.MaxResistance) return;

        var current = SeletedRheostat.Resistance;
        var target = current + StepAmount;
        if (target > SeletedRheostat.MaxResistance)
        {
            target = SeletedRheostat.MaxResistance;
        }

        SeletedRheostat.Resistance = target;

        Display.UpdateRheostat(_selectedIndex, SeletedRheostat);
    }
}
