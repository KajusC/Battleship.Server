using System.Media;

public class VictoryMusic : IMusic
{
    private SoundPlayer? _player;

    public void Play()
    {
        Stop();
        _player = new SoundPlayer("Audio/victory.wav");
        _player.PlayLooping();
    }

    public void Stop()
    {
        _player?.Stop();
        _player = null;
    }
}