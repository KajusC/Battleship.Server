using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Client.Audio
{
    public interface IAudioPlayer
    {
        void PlayLooping(string filePath);

        void PlayOnce(string filePath);

        void Stop();
    }
}
