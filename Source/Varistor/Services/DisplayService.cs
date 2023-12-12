using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Hardware;

namespace Varistor;

public class DisplayService : IDisplayService
{
    private DisplayScreen _screen;
    private Layout _rheostatLayout;
    private Label _headerLabel;
    private RheostatControl[] _statControls;

    public DisplayService(IGraphicsDisplay display)
    {
        var theme = new DisplayTheme
        {
            TextColor = Color.White,
            Font = new Font12x20()
        };

        _screen = new DisplayScreen(
            display,
            RotationType._270Degrees,
            theme: theme);

        _rheostatLayout = new AbsoluteLayout(_screen);

        _headerLabel = new Label(0, 1, _screen.Width, theme.Font.Height + 2)
        {
            Text = "VARISTOR",
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        var stat = new RheostatControl(_screen, 0, _rheostatLayout.Top + 30, _screen.Width, 30, theme.Font);

        _statControls = new RheostatControl[1];
        _statControls[0] = stat;

        _rheostatLayout.Controls.Add(stat);

        _rheostatLayout.Controls.Add(_headerLabel);

        _screen.Controls.Add(_rheostatLayout);
    }

    public void Update()

    {
        _screen.Invalidate();
    }

    public void Initialize(int rheostatCount)
    {
        // TODO: create as many controls as we have rheostats
    }

    public void UpdateRheostat(int index, IRheostat rheostat)
    {
        // this is the label
        _statControls[index].DisplayValue = rheostat.Resistance.Ohms;
        // this is the progressbar percentage
        _statControls[index].Value = (int)((rheostat.Resistance.Ohms / (float)rheostat.MaxResistance.Ohms) * 100);
    }
}
