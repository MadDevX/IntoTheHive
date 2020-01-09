using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Music
{
    [System.Serializable]
    public class SoundClip
    {
        public Sound Name;

        public SoundType SoundType;

        public AudioClip Clip;

        public bool Loop = false;

        [HideInInspector]
        public AudioSource Source;
    }
}
