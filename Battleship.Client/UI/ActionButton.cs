using System.Drawing;
using System.Windows.Forms;

namespace Battleship.Client.UI;

public sealed class ActionButton : Control
{
    private bool _hover;
    private bool _pressed;

    public ActionButton()
    {
        Size = new Size(140, 40);
        BackColor = Color.FromArgb(50, 90, 160);
        ForeColor = Color.White;
        Font = new Font("Segoe UI", 9f, FontStyle.Bold, GraphicsUnit.Point);
        Cursor = Cursors.Hand;
        DoubleBuffered = true;
    }

    public new event EventHandler? Click;

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        var rect = ClientRectangle;

        var baseColor = _pressed
            ? Color.FromArgb(70, 110, 190)
            : _hover
                ? Color.FromArgb(60, 100, 175)
                : BackColor;

        using var brush = new SolidBrush(baseColor);
        using var borderPen = new Pen(Color.FromArgb(30, 60, 120), 2f);
        var radius = 10;
        var path = CreateRoundedRectangle(rect, radius);
        g.FillPath(brush, path);
        g.DrawPath(borderPen, path);

        using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString(Text, Font, new SolidBrush(ForeColor), rect, format);
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        _hover = true;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _hover = false;
        _pressed = false;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button == MouseButtons.Left)
        {
            _pressed = true;
            Invalidate();
        }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (_pressed && e.Button == MouseButtons.Left)
        {
            _pressed = false;
            Invalidate();
            Click?.Invoke(this, EventArgs.Empty);
        }
    }

    private static System.Drawing.Drawing2D.GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
    {
        var path = new System.Drawing.Drawing2D.GraphicsPath();
        var diameter = radius * 2;
        var arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));

        // top left
        path.AddArc(arcRect, 180, 90);

        // top right
        arcRect.X = rect.Right - diameter;
        path.AddArc(arcRect, 270, 90);

        // bottom right
        arcRect.Y = rect.Bottom - diameter;
        path.AddArc(arcRect, 0, 90);

        // bottom left
        arcRect.X = rect.Left;
        path.AddArc(arcRect, 90, 90);

        path.CloseFigure();
        return path;
    }
}
