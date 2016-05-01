﻿using MiniCraft.Gfx;
using MiniCraft.Items;
using MiniCraft.Items.Resources;

namespace MiniCraft.Entities
{
    public class Slime : Mob
    {
        private int _xa, _ya;
        private int _jumpTime;
        private readonly int _lvl;

        public Slime(int lvl)
        {
            _lvl = lvl;
            X = Random.NextInt(64 * 16);
            Y = Random.NextInt(64 * 16);
            Health = MaxHealth = lvl * lvl * 5;
        }

        public override void Tick()
        {
            base.Tick();

            int speed = 1;
            if (!Move(_xa * speed, _ya * speed) || Random.NextInt(40) == 0)
            {
                if (_jumpTime <= -10)
                {
                    _xa = (Random.NextInt(3) - 1);
                    _ya = (Random.NextInt(3) - 1);

                    if (Level.Player != null)
                    {
                        int xd = Level.Player.X - X;
                        int yd = Level.Player.Y - Y;
                        if (xd * xd + yd * yd < 50 * 50)
                        {
                            if (xd < 0) _xa = -1;
                            if (xd > 0) _xa = +1;
                            if (yd < 0) _ya = -1;
                            if (yd > 0) _ya = +1;
                        }

                    }

                    if (_xa != 0 || _ya != 0) _jumpTime = 10;
                }
            }

            _jumpTime--;
            if (_jumpTime == 0)
            {
                _xa = _ya = 0;
            }
        }

        protected override void Die()
        {
            base.Die();

            int count = Random.NextInt(2) + 1;
            for (int i = 0; i < count; i++)
            {
                Level.Add(new ItemEntity(new ResourceItem(Resource.Slime), X + Random.NextInt(11) - 5, Y + Random.NextInt(11) - 5));
            }

            if (Level.Player != null)
            {
                Level.Player.Score += 25 * _lvl;
            }

        }

        public override void Render(Screen screen)
        {
            int xt = 0;
            int yt = 18;

            int xo = X - 8;
            int yo = Y - 11;

            if (_jumpTime > 0)
            {
                xt += 2;
                yo -= 4;
            }

            int col = ColorHelper.Get(-1, 10, 252, 555);
            if (_lvl == 2) col = ColorHelper.Get(-1, 100, 522, 555);
            if (_lvl == 3) col = ColorHelper.Get(-1, 111, 444, 555);
            if (_lvl == 4) col = ColorHelper.Get(-1, 000, 111, 224);

            if (HurtTime > 0)
            {
                col = ColorHelper.Get(-1, 555, 555, 555);
            }

            screen.Render(xo + 0, yo + 0, xt + yt * 32, col, 0);
            screen.Render(xo + 8, yo + 0, xt + 1 + yt * 32, col, 0);
            screen.Render(xo + 0, yo + 8, xt + (yt + 1) * 32, col, 0);
            screen.Render(xo + 8, yo + 8, xt + 1 + (yt + 1) * 32, col, 0);
        }

        public override void TouchedBy(Entity entity)
        {
            if (entity is Player)
            {
                entity.Hurt(this, _lvl, Dir);
            }
        }
    }
}