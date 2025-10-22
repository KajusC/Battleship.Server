using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Client.Audio
{
    public interface IAudioPlayer
    {
        /// <summary>Play the specified audio file in a loop.</summary>
        void PlayLooping(string filePath);

        /// <summary>Play the specified audio file once (fire-and-forget).</summary>
        void PlayOnce(string filePath);

        /// <summary>Stop any currently playing audio started by this player.</summary>
        void Stop();
    }
}
