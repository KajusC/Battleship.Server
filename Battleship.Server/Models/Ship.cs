using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Server.Models;


public enum ShipOrientation
{
    Horizontal,
    Vertical
}

public enum ShipType
{
    Carrier,
    Battleship,
    Cruiser,
    Submarine,
    Destroyer
}

public sealed class Ship
{
    private readonly HashSet<Coordinate> _positions;
    private readonly HashSet<Coordinate> _hits = new();

    public Ship(ShipType type, IEnumerable<Coordinate> positions)
    {
        Type = type;
        _positions = positions.ToHashSet();
    }

    public ShipType Type { get; }

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
    public static IReadOnlyDictionary<ShipType, int> StandardFleet { get; } = new Dictionary<ShipType, int>
    {
        [ShipType.Carrier] = 5,
        [ShipType.Battleship] = 4,
        [ShipType.Cruiser] = 3,
        [ShipType.Submarine] = 3,
        [ShipType.Destroyer] = 2
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
    public ShipPlacement(ShipType shipType, Coordinate start, ShipOrientation orientation)
    {
        ShipType = shipType;
        Start = start;
        Orientation = orientation;
    }

    public ShipType ShipType { get; }
    public Coordinate Start { get; }
    public ShipOrientation Orientation { get; }
}

