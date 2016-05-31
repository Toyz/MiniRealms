using MiniRealms.Engine;
using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Engine.Gfx;
using MiniRealms.Items;

namespace MiniRealms.Entities
{

    public class ItemEntity : Entity
    {
        private readonly int _lifeTime;
        protected int WalkDist = 0;
        protected int Dir = 0;
        public int HurtTime;
        protected int XKnockback, YKnockback;
        public double Xa, Ya, Za;
        public double Xx, Yy, Zz;
        public Item Item;
        private int _time;

        public ItemEntity(Item item, int x, int y)
        {
            Item = item;
            Xx = X = x;
            Yy = Y = y;
            Xr = 3;
            Yr = 3;

            Zz = 2;
            Xa = Random.NextGaussian() * 0.3;
            Ya = Random.NextGaussian() * 0.2;
            Za = Random.NextFloat() * 0.7 + 1;

            _lifeTime = 60 * 10 + Random.NextInt(60);
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
            Zz += Za;
            if (Zz < 0)
            {
                Zz = 0;
                Za *= -0.5;
                Xa *= 0.6;
                Ya *= 0.6;
            }
            Za -= 0.15;
            int ox = X;
            int oy = Y;
            int nx = (int)Xx;
            int ny = (int)Yy;
            int expectedx = nx - X;
            int expectedy = ny - Y;
            Move(nx - X, ny - Y);
            int gotx = X - ox;
            int goty = Y - oy;
            Xx += gotx - expectedx;
            Yy += goty - expectedy;

            if (HurtTime > 0) HurtTime--;
        }

        public override bool IsBlockableBy(Mob mob) => false;

        public override void Render(Screen screen)
        {
            if (_time >= _lifeTime - 6 * 20)
            {
                if (_time / 6 % 2 == 0) return;
            }
            screen.Render(X - 4, Y - 4, Item.GetSprite(), Color.Get(-1, 0, 0, 0), 0);
            screen.Render(X - 4, Y - 4 - (int)(Zz), Item.GetSprite(), Item.GetColor(), 0);
        }

        public override void TouchedBy(Entity entity)
        {
            if (_time > 30) entity.TouchItem(this);
        }

        public void Take(Player player)
        {
            GameEffectManager.Play("pickup");
            player.Score++;
            Item.OnTake(this);
            Remove();
        }
    }

}
