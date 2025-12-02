using Battleship.Client.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Client.Models
{
    public interface GameState
    {
        public void PlayMusic();
        public void StopMusic();
    }
    public class StartingState : GameState
    {
        public void PlayMusic()
        {
        }

        public void StopMusic()
        {
        }
    }
    public class WonState : GameState
    {
        private IMusic music;

        public void PlayMusic()
        {
            music = MusicFactory.CreateMusic(MusicType.Victory);
            music.Play();
        }

        public void StopMusic()
        {
            music.Stop();
        }
    }
    public class LostState : GameState
    {
        private IMusic music;

        public void PlayMusic()
        {
            music = MusicFactory.CreateMusic(MusicType.Defeat);
            music.Play();

        }

        public void StopMusic()
        {

            music.Stop();
        }
    }
    public class WinningState : GameState
    {
        private IMusic music;

        public void PlayMusic()
        {
            music = MusicFactory.CreateMusic(MusicType.Winning);
            music.Play();
        }

        public void StopMusic()
        {
            music.Stop();
        }
    }
    public class LoosingState : GameState
    {
        private IMusic music;
        public void PlayMusic()
        {
            music = MusicFactory.CreateMusic(MusicType.Loosing);
            music.Play();
        }

        public void StopMusic()
        {
            music.Stop();
        }
    }
}
