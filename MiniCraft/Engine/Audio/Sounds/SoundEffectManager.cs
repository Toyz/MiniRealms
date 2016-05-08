using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace MiniRealms.Engine.Audio.Sounds
{
    public class SoundEffectManager
    {
        public static Dictionary<string, SoundEffect> AllSounds { get; protected set; }

        public static void Initialize(ContentManager content)
        {
            AllSounds = new Dictionary<string, SoundEffect>();

            foreach (var fileName in Directory.EnumerateFiles(Path.Combine(content.RootDirectory, "Sounds"), "*.xnb"))
            {
                var file = Path.GetFileNameWithoutExtension(fileName);

                if (file != null) AllSounds.Add(file, new SoundEffect(content, file));
            }
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

        public static SoundEffect Get(string key)
        {
            if (!AllSounds.ContainsKey(key)) throw new IndexOutOfRangeException($"Sound \"{key}\" doesn't exist");

            return AllSounds[key];
        }


    }
}
