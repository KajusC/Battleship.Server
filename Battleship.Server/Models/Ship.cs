using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Server.Models;

public sealed class Ship
{
    private readonly HashSet<Coordinate> _positions;
    private readonly HashSet<Coordinate> _hits = new();

    public Ship(string name, IEnumerable<Coordinate> positions)
    {
        Name = name;
        _positions = positions.ToHashSet();
    }

    public string Name { get; }

    public IReadOnlyCollection<Coordinate> Positions => _positions;

    public bool IsSunk => _hits.Count == _positions.Count;

    public void RegisterHit(Coordinate coordinate)
    {
        if (!_positions.Contains(coordinate))
        {
            throw new InvalidOperationException("Cannot register hit on coordinate that does not belong to the ship.");
        }

        _hits.Add(coordinate);
    }
}
