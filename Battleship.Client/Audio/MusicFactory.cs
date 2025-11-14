using System;

namespace Battleship.Client.Audio
{
    public enum MusicType
    {
        Victory,
        Game,
        Menu,
        Defeat
    }

    public static class MusicFactory
    {
        private static readonly IAudioPlayer DefaultPlayer = new SystemAudioPlayer();

        public static IMusic CreateMusic(MusicType type)
        {
            return type switch
            {
                MusicType.Victory => new VictoryMusic(DefaultPlayer),
                MusicType.Game => new GameMusic(DefaultPlayer),
                MusicType.Menu => new MenuMusic(DefaultPlayer),
                MusicType.Defeat => new DefeatMusic(DefaultPlayer),
                _ => throw new ArgumentException("Unknown music type")
            };
        }
    }
}