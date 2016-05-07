using Microsoft.Xna.Framework.Graphics;

namespace MiniRealms.Engine.Gfx
{
    public class SpriteSheet
    {
        public int Width;
        public int Height;
        public int[] Pixels;

        public SpriteSheet(Texture2D image)
        {
            Width = image.Width;
            Height = image.Height;

            var colors = new Microsoft.Xna.Framework.Color[Width  * Height];
            image.GetData(colors);

            Pixels = new int[Width * Height];
            for (int i = 0; i < Pixels.Length; i++)
                Pixels[i] = colors[i].B / 64;
        }
    }
}
