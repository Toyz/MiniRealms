using System;
using MiniRealms.Engine.Audio.Music;
using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Screens.Interfaces;

namespace MiniRealms.Screens.OptionItems
{
    public class VolumeContol : Option
    {
        public enum SoundType
        {
            Effects,
            Music
        }

        private readonly SoundType _type;

        private float _current;
        public override bool Enabled { get; set; } = true;
        public sealed override string Text { get; set; }
        public override string SelectedText => $"< {Text} >";

        private readonly string _volumeLabel;

        public VolumeContol(SoundType type, string volumeLabel)
        {
            _type = type;

            _volumeLabel = volumeLabel;

            switch (type)
            {
                case SoundType.Effects:
                    Text = $"{_volumeLabel}{GameConts.Instance.SoundEffectVolume.ToString("P0")}";
                    break;
                case SoundType.Music:
                    Text = $"{_volumeLabel}{GameConts.Instance.MusicVolume.ToString("P0")}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

        }

        protected internal override void HandleInput(InputHandler input)
        {
            float volume = SoundEffectManager.GetMasterVolume();

            if (_type == SoundType.Music)
            {
                volume = GameSongManager.GetMasterVolume();
            }

            _current = volume;
            if (input.Right.Clicked)
            {
                volume += 0.05f;
                if (volume > 1.0f)
                {
                    volume = 1.0f;
                }
            }
            else if (input.Left.Clicked)
            {
                volume -= 0.05f;
                if (volume < 0.0f)
                {
                    volume = 0.0f;
                }
            }

            if (!volume.Equals(_current))
            {
                switch (_type)
                {
                    case SoundType.Effects:
                        GameConts.Instance.SoundEffectVolume = volume;
                        SoundEffectManager.SetMasterVolume(volume);
                        break;
                    case SoundType.Music:
                        GameConts.Instance.MusicVolume = volume;
                        GameSongManager.SetMasterVolume(volume);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                GameConts.Instance.Save();
            }

            Text = $"{_volumeLabel}{volume.ToString("P0")}";
        }

        protected internal override void HandleRender()
        {
        }
    }
}
