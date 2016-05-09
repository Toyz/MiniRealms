using MiniRealms.Engine.Audio.Sounds;

namespace MiniRealms.Screens.Options
{
    public class VolumeContol : IOption
    {
        public string Text { get; set; } = "Volume: ";

        public void HandleInput(InputHandler input)
        {
            float volume = SoundEffectManager.GetMasterVolume();

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

            SoundEffectManager.SetMasterVolume(volume);
            Text = $"Volume: {volume.ToString("P0")}";
        }

        public void HandleRender()
        {
        }
    }
}
