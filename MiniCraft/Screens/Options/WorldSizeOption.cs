using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MiniRealms.Engine.Audio.Sounds;

namespace MiniRealms.Screens.Options
{
    public class WorldSizeOption : IOption
    {
        public readonly List<Point> Sizes = new List<Point>
        {
            new Point(128, 128),
            new Point(256, 256),
            new Point(512, 512),
            new Point(1024, 1024)
        };

        public int Selected;

        public string Text { get; set; } = "World Size: ";

        public void Tick(InputHandler input)
        {
            if (input.Left.Clicked)
            {
                Selected--;
                SoundEffectManager.Play("menu_move");
            }

            if (input.Right.Clicked)
            {
                SoundEffectManager.Play("menu_move");
                Selected++;
            }

            int len = Sizes.Count;
            if (Selected < 0) Selected += len;
            if (Selected >= len) Selected -= len;

            Point s = Sizes[Selected];

            Text = $"World Size: {s.X}x{s.Y}";
        }

        public void Update()
        {
        }
    }
}
