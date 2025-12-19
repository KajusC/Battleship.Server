using System;
using System.Collections;
using System.Collections.Generic;

namespace Battleship.Client.Models;

public sealed class BoardState : IEnumerable<BoardState.Cell>
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

    public BoardState Clone()
    {
        var clone = new BoardState(Size);
        for (var row = 0; row < Size; row++)
        {
            for (var column = 0; column < Size; column++)
            {
                clone[row, column] = _cells[row, column];
            }
        }
        return clone;
    }

    public readonly record struct Cell(int Row, int Column, CellState State);

    public IEnumerator<Cell> GetEnumerator() => new BoardStateIterator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private sealed class BoardStateIterator : IEnumerator<Cell>
    {
        private readonly BoardState _board;
        private int _index = -1;

        public BoardStateIterator(BoardState board) => _board = board;

        public bool MoveNext()
        {
            _index++;
            return _index < _board.Size * _board.Size;
        }

        public void Reset() => _index = -1;

        public Cell Current
        {
            get
            {
                if (_index < 0 || _index >= _board.Size * _board.Size)
                {
                    throw new InvalidOperationException("Enumeration has not started or has already finished.");
                }

                var row = _index / _board.Size;
                var col = _index % _board.Size;
                return new Cell(row, col, _board[row, col]);
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }
    }
}
