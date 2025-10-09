namespace Battleship.Client.Models;

public sealed class BoardState
{
    private readonly CellState[,] _cells;

    public BoardState(int size)
    {
        Size = size;
        _cells = new CellState[size, size];
    }

    public int Size { get; }

    public CellState this[int row, int column]
    {
        get => _cells[row, column];
        set => _cells[row, column] = value;
    }

    public void Clear(CellState state = CellState.Unknown)
    {
        for (var row = 0; row < Size; row++)
        {
            for (var column = 0; column < Size; column++)
            {
                _cells[row, column] = state;
            }
        }
    }
}
