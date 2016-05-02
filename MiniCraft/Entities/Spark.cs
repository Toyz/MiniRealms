using MiniCraft.Gfx;

namespace MiniCraft.Entities
{
    public class Spark : Entity
    {
        private readonly int _lifeTime;
        public double Xa, Ya;
        public double Xx, Yy;
        private int _time;
        private readonly AirWizard _owner;

        public Spark(AirWizard owner, double xa, double ya)
        {
            _owner = owner;
            Xx = X = owner.X;
            Yy = Y = owner.Y;
            Xr = 0;
            Yr = 0;

            Xa = xa;
            Ya = ya;

            _lifeTime = 60*10 + Random.NextInt(30);
        }

        public override void Tick()
        {
            _time++;
            if (_time >= _lifeTime)
            {
                Remove();
                return;
            }
            Xx += Xa;
            Yy += Ya;
            X = (int) Xx;
            Y = (int) Yy;
            var toHit = Level.GetEntities(X, Y, X, Y);
            for (int i = 0; i < toHit.Size(); i++)
            {
                var e = toHit.Get(i);
                var mob = e as Mob;
                if (mob != null && !(e is AirWizard))
                {
                    e.Hurt(_owner, 1, mob.Dir ^ 1);
                }
            }
        }

        public override bool IsBlockableBy(Mob mob) => false;

        public override void Render(Screen screen)
        {
            if (_time >= _lifeTime - 6*20)
            {
                if (_time/6%2 == 0) return;
            }

            int xt = 8;
            int yt = 13;

            screen.Render(X - 4, Y - 4 - 2, xt + yt*32, Color.Get(-1, 555, 555, 555), Random.NextInt(4));
            screen.Render(X - 4, Y - 4 + 2, xt + yt*32, Color.Get(-1, 000, 000, 000), Random.NextInt(4));
        }
    }
}
