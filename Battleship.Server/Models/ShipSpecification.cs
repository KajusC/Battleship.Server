using System.Collections.Generic;

namespace Battleship.Server.Models;

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
}
