using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Client.Audio
{
    public abstract class BaseMusic : IMusic
    {
        private readonly IAudioPlayer _player;
        private readonly string _filePath;

        protected BaseMusic(IAudioPlayer player, string filePath)
        {
            _player = player;
            _filePath = filePath;
        }

        public void Play()
        {
            _player.PlayLooping(_filePath);
        }

        public void Stop()
        {
            _player.Stop();
        }
    }
}
