using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Linq;

namespace Varistor;

public class AppLogic
{
    private readonly Resistance StepAmount = 50.0d.Ohms();

    private IVaristorHardware Hardware { get; }
    private IDisplayService Display { get; }
    private IInputService Inputs { get; }
    private IRheostat SeletedRheostat { get; set; }

    public AppLogic(IVaristorHardware hardware, IDisplayService display, IInputService inputService)
    {
        Hardware = hardware;
        Display = display;
        Inputs = inputService;

        // TODO: support multiple rheostats in the HMI
        SeletedRheostat = Hardware.GetRheostats().First();

        Display.UpdateRheostat(0, SeletedRheostat);

        Inputs.SmallIncreaseRequested += OnSmallIncreaseRequested;
        Inputs.SmallDecreaseRequested += OnSmallDecreaseRequested;
    }

    private void OnSmallDecreaseRequested(object sender, EventArgs e)
    {
        if (SeletedRheostat.Resistance <= Resistance.Zero) return;

        var current = SeletedRheostat.Resistance;
        var target = current - StepAmount;
        if (target < Resistance.Zero)
        {
            target = Resistance.Zero;
        }

        SeletedRheostat.Resistance = target;

        Display.UpdateRheostat(0, SeletedRheostat);
    }

    private void OnSmallIncreaseRequested(object sender, EventArgs e)
    {
        if (SeletedRheostat.Resistance >= SeletedRheostat.MaxResistance) return;

        var current = SeletedRheostat.Resistance;
        var target = current + StepAmount;
        if (target > SeletedRheostat.MaxResistance)
        {
            target = SeletedRheostat.MaxResistance;
        }

        SeletedRheostat.Resistance = target;

        Display.UpdateRheostat(0, SeletedRheostat);
    }
}
