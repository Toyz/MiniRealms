using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace MiniRealms.Sounds
{
    public class Sound
    {
        public static Sound PlayerHurt { get; private set; }
        public static Sound PlayerDeath { get; private set; }
        public static Sound MonsterHurt { get; private set; }
        public static Sound Test { get; private set; }
        public static Sound Pickup { get; private set; }
        public static Sound Bossdeath { get; private set; }
        public static Sound Craft { get; private set; }

        public static void Initialize(ContentManager content)
        {
            PlayerHurt = new Sound(content, "playerhurt");
            PlayerDeath = new Sound(content, "death");
            MonsterHurt = new Sound(content, "monsterhurt");
            Test = new Sound(content, "test");
            Pickup = new Sound(content, "pickup");
            Bossdeath = new Sound(content, "bossdeath");
            Craft = new Sound(content, "craft");
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
