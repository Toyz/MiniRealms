using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Screens.Interfaces;

namespace MiniRealms.Screens.OptionItems
{
    public class VolumeContol : Option
    {
        private float _current;
        public override string Text { get; set; } = "Volume: ";
        public override string SelectedText => $"< {Text} >";

        protected internal override void HandleInput(InputHandler input)
        {
            float volume = SoundEffectManager.GetMasterVolume();

            _current = volume;
            if (input.Right.Clicked)
            {
                volume += 0.10f;
                if (volume > 1.0f)
                {
                    volume = 1.0f;
                }
            }
            else if (input.Left.Clicked)
            {
                volume -= 0.10f;
                if (volume < 0.0f)
                {
                    volume = 0.0f;
                }
            }

            if (volume != _current)
            {
                GameConts.Instance.Volume = volume;
                GameConts.Instance.Save();
                SoundEffectManager.SetMasterVolume(volume);
            }
            Text = $"Volume: {volume.ToString("P0")}";
        }

        protected internal override void HandleRender()
        {
        }
    }
}
