using System.Drawing;
using System.Windows.Forms;
using Battleship.Client.Models;

namespace Battleship.Client.UI;

public sealed class BoardControl : Control
{
    private const int MarginSize = 40;
    private const int CellSize = 32;
    private BoardState? _board;
    private bool _showShips;
    private Point? _hoverCell;

    public BoardControl()
    {
        DoubleBuffered = true;
        SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        BackColor = Color.WhiteSmoke;
        ForeColor = Color.Black;
        Font = new Font("Segoe UI", 9f, FontStyle.Regular, GraphicsUnit.Point);
        Size = new Size(MarginSize * 2 + CellSize * 10, MarginSize * 2 + CellSize * 10);
    }

    public BoardState? Board
    {
        get => _board;
        set
        {
            _board = value;

            if (_board != null)
            {
                int requiredSize = MarginSize * 2 + CellSize * _board.Size;
                Size = new Size(requiredSize, requiredSize);
            }

            Invalidate();
        }
    }

    public bool ShowShips
    {
        get => _showShips;
        set
        {
            _showShips = value;
            Invalidate();
        }
    }

    public event EventHandler<BoardCellEventArgs>? CellClicked;

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.Clear(BackColor);

        if (_board is null)
        {
            DrawEmptyBoard(g, 10);
            return;
        }

        DrawBoard(g, _board);
        DrawLabels(g, _board.Size);
        DrawHover(g);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        var cell = GetCellFromPoint(e.Location);
        if (cell != _hoverCell)
        {
            _hoverCell = cell;
            Invalidate();
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _hoverCell = null;
        Invalidate();
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        var cell = GetCellFromPoint(e.Location);
        if (cell.HasValue)
        {
            CellClicked?.Invoke(this, new BoardCellEventArgs(cell.Value.Y, cell.Value.X, e.Button));
        }
    }

    private void DrawEmptyBoard(Graphics g, int size)
    {
        using var pen = new Pen(Color.Gray);
        for (var i = 0; i <= size; i++)
        {
            var x = MarginSize + i * CellSize;
            g.DrawLine(pen, x, MarginSize, x, MarginSize + size * CellSize);
            var y = MarginSize + i * CellSize;
            g.DrawLine(pen, MarginSize, y, MarginSize + size * CellSize, y);
        }
    }

    private void DrawBoard(Graphics g, BoardState board)
    {
        using var gridPen = new Pen(Color.Gray, 1f);
        using var shipBrush = new SolidBrush(Color.FromArgb(130, 30, 144, 255));
        using var missBrush = new SolidBrush(Color.FromArgb(200, Color.LightBlue));
        using var hitBrush = new SolidBrush(Color.FromArgb(220, Color.IndianRed));
        using var sunkBrush = new SolidBrush(Color.FromArgb(255, 105, 97));
        using var notUnknownBrush = new SolidBrush(Color.FromArgb(200, 200, 200, 200));

        foreach (var cell in board)
        {
            var row = cell.Row;
            var col = cell.Column;
            var rect = GetCellRectangle(row, col);
            var state = cell.State;

            switch (state)
            {
                case CellState.Ship when _showShips:
                    g.FillRectangle(shipBrush, rect);
                    break;
                case CellState.Miss:
                    g.FillRectangle(missBrush, rect);
                    break;
                case CellState.Hit:
                    g.FillRectangle(hitBrush, rect);
                    break;
                case CellState.Sunk:
                    g.FillRectangle(sunkBrush, rect);
                    break;
                case CellState.NotUnknown:
                    g.FillRectangle(notUnknownBrush, rect);
                    break;
            }

            g.DrawRectangle(gridPen, rect);

            if (state == CellState.Miss)
            {
                DrawMissMarker(g, rect);
            }
            else if (state is CellState.Hit or CellState.Sunk)
            {
                DrawHitMarker(g, rect);
            }
        }
    }

    private void DrawLabels(Graphics g, int size)
    {
        using var brush = new SolidBrush(ForeColor);
        using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        for (var i = 0; i < size; i++)
        {
            var letterRect = new Rectangle(MarginSize + i * CellSize, MarginSize - CellSize, CellSize, CellSize);
            g.DrawString(((char)('A' + i)).ToString(), Font, brush, letterRect, format);

            var numberRect = new Rectangle(MarginSize - CellSize, MarginSize + i * CellSize, CellSize, CellSize);
            g.DrawString((i + 1).ToString(), Font, brush, numberRect, format);
        }
    }

    private void DrawHover(Graphics g)
    {
        if (_hoverCell is null || _board is null)
        {
            return;
        }

        using var hoverPen = new Pen(Color.FromArgb(180, Color.DarkOrange), 2f);
        var rect = GetCellRectangle(_hoverCell.Value.Y, _hoverCell.Value.X);
        g.DrawRectangle(hoverPen, rect);
    }

    private Rectangle GetCellRectangle(int row, int column)
    {
        return new Rectangle(
            MarginSize + column * CellSize,
            MarginSize + row * CellSize,
            CellSize,
            CellSize);
    }

    private Point? GetCellFromPoint(Point point)
    {
        if (_board is null)
        {
            return null;
        }

        var col = (point.X - MarginSize) / CellSize;
        var row = (point.Y - MarginSize) / CellSize;

        if (col < 0 || row < 0 || col >= _board.Size || row >= _board.Size)
        {
            return null;
        }

        return new Point(col, row);
    }

    private static void DrawMissMarker(Graphics g, Rectangle rect)
    {
        using var pen = new Pen(Color.Navy, 2f);
        var center = new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);
        g.DrawEllipse(pen, center.X - 6, center.Y - 6, 12, 12);
    }

    private static void DrawHitMarker(Graphics g, Rectangle rect)
    {
        using var pen = new Pen(Color.DarkRed, 2.5f);
        g.DrawLine(pen, rect.Left + 5, rect.Top + 5, rect.Right - 5, rect.Bottom - 5);
        g.DrawLine(pen, rect.Left + 5, rect.Bottom - 5, rect.Right - 5, rect.Top + 5);
    }
}

public sealed class BoardCellEventArgs : EventArgs
{
    public BoardCellEventArgs(int row, int column, MouseButtons button)
    {
        Row = row;
        Column = column;
        Button = button;
    }

    public int Row { get; }
    public int Column { get; }
    public MouseButtons Button { get; }
}
