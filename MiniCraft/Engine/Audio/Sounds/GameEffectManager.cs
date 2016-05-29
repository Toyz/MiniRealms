using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace MiniRealms.Engine.Audio.Sounds
{
    public static class GameEffectManager
    {
        public static Dictionary<string, GameEffect> AllSounds { get; private set; }

        public static void Initialize(ContentManager content)
        {
            AllSounds = new Dictionary<string, GameEffect>();

            foreach (var fileName in Directory.EnumerateFiles(Path.Combine(content.RootDirectory, "Sounds"), "*.xnb"))
            {
                var file = Path.GetFileNameWithoutExtension(fileName);

                if (file != null) AllSounds.Add(file, new GameEffect(content, file));
            }
        }

        public static float GetMasterVolume()
        {
            return SoundEffect.MasterVolume;
        }

        public static void SetMasterVolume(float volume)
        {
            SoundEffect.MasterVolume = volume;
        }

        public static void Play(string key)
        {
            Get(key).Play();
        }

        public static void Stop(string key)
        {
            Get(key).Stop();
        }

        public static void Registry(string name, GameEffect gameEffect)
        {
            AllSounds.Add(name, gameEffect);
        }

        private static GameEffect Get(string key)
        {
            if (!AllSounds.ContainsKey(key)) throw new IndexOutOfRangeException($"Sound \"{key}\" doesn't exist");

            return AllSounds[key];
        }
    }
}
