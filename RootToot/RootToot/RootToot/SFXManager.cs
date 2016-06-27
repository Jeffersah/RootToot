using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using NCodeRiddian;

namespace RootToot
{
    class SFXManager
    {
        public static SoundEffect[] PianoSounds;
        public static SoundEffect e3NoteSpawn;

        public static void Load(ContentManager cm)
        {
            e3NoteSpawn = cm.Load<SoundEffect>("SFX\\Spawn\\e3s");
            PianoSounds = new SoundEffect[] { cm.Load<SoundEffect>("SFX\\Piano\\1"), cm.Load<SoundEffect>("SFX\\Piano\\2"), cm.Load<SoundEffect>("SFX\\Piano\\3"), cm.Load<SoundEffect>("SFX\\Piano\\4") };
        }

        public static void PlayPiano()
        {
            GlobalRandom.RandomFrom<SoundEffect>(PianoSounds).Play();
        }

        public static void PlayE3Spawn()
        {
            e3NoteSpawn.Play();
        }
    }
}
