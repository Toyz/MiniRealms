namespace MiniCraft.Gfx
{
    public class ColorHelper
    {
        public static int Get(int a, int b, int c, int d)
        {
            return (Get(d) << 24) + (Get(c) << 16) + (Get(b) << 8) + (Get(a));
        }

        public static int Get(int d)
        {
            if (d < 0) return 255;
            int r = d / 100 % 10;
            int g = d / 10 % 10;
            int b = d % 10;
            return r * 36 + g * 6 + b;
        }

    }
}
