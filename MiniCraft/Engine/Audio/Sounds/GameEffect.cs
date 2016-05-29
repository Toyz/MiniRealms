using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace MiniRealms.Engine.Audio.Sounds
{
    public class GameEffect
    {
        //actual sound object
        private Microsoft.Xna.Framework.Audio.SoundEffect _soundEffect;
        private SoundEffectInstance _soundEffectInstance;

        public GameEffect(ContentManager content, string fileName, bool isPhysicalPath = false)
        {
            _soundEffect = isPhysicalPath ? content.Load<Microsoft.Xna.Framework.Audio.SoundEffect>(fileName) : content.Load<Microsoft.Xna.Framework.Audio.SoundEffect>("Sounds/" + fileName);
            _soundEffectInstance = _soundEffect.CreateInstance();
        }

        public GameEffect(byte[] file, int sampleRate)
        {
            _soundEffect = new Microsoft.Xna.Framework.Audio.SoundEffect(file, sampleRate, AudioChannels.Stereo);
            _soundEffectInstance = _soundEffect.CreateInstance();
        }

        public void Play()
        {
            _soundEffectInstance.Play();
        }

        public void Stop(bool rightaway = true)
        {
           _soundEffectInstance.Stop(rightaway);
        }

        public void Unload()
        {
            _soundEffectInstance = null;
            _soundEffect = null;
        }

    }
}
