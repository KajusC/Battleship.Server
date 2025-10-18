using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Server.Models;


public enum ShipOrientation
{
    Horizontal,
    Vertical
}

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

public static class ShipSpecification
{
    public static IReadOnlyDictionary<string, int> StandardFleet { get; } = new Dictionary<string, int>
    {
        ["Carrier"] = 5,
        ["Battleship"] = 4,
        ["Cruiser"] = 3,
        ["Submarine"] = 3,
        ["Destroyer"] = 2
    };

    public static IReadOnlyDictionary<string, int> AdvancedFleet { get; } = new Dictionary<string, int>
    {
        ["Carrier"] = 5,
        ["Battleship"] = 4,
        ["Cruiser"] = 3,
        ["Submarine"] = 3,
        ["Frigate"] = 3,
        ["Destroyer"] = 2
    };
}

public sealed class ShipPlacement

{
    public ShipPlacement(string name, Coordinate start, ShipOrientation orientation)
    {
        Name = name;
        Start = start;
        Orientation = orientation;
    }

    public string Name { get; }
    public Coordinate Start { get; }
    public ShipOrientation Orientation { get; }
}

