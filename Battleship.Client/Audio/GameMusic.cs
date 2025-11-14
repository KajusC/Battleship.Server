using System.Media;

namespace Battleship.Client.Audio
{
    public class GameMusic : BaseMusic
    {
        public GameMusic() : this(new SystemAudioPlayer()) { }

        public GameMusic(IAudioPlayer player) : base(player, "Audio/game.wav") { }
    }
}