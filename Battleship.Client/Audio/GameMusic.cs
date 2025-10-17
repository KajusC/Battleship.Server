using System.Media;

public class GameMusic : IMusic
{
    private SoundPlayer? _player;

    public void Play()
    {
        Stop();
        _player = new SoundPlayer("Audio/game.wav");
        _player.PlayLooping();
    }

    public void Stop()
    {
        _player?.Stop();
        _player = null;
    }
}