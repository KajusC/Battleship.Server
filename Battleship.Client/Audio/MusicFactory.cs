public enum MusicType
{
    Victory,
    Game,
    Menu,
    Defeat
}

public static class MusicFactory
{
    public static IMusic CreateMusic(MusicType type)
    {
        return type switch
        {
            MusicType.Victory => new VictoryMusic(),
            MusicType.Game => new GameMusic(),
            MusicType.Menu => new MenuMusic(),
            MusicType.Defeat=> new DefeatMusic(),
            _ => throw new ArgumentException("Unknown music type")
        };
    }
}