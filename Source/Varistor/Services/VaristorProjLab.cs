using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Linq;
using System.Threading;

namespace VaristorDisplay;

public class VaristorProjLab : IVaristorHardware, IInputService
{
    private IProjectLabHardware _projLab;
    private Varistor _varistor;
    private Timer _upTimer;
    private Timer _downTimer;

    public event EventHandler IncreaseRequested;
    public event EventHandler DecreaseRequested;
    public event EventHandler PreviousStatRequested;
    public event EventHandler NextStatRequested;

    private const int ButtonSlowTick = 800;
    private const int ButtonFastTick = 100;

    public VaristorProjLab()
    {
    }

    public void Initialize()
    {
        _projLab = ProjectLab.Create();

        _varistor = new Varistor(
            Varistor.Chip.Mcp4142,
            5000.Ohms(),
            _projLab.MikroBus1.SpiBus,
            _projLab.MikroBus1.Pins.CS.CreateDigitalOutputPort(true),
            _projLab.MikroBus1.Pins.RST.CreateDigitalOutputPort(true));

        _upTimer = new Timer(UpTimerElapsed);
        _projLab.RightButton!.PressStarted += RightButton_PressStarted;
        _projLab.RightButton!.PressEnded += RightButton_PressEnded;

        _downTimer = new Timer(DownTimerElapsed);
        _projLab.LeftButton!.PressStarted += LeftButton_PressStarted;
        _projLab.LeftButton!.PressEnded += LeftButton_PressEnded;

        _projLab.DownButton!.Clicked += DownButton_Clicked;
        _projLab.UpButton!.Clicked += UpButton_Clicked;
    }

    private void UpButton_Clicked(object sender, EventArgs e)
    {
        PreviousStatRequested?.Invoke(this, EventArgs.Empty);
    }

    private void DownButton_Clicked(object sender, EventArgs e)
    {
        NextStatRequested?.Invoke(this, EventArgs.Empty);
    }

    private void UpTimerElapsed(object o)
    {
        _upTimer.Change(ButtonFastTick, -1);
        IncreaseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void RightButton_PressEnded(object sender, EventArgs e)
    {
        _upTimer.Change(-1, -1);
    }

    private void RightButton_PressStarted(object sender, EventArgs e)
    {
        _upTimer.Change(ButtonSlowTick, -1);
        IncreaseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void DownTimerElapsed(object o)
    {
        _downTimer.Change(ButtonFastTick, -1);
        DecreaseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void LeftButton_PressEnded(object sender, EventArgs e)
    {
        _downTimer.Change(-1, -1);
    }

    private void LeftButton_PressStarted(object sender, EventArgs e)
    {
        _downTimer.Change(ButtonSlowTick, -1);
        DecreaseRequested?.Invoke(this, EventArgs.Empty);
    }

    public IRheostat[] GetRheostats()
    {
        return _varistor.ToArray();
    }

    public IGraphicsDisplay? GetDisplay()
    {
        return _projLab.Display;
    }
}
