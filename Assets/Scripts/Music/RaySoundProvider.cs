using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Music
{
    class RaySoundProvider : ISoundProvider
    {
        private AudioManager _audioManager;
        public RaySoundProvider(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        public void PlayShootSound()
        {
            _audioManager.Play(Sound.LaserShoot);
        }

        public void PlayHitSound()
        {
            _audioManager.Play(Sound.LaserHit);
        }
    }
}
