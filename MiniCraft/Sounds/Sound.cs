using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace MiniRealms.Sounds
{
    public class Sound
    {
        public static readonly Dictionary<string, Sound> AllSounds = new Dictionary<string, Sound>();

        public static void Initialize(ContentManager content)
        {
            foreach (var fileName in Directory.EnumerateFiles(Path.Combine(content.RootDirectory, "Sounds"), "*.xnb"))
            {
                var file = Path.GetFileNameWithoutExtension(fileName);

                if (file != null) AllSounds.Add(file, new Sound(content, file));
            }
        }

        public static void PlaySound(string key)
        {
            if (!AllSounds.ContainsKey(key)) throw new IndexOutOfRangeException($"Sound \"{key}\" doesn't exist");
            AllSounds[key].Play();
        }


        //actual sound object
        private readonly SoundEffect _soundEffect;

        private Sound(ContentManager content, string fileName, bool isPhysicalPath = false)
        {
            _soundEffect = isPhysicalPath ? content.Load<SoundEffect>(fileName) : content.Load<SoundEffect>("Sounds/" + fileName);
        }

        public void Play()
        {
            _soundEffect.Play();
        }
    }
}
