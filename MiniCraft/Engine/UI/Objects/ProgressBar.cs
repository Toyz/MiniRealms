using System;
using MiniRealms.Engine.Gfx;
using MiniRealms.Engine.UI.Interface;

namespace MiniRealms.Engine.UI.Objects
{
    public class ProgressBar : UiObject
    {
        public int Max { get; set; } = 100;
        public int Progress { get; set; } = 0;
        public int FinishedColor { get; set; } = Color.Get(5, 10, 252, 050);
        public int NotFinishedColor { get; set; } = Color.Get(333, 333, 333, 333);
        public int Width { get; set; }

        public ProgressBar(UiManager manager) : base(manager)
        {
        }

        public override void Tick()
        {
        }

        public override void Render(Screen screen)
        {
            int w = Max/Width;

            double prog = Progress;
            if (prog > 0)
            {
                prog = Math.Floor((double)Progress / 100 * 100);
            }

            if (prog > Max)
            {
                prog = Max;
            }

            for (var i = 0; i < Width; i++)
            {
                var c = NotFinishedColor;
                if ((i + 1) * w <= prog)
                {
                    c = FinishedColor;
                }

                Font.Draw(" ", screen, (i + X) * 8, Y, c);
            }
        }
    }
}
