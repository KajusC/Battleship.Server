using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Client.Audio
{
    // Simple no-op implementor useful for unit tests or headless runs.
    public class NullAudioPlayer : IAudioPlayer
    {
        public void PlayLooping(string filePath) { /* no-op */ }
        public void PlayOnce(string filePath) { /* no-op */ }
        public void Stop() { /* no-op */ }
    }
}
