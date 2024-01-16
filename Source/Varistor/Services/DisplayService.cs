using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Hardware;

namespace VaristorDisplay;

public class DisplayService : IDisplayService
{
    private int _selectedIndex = 0;
    private DisplayScreen _screen;
    private Layout _rheostatLayout;
    private Label _headerLabel;
    private RheostatControl[] _statControls;

    private Color _selectedColor;

    public DisplayService(IGraphicsDisplay display, int statCount)
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

        _statControls = new RheostatControl[statCount];

        for (var i = 0; i < statCount; i++)
        {
            var stat = new RheostatControl(_screen, 0, _rheostatLayout.Top + 30 + (40 * i), _screen.Width, 30, theme.Font)
            {
                IsFocused = i == 0
            };

            _statControls[i] = stat;

            _rheostatLayout.Controls.Add(stat);
        }

        _rheostatLayout.Controls.Add(_headerLabel);

        _screen.Controls.Add(_rheostatLayout);
    }

    public void SelectRheostat(int index)
    {
        _statControls[_selectedIndex].IsFocused = false;
        _selectedIndex = index;
        _statControls[_selectedIndex].IsFocused = true;
    }

    public void Update()
    {
        _screen.Invalidate();
    }

    public void UpdateRheostat(int index, IRheostat rheostat)
    {
        // this is the label
        _statControls[index].DisplayValue = rheostat.Resistance.Ohms;
        // this is the progressbar percentage
        _statControls[index].Value = (int)((rheostat.Resistance.Ohms / (float)rheostat.MaxResistance.Ohms) * 100);
    }
}
