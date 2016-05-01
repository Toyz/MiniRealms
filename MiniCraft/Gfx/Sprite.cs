namespace MiniCraft.Gfx
{
    public class Sprite
    {
        public int X, Y;
        public int Img;
        public int Col;
        public int Bits;

        public Sprite(int x, int y, int img, int col, int bits)
        {
            X = x;
            Y = y;
            Img = img;
            Col = col;
            Bits = bits;
        }
    }
}
