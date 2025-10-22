using System.Media;

public class DefeatMusic : IMusic
{
    private SoundPlayer? _player;

    public void Play()
    {
        Stop();
        _player = new SoundPlayer("Audio/defeat.wav");
        _player.PlayLooping();
    }

    public void Stop()
    {
        _player?.Stop();
        _player = null;
    }
}