using MiniCraft.Gfx;
using MiniCraft.Items;
using MiniCraft.Items.Resources;

namespace MiniCraft.Entities
{

    public class Zombie : Mob
    {
        private int _xa, _ya;
        private readonly int _lvl;
        private int _randomWalkTime;

        public Zombie(int lvl)
        {
            _lvl = lvl;
            X = Random.NextInt(64 * 16);
            Y = Random.NextInt(64 * 16);
            Health = MaxHealth = lvl * lvl * 10;

        }

        public override void Tick()
        {
            base.Tick();

            if (Level.Player != null && _randomWalkTime == 0)
            {
                int xd = Level.Player.X - X;
                int yd = Level.Player.Y - Y;
                if (xd * xd + yd * yd < 50 * 50)
                {
                    _xa = 0;
                    _ya = 0;
                    if (xd < 0) _xa = -1;
                    if (xd > 0) _xa = +1;
                    if (yd < 0) _ya = -1;
                    if (yd > 0) _ya = +1;
                }
            }

            int speed = TickTime & 1;
            if (!Move(_xa * speed, _ya * speed) || Random.NextInt(200) == 0)
            {
                _randomWalkTime = 60;
                _xa = (Random.NextInt(3) - 1) * Random.NextInt(2);
                _ya = (Random.NextInt(3) - 1) * Random.NextInt(2);
            }
            if (_randomWalkTime > 0) _randomWalkTime--;
        }

        public override void Render(Screen screen)
        {
            int xt = 0;
            int yt = 14;

            int flip1 = (WalkDist >> 3) & 1;
            int flip2 = (WalkDist >> 3) & 1;

            if (Dir == 1)
            {
                xt += 2;
            }
            if (Dir > 1)
            {

                flip1 = 0;
                flip2 = ((WalkDist >> 4) & 1);
                if (Dir == 2)
                {
                    flip1 = 1;
                }
                xt += 4 + ((WalkDist >> 3) & 1) * 2;
            }

            int xo = X - 8;
            int yo = Y - 11;

            int col = ColorHelper.Get(-1, 10, 252, 050);
            if (_lvl == 2) col = ColorHelper.Get(-1, 100, 522, 050);
            if (_lvl == 3) col = ColorHelper.Get(-1, 111, 444, 050);
            if (_lvl == 4) col = ColorHelper.Get(-1, 000, 111, 020);
            if (HurtTime > 0)
            {
                col = ColorHelper.Get(-1, 555, 555, 555);
            }

            screen.Render(xo + 8 * flip1, yo + 0, xt + yt * 32, col, flip1);
            screen.Render(xo + 8 - 8 * flip1, yo + 0, xt + 1 + yt * 32, col, flip1);
            screen.Render(xo + 8 * flip2, yo + 8, xt + (yt + 1) * 32, col, flip2);
            screen.Render(xo + 8 - 8 * flip2, yo + 8, xt + 1 + (yt + 1) * 32, col, flip2);
        }

        public override void TouchedBy(Entity entity) {
		if (entity is Player) {
			entity.Hurt(this, _lvl + 1, Dir);
		}
	}

        protected override void Die()
        {
            base.Die();

            int count = Random.NextInt(2) + 1;
            for (int i = 0; i < count; i++)
            {
                Level.Add(new ItemEntity(new ResourceItem(Resource.Cloth), X + Random.NextInt(11) - 5, Y + Random.NextInt(11) - 5));
            }

            if (Level.Player != null)
            {
                Level.Player.Score += 50 * _lvl;
            }

        }

    }
}
