using MiniRealms.Engine;
using MiniRealms.Engine.Gfx;

namespace MiniRealms.Entities.Particles
{
    public class TextParticle : Entity
    {
        private readonly string _msg;
        private readonly int _col;
        private int _time;
        private double _xa;
        private double _ya;
        private double _za;
        private double _xx;
        private double _yy;
        private double _zz;

        public TextParticle(string msg, int x, int y, int col)
        {
            _msg = msg;
            X = x;
            Y = y;
            _col = col;
            _xx = x;
            _yy = y;
            _zz = 2;
            _xa = Random.NextGaussian() * 0.3;
            _ya = Random.NextGaussian() * 0.2;
            _za = Random.NextFloat() * 0.7 + 2;
        }

        public override void Tick()
        {
            _time++;
            if (_time > 60)
            {
                Remove();
            }
            _xx += _xa;
            _yy += _ya;
            _zz += _za;
            if (_zz < 0)
            {
                _zz = 0;
                _za *= -0.5;
                _xa *= 0.6;
                _ya *= 0.6;
            }
            _za -= 0.15;
            X = (int)_xx;
            Y = (int)_yy;
        }

        public override void Render(Screen screen)
        {
            //		Font.draw(msg, screen, x - msg.length() * 4, y, Color.get(-1, 0, 0, 0));
            Font.Draw(_msg, screen, X - _msg.Length() * 4 + 1, Y - (int)(_zz) + 1, Color.Get(-1, 0, 0, 0));
            Font.Draw(_msg, screen, X - _msg.Length() * 4, Y - (int)(_zz), _col);
        }

    }
}
