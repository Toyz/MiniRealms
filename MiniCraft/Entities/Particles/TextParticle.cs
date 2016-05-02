using MiniCraft.Gfx;

namespace MiniCraft.Entities.Particles
{
    public class TextParticle : Entity
    {
        private readonly string _msg;
        private readonly int _col;
        private int _time;
        public double Xa;
        public double Ya;
        public double Za;
        public double Xx;
        public double Yy;
        public double Zz;

        public TextParticle(string msg, int x, int y, int col)
        {
            _msg = msg;
            X = x;
            Y = y;
            _col = col;
            Xx = x;
            Yy = y;
            Zz = 2;
            Xa = Random.NextGaussian() * 0.3;
            Ya = Random.NextGaussian() * 0.2;
            Za = Random.NextFloat() * 0.7 + 2;
        }

        public override void Tick()
        {
            _time++;
            if (_time > 60)
            {
                Remove();
            }
            Xx += Xa;
            Yy += Ya;
            Zz += Za;
            if (Zz < 0)
            {
                Zz = 0;
                Za *= -0.5;
                Xa *= 0.6;
                Ya *= 0.6;
            }
            Za -= 0.15;
            X = (int)Xx;
            Y = (int)Yy;
        }

        public override void Render(Screen screen)
        {
            //		Font.draw(msg, screen, x - msg.length() * 4, y, Color.get(-1, 0, 0, 0));
            Font.Draw(_msg, screen, X - _msg.Length() * 4 + 1, Y - (int)(Zz) + 1, Color.Get(-1, 0, 0, 0));
            Font.Draw(_msg, screen, X - _msg.Length() * 4, Y - (int)(Zz), _col);
        }

    }
}
