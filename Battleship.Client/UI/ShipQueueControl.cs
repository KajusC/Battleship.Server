using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Battleship.Client.Models;

namespace Battleship.Client.UI;

public sealed class ShipQueueControl : Control
{
    private readonly List<ShipDefinition> _fleet = new();
    private readonly HashSet<string> _placedShips = new();
    private string? _currentShipName;

    public ShipQueueControl()
    {
        DoubleBuffered = true;
        BackColor = Color.FromArgb(240, 240, 240);
        ForeColor = Color.Black;
        Font = new Font("Segoe UI", 9f, FontStyle.Regular, GraphicsUnit.Point);
        Size = new Size(200, 200);
    }

    public void SetFleet(IEnumerable<ShipDefinition> ships)
    {
        _fleet.Clear();
        _fleet.AddRange(ships);
        _placedShips.Clear();
        Invalidate();
    }
    public void Clear()
    {
        _placedShips.Clear();
        Invalidate();
    }
    public void MarkPlaced(string shipName)
    {
        _placedShips.Add(shipName);
        Invalidate();
    }


    public void SetCurrentShip(string? shipName)
    {
        _currentShipName = shipName;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.Clear(BackColor);

        var y = 10;
        foreach (var ship in _fleet)
        {
            var isPlaced = _placedShips.Contains(ship.Name);
            var isCurrent = string.Equals(ship.Name, _currentShipName, StringComparison.OrdinalIgnoreCase);

            var rect = new Rectangle(10, y, Width - 20, 32);
            using var background = new SolidBrush(isPlaced
                ? Color.FromArgb(80, 180, 80)
                : isCurrent
                    ? Color.FromArgb(255, 215, 120)
                    : Color.FromArgb(220, 220, 220));
            using var borderPen = new Pen(Color.FromArgb(160, 160, 160));
            g.FillRectangle(background, rect);
            g.DrawRectangle(borderPen, rect);
            var text = $"{ship.Name} ({ship.Length})";
            TextRenderer.DrawText(g, text, Font, new Point(rect.X + 8, rect.Y + 8), ForeColor);

            y += rect.Height + 6;
        }
        if (_fleet.Count == 0)
        {
            TextRenderer.DrawText(g, "No fleet information", Font, new Point(10, 10), ForeColor);
        }
    }
}
