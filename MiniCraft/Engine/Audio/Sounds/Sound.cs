using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace MiniRealms.Engine.Audio.Sounds
{
    public class Sound
    {
        //actual sound object
        private SoundEffect _soundEffect;
        private SoundEffectInstance _soundEffectInstance;

        public Sound(ContentManager content, string fileName, bool isPhysicalPath = false)
        {
            _soundEffect = isPhysicalPath ? content.Load<SoundEffect>(fileName) : content.Load<SoundEffect>("Sounds/" + fileName);
            _soundEffectInstance = _soundEffect.CreateInstance();
        }

        public Sound(byte[] file, int sampleRate)
        {
            _soundEffect = new SoundEffect(file, sampleRate, AudioChannels.Stereo);
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
