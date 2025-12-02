using System;

namespace Battleship.Client.Audio
{
    public enum MusicType
    {
        Victory,
        Winning,
        Loosing,
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
                MusicType.Loosing => new LoosingMusic(DefaultPlayer),
                MusicType.Winning => new WinningMusic(DefaultPlayer),
                MusicType.Game => new GameMusic(DefaultPlayer),
                MusicType.Menu => new MenuMusic(DefaultPlayer),
                MusicType.Defeat => new DefeatMusic(DefaultPlayer),
                _ => throw new ArgumentException("Unknown music type")
            };
        }
    }
    public class WinningMusic : BaseMusic
    {
        public WinningMusic() : this(new SystemAudioPlayer()) { }

        public WinningMusic(IAudioPlayer player) : base(player, "Audio/Bangers/battlefield_1942.wav") { }
    }

    public class LoosingMusic : BaseMusic
    {
        public LoosingMusic() : this(new SystemAudioPlayer()) { }
        public LoosingMusic(IAudioPlayer player) : base(player, "Audio/Bangers/loosing.wav") { }
    }
}

