using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace MiniRealms.Engine.Audio.Music
{
    public static class GameSongManager
    {
        private static Dictionary<string, GameSong> AllSongs { get; set; }
        private static bool PlaySong = true;
        private static int currentSong;
        private static Random _randomNumber;

        public static void Initialize(ContentManager content)
        {
            _randomNumber = new Random();
            AllSongs = new Dictionary<string, GameSong>();

            foreach (var fileName in Directory.EnumerateFiles(Path.Combine(content.RootDirectory, "Music"), "*.xnb"))
            {
                var file = Path.GetFileNameWithoutExtension(fileName);

                if (file != null) AllSongs.Add(file, new GameSong(content, file));
            }
        }

        public static float GetMasterVolume()
        {
            return MediaPlayer.Volume;
        }

        public static void SetMasterVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }

        public static void Play(string key)
        {
            Get(key).Play();
        }

        public static void Stop(string key)
        {
            Get(key).Stop();
        }

        public static void Registry(string name, GameSong song)
        {
            AllSongs.Add(name, song);
        }

        public static void PlayNextSong()
        {
            if (MediaPlayer.State != MediaState.Playing)
            {
                PlaySong = false;

                if(!GameConts.Instance.RandomMusicCycle)
                {
                    currentSong++;
                    if (currentSong > AllSongs.Count - 1)
                    {
                        currentSong = 0;
                    }
                }
                else
                {
                    currentSong = _randomNumber.Next(AllSongs.Count);
                }

                var key = AllSongs.Keys.ElementAt(currentSong);

                AllSongs[key].Play();
            }
        }  

        private static GameSong Get(string key)
        {
            if (!AllSongs.ContainsKey(key)) throw new IndexOutOfRangeException($"Sound \"{key}\" doesn't exist");

            return AllSongs[key];
        }
    }
}
