using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace MiniRealms.Engine.Audio.Sounds
{
    public static class SoundEffectManager
    {
        public static Dictionary<string, SoundEffect> AllSounds { get; private set; }

        public static void Initialize(ContentManager content)
        {
            AllSounds = new Dictionary<string, SoundEffect>();

            foreach (var fileName in Directory.EnumerateFiles(Path.Combine(content.RootDirectory, "Sounds"), "*.xnb"))
            {
                var file = Path.GetFileNameWithoutExtension(fileName);

                if (file != null) AllSounds.Add(file, new SoundEffect(content, file));
            }
        }

        public static float GetMasterVolume()
        {
            return Microsoft.Xna.Framework.Audio.SoundEffect.MasterVolume;
        }

        public static void SetMasterVolume(float volume)
        {
            Microsoft.Xna.Framework.Audio.SoundEffect.MasterVolume = volume;
        }

        public static void Play(string key)
        {
            Get(key).Play();
        }

        public static void Stop(string key)
        {
            Get(key).Stop();
        }

        public static void Registry(string name, SoundEffect soundEffect)
        {
            AllSounds.Add(name, soundEffect);
        }

        private static SoundEffect Get(string key)
        {
            if (!AllSounds.ContainsKey(key)) throw new IndexOutOfRangeException($"Sound \"{key}\" doesn't exist");

            return AllSounds[key];
        }
    }
}
