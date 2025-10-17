using System.Media;

public class DefeatMusic : IMusic
{
    private SoundPlayer? _player;

    public void Play()
    {
        Stop();
        _player = new SoundPlayer("C:\\Users\\Pelkių vadas\\Desktop\\Studijos\\OPP\\repo\\Battleship.Server\\Battleship.Client\\Audio\\defeat.wav");
        _player.PlayLooping();
    }

    public void Stop()
    {
        _player?.Stop();
        _player = null;
    }
}