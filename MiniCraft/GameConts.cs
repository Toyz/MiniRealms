using Microsoft.Xna.Framework.Graphics;

namespace MiniRealms
{
    public static class GameConts
    {
        public static int MaxHeight = 256;
        public static int MaxWidth = 256;

        public static string Name = "MiniRealms";
        public static string Version = "0.0.1 - Alpha";

        public static int BaseScaling = 5;
        public static int Scale = 3;

        public static int Height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / BaseScaling;
        public static int Width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / BaseScaling;

        public static int ScreenMiddleWidth = Width/2;
        public static int ScreenMiddleHeight = Height/2;
    }
}
