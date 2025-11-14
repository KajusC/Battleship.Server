using System.Media;

namespace Battleship.Client.Audio
{
    public class DefeatMusic : BaseMusic
    {
        public DefeatMusic() : this(new SystemAudioPlayer()) { }

        public DefeatMusic(IAudioPlayer player) : base(player, "Audio/defeat.wav") { }
    }
}