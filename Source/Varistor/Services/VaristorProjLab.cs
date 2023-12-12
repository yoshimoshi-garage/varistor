using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.ICs.DigiPots;
using Meadow.Hardware;
using System;
using System.Threading;

namespace Varistor;

public class VaristorProjLab : IVaristorHardware, IInputService
{
    private IProjectLabHardware _projLab;
    private IRheostat[] _stats;
    private Timer _upTimer;
    private Timer _downTimer;

    public event EventHandler SmallIncreaseRequested;
    public event EventHandler LargeIncreaseRequested;
    public event EventHandler SmallDecreaseRequested;
    public event EventHandler LargeDecreaseRequested;

    private const int ButtonSlowTick = 800;
    private const int ButtonFastTick = 100;

    public VaristorProjLab()
    {
    }

    public void Initialize()
    {
        _projLab = ProjectLab.Create();

        _stats = new IRheostat[1];

        var stat = new Mcp4162(
            _projLab.MikroBus1.SpiBus,
            _projLab.MikroBus1.Pins.CS.CreateDigitalOutputPort(true),
            5000.Ohms()
            );

        _stats[0] = stat.Rheostats[0];

        _upTimer = new Timer(UpTimerElapsed);
        _projLab.RightButton!.PressStarted += RightButton_PressStarted;
        _projLab.RightButton!.PressEnded += RightButton_PressEnded;

        _downTimer = new Timer(DownTimerElapsed);
        _projLab.LeftButton!.PressStarted += LeftButton_PressStarted;
        _projLab.LeftButton!.PressEnded += LeftButton_PressEnded;
    }

    private void UpTimerElapsed(object o)
    {
        _upTimer.Change(ButtonFastTick, -1);
        SmallIncreaseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void RightButton_PressEnded(object sender, EventArgs e)
    {
        _upTimer.Change(-1, -1);
    }

    private void RightButton_PressStarted(object sender, EventArgs e)
    {
        _upTimer.Change(ButtonSlowTick, -1);
        SmallIncreaseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void DownTimerElapsed(object o)
    {
        _downTimer.Change(ButtonFastTick, -1);
        SmallDecreaseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void LeftButton_PressEnded(object sender, EventArgs e)
    {
        _downTimer.Change(-1, -1);
    }

    private void LeftButton_PressStarted(object sender, EventArgs e)
    {
        _downTimer.Change(ButtonSlowTick, -1);
        SmallDecreaseRequested?.Invoke(this, EventArgs.Empty);
    }

    public IRheostat[] GetRheostats()
    {
        return _stats;
    }

    public IGraphicsDisplay? GetDisplay()
    {
        return _projLab.Display;
    }
}
