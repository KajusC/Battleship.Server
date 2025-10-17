using System.Media;

public class MenuMusic : IMusic
{
    private SoundPlayer? _player;

    public void Play()
    {
        Stop();
        _player = new SoundPlayer("Audio/menu.wav");
        _player.PlayLooping();
    }

    public void Stop()
    {
        _player?.Stop();
        _player = null;
    }
}