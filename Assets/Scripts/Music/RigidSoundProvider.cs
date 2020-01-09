using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Music
{
    class RigidSoundProvider : ISoundProvider
    {
        private AudioManager _audioManager;
        public RigidSoundProvider(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        public void PlayShootSound()
        {
            _audioManager.Play(Sound.BulletShoot);
            Debug.Log("Shoot sound");
        }

        public void PlayHitSound()
        {
            _audioManager.Play(Sound.BulletHit);
            Debug.Log("Hit sound");
        }
    }
}
