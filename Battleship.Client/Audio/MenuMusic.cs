using System.Media;

namespace Battleship.Client.Audio
{
    public class MenuMusic : BaseMusic
    {
        public MenuMusic() : this(new SystemAudioPlayer()) { }

        public MenuMusic(IAudioPlayer player) : base(player, "Audio/menu.wav") { }
    }
}