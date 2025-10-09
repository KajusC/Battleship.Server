using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Battleship.Client.UI;

public sealed class LogControl : Control
{
    private readonly List<string> _entries = new();
    private readonly object _sync = new();

    public LogControl()
    {
        DoubleBuffered = true;
        BackColor = Color.FromArgb(20, 20, 30);
        ForeColor = Color.White;
        Font = new Font("Consolas", 9f, FontStyle.Regular, GraphicsUnit.Point);
    }

    public void AddEntry(string message)
    {
        lock (_sync)
        {
            _entries.Add($"{DateTime.Now:HH:mm:ss}  {message}");
            const int maxEntries = 300;
            if (_entries.Count > maxEntries)
            {
                _entries.RemoveRange(0, _entries.Count - maxEntries);
            }
        }

        Invalidate();
    }

    public void ClearEntries()
    {
        lock (_sync)
        {
            _entries.Clear();
        }

        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.Clear(BackColor);

        List<string> copy;
        lock (_sync)
        {
            copy = _entries.ToList();
        }

        var lineHeight = TextRenderer.MeasureText("A", Font).Height + 2;
        var x = 8;
        var y = ClientSize.Height - lineHeight - 6;

        for (var i = copy.Count - 1; i >= 0; i--)
        {
            TextRenderer.DrawText(e.Graphics, copy[i], Font, new Point(x, y), ForeColor);
            y -= lineHeight;
            if (y < 0)
            {
                break;
            }
        }
    }
}