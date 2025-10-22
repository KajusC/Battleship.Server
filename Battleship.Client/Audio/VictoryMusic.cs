namespace Battleship.Client.Audio
{
    public class VictoryMusic : BaseMusic
    {
        public VictoryMusic() : this(new SystemAudioPlayer()) { }

        public VictoryMusic(IAudioPlayer player) : base(player, "Audio/victory.wav") { }
    }
}