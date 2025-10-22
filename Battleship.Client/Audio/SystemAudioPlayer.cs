using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace Battleship.Client.Audio
{
    public class SystemAudioPlayer : IAudioPlayer
    {
        private SoundPlayer? _currentPlayer;

        public void PlayLooping(string filePath)
        {
            Stop();
            _currentPlayer = new SoundPlayer(filePath);
            _currentPlayer.PlayLooping();
        }

        public void PlayOnce(string filePath)
        {
            // PlaySync would block; Play() is non-blocking and plays once.
            var player = new SoundPlayer(filePath);
            player.Play();
        }

        public void Stop()
        {
            _currentPlayer?.Stop();
            _currentPlayer = null;
        }
    }
}
