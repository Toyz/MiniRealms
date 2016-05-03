using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace MiniRealms.Sounds
{
    public class Sound
    {
        public static Dictionary<string, Sound> AllSounds = new Dictionary<string, Sound>();

        public static Sound PlayerHurt { get; private set; }
        public static Sound PlayerDeath { get; private set; }
        public static Sound MonsterHurt { get; private set; }
        public static Sound Test { get; private set; }
        public static Sound Pickup { get; private set; }
        public static Sound Bossdeath { get; private set; }
        public static Sound Craft { get; private set; }
        public static Sound Fuse { get; private set; }
        public static Sound Boom { get; private set; }

        public static void Initialize(ContentManager content)
        {
            PlayerHurt = new Sound(content, "playerhurt");
            PlayerDeath = new Sound(content, "death");
            MonsterHurt = new Sound(content, "monsterhurt");
            Test = new Sound(content, "test");
            Pickup = new Sound(content, "pickup");
            Bossdeath = new Sound(content, "bossdeath");
            Craft = new Sound(content, "craft");
            Fuse = new Sound(content, "fuse");
            Boom = new Sound(content, "boom");

            AllSounds.Add(nameof(PlayerHurt).ToLower(), PlayerHurt);
            AllSounds.Add(nameof(PlayerDeath).ToLower(), PlayerDeath);
            AllSounds.Add(nameof(MonsterHurt).ToLower(), MonsterHurt);
            AllSounds.Add(nameof(Test).ToLower(), Test);
            AllSounds.Add(nameof(Pickup).ToLower(), Pickup);
            AllSounds.Add(nameof(Bossdeath).ToLower(), Bossdeath);
            AllSounds.Add(nameof(Craft).ToLower(), Craft);
            AllSounds.Add(nameof(Fuse).ToLower(), Fuse);
            AllSounds.Add(nameof(Boom).ToLower(), Boom);
        }

        private readonly SoundEffect _soundEffect;

        private Sound(ContentManager content, string path)
        {
            _soundEffect = content.Load<SoundEffect>("Sounds/" + path);
        }

        public void Play()
        {
            _soundEffect.Play();
        }
    }
}
