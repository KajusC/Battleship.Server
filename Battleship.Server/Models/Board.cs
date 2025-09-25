using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Server.Models;

public sealed class Board
{
    private readonly int _size;
    private readonly BoardCell[,] _cells;
    private readonly List<Ship> _ships = new();

    public Board(int size)
    {
        if (size <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size));
        }

        _size = size;
        _cells = new BoardCell[size, size];
        for (var row = 0; row < size; row++)
        {
            for (var col = 0; col < size; col++)
            {
                _cells[row, col] = new BoardCell();
            }
        }
    }

    public int Size => _size;

    public IReadOnlyCollection<Ship> Ships => _ships;

    public bool AreAllShipsSunk => _ships.All(s => s.IsSunk);

    public void Clear()
    {
        foreach (var cell in _cells)
        {
            cell.Ship = null;
            cell.IsShot = false;
        }
        _ships.Clear();
    }

    public void PlaceShips(IEnumerable<Ship> ships)
    {
        Clear();
        foreach (var ship in ships)
        {
            PlaceShip(ship);
        }
    }

    public void PlaceShip(Ship ship)
    {
        foreach (var position in ship.Positions)
        {
            EnsureWithinBounds(position);
            if (_cells[position.Row, position.Column].Ship is not null)
            {
                throw new InvalidOperationException("Ship placement overlaps with existing ship.");
            }
        }

        foreach (var position in ship.Positions)
        {
            _cells[position.Row, position.Column].Ship = ship;
        }

        _ships.Add(ship);
    }

    public ShotResolution Shoot(Coordinate coordinate)
    {
        EnsureWithinBounds(coordinate);
        var cell = _cells[coordinate.Row, coordinate.Column];
        if (cell.IsShot)
        {
            throw new InvalidOperationException("Coordinate already shot.");
        }

        cell.IsShot = true;
        if (cell.Ship is null)
        {
            return new ShotResolution(ShotOutcome.Miss, null);
        }

        cell.Ship.RegisterHit(coordinate);
        var outcome = cell.Ship.IsSunk ? ShotOutcome.Sunk : ShotOutcome.Hit;
        return new ShotResolution(outcome, cell.Ship);
    }

    private void EnsureWithinBounds(Coordinate coordinate)
    {
        if (!coordinate.IsWithin(_size))
        {
            throw new InvalidOperationException("Coordinate is outside of the board.");
        }
    }
}
