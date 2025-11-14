using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Client.Audio
{
    public class NullAudioPlayer : IAudioPlayer
    {
        public void PlayLooping(string filePath) {  }
        public void PlayOnce(string filePath) {  }
        public void Stop() {  }
    }
}
