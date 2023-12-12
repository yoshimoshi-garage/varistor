using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;

namespace Varistor;

internal class RheostatControl : AbsoluteLayout
{
    public const int LabelWidth = 100;
    public const int Margin = 2;

    private IFont _font;
    private ProgressBar _progressBar;
    private Label _value;
    private bool _focused = true;
    private double? _displayValue;

    public Color FocusedColor => Color.DarkOliveGreen;
    public Color ValueColor => Color.OrangeRed;
    public Color ValueBackgroundColor => Color.FromRgb(50, 50, 50);

    public bool IsFocused
    {
        get => _focused;
        set
        {
            _focused = value;
            this.BackgroundColor = _focused ? FocusedColor : Color.Transparent;
        }
    }

    public double? DisplayValue
    {
        get => _displayValue;
        set
        {
            var v = value switch
            {
                null => "??",
                >= 1000000 => $"{value / 1000f:0.00}M",
                >= 1000 => $"{value / 1000f:0.00}k",
                _ => $"{value:0}"
            };

            _value.Text = v;
            _displayValue = value;
        }
    }

    public int? Value
    {
        get => _progressBar.Value;
        set => _progressBar.Value = value ?? 0;
    }

    public RheostatControl(DisplayScreen screen, int left, int top, int width, int height, IFont font)
        : base(screen, left, top, width, height)
    {
        _font = font;

        this.BackgroundColor = _focused ? FocusedColor : Color.Transparent;

        _progressBar = new ProgressBar(LabelWidth + Margin + 2, Margin, width - LabelWidth - Margin * 3, height - Margin * 2)
        {
            BackColor = ValueBackgroundColor,
            ValueColor = ValueColor,
            Value = 0
        };

        _value = new Label(Margin, Margin, LabelWidth, height - Margin * 2)
        {
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            Text = $"0",
            Font = _font
        };

        this.Controls.Add(_progressBar);
        this.Controls.Add(_value);
    }
}
