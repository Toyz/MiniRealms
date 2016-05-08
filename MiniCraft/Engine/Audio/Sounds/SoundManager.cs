﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MiniRealms.Engine.Audio.Sounds
{
    public class SoundManager
    {
        public static Dictionary<string, Sound> AllSounds { get; protected set; }

        public static void Initialize(ContentManager content)
        {
            AllSounds = new Dictionary<string, Sound>();

            foreach (var fileName in Directory.EnumerateFiles(Path.Combine(content.RootDirectory, "Sounds"), "*.xnb"))
            {
                var file = Path.GetFileNameWithoutExtension(fileName);

                if (file != null) AllSounds.Add(file, new Sound(content, file));
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

        public static void Registry(string name, Sound sound)
        {
            AllSounds.Add(name, sound);
        }

        public static Sound Get(string key)
        {
            if (!AllSounds.ContainsKey(key)) throw new IndexOutOfRangeException($"Sound \"{key}\" doesn't exist");

            return AllSounds[key];
        }


    }
}
