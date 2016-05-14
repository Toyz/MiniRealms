using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MiniRealms.Screens.Interfaces;

namespace MiniRealms.Screens.OptionItems
{
    public class WorldSizeOption : Option
    {
        public readonly List<Point> Sizes = new List<Point>
        {
            new Point(128, 128),
            new Point(256, 256),
            new Point(512, 512),
            new Point(1024, 1024)
        };

        public int Selected;

        public override bool Enabled { get; set; } = true;
        public override string Text { get; set; } = "World Size: ";
        public override string SelectedText => $"< {Text} >";

        public WorldSizeOption()
        {
            Point s = Sizes[Selected];

            Text = $"World Size: {s.X}x{s.Y}";
        }

        protected internal override void HandleInput(InputHandler input)
        {
            if (input.Left.Clicked)
            {
                Selected--;
            }

            if (input.Right.Clicked)
            {
                Selected++;
            }

            int len = Sizes.Count;
            if (Selected < 0) Selected += len;
            if (Selected >= len) Selected -= len;

            Point s = Sizes[Selected];

            Text = $"World Size: {s.X}x{s.Y}";
        }

        protected internal override void HandleRender()
        {
        }
    }
}
