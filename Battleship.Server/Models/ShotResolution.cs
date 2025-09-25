namespace Battleship.Server.Models;

public sealed class ShotResolution
{
    public ShotResolution(ShotOutcome outcome, Ship? ship)
    {
        Outcome = outcome;
        Ship = ship;
    }

    public ShotOutcome Outcome { get; }
    public Ship? Ship { get; }
}
