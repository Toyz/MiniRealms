using System;
using MiniRealms.Engine.Gfx;
using MiniRealms.Levels.Tiles;
using MiniRealms.Sounds;

namespace MiniRealms.Entities
{
    public class Creeper : Mob
    {
        private static int _maxFuseTime = 60;
        private static int _blastRadius = 30;
        private static int _blastDamage = 20;
        private int _fuseTime;
        private bool _fuseLit;


        private int _xa, _ya;
        private readonly int _lvl;
        private int _randomWalkTime;

        public Creeper(int lvl)
        {
            _lvl = lvl;
            _fuseLit = false;

            X = Random.NextInt(64 * 16);
            Y = Random.NextInt(64 * 16);

            Health = MaxHealth = lvl * lvl * 5;
        }

        public override void Tick()
        {
            base.Tick();

            if (_fuseTime == 0)
            {
                if (!_fuseLit)
                {
                    if (Level.Player != null && _randomWalkTime == 0)
                    {
                        int xd = Level.Player.X - X;
                        int yd = Level.Player.Y - Y;
                        if (xd*xd + yd*yd < 50*50)
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
                    if (!Move(_xa*speed, _ya*speed) || Random.NextInt(200) == 0)
                    {
                        _randomWalkTime = 60;
                        _xa = (Random.NextInt(3) - 1)*Random.NextInt(2);
                        _ya = (Random.NextInt(3) - 1)*Random.NextInt(2);
                    }
                    if (_randomWalkTime > 0) _randomWalkTime--;
                }
                else
                {
                    int pdx = Math.Abs(Level.Player.X - X);
                    int pdy = Math.Abs(Level.Player.Y - Y);
                    if ((pdx < _blastRadius) && (pdy < _blastRadius))
                    {
                        float pd = (float)Math.Sqrt(pdx * pdx + pdy * pdy);
                        int dmg = (int)(_blastDamage * (1.0F - pd / _blastRadius)) + 1;
                        Level.Player.Hurt(this, dmg, 0);
                        Level.Player.PayStamina(dmg * 2);

                        int xt = X >> 4;
                        int yt = Y - 2 >> 4;
                        if (_lvl == 4)
                        {
                            Level.SetTile(xt, yt, Tile.InfiniteFall, 0);
                        }
                        else if (_lvl == 3)
                        {
                            Level.SetTile(xt, yt, Tile.Lava, 0);
                        }
                        else
                        {
                            Level.SetTile(xt, yt, Tile.Hole, 0);
                        }

                        Sound.PlaySound("boom");
                        Die();
                    }
                    else
                    {
                        _fuseTime = 0;
                        _fuseLit = false;
                    }
                }
            }
            else
            {
                _fuseTime -= 1;
            }
        }

        public override void Render(Screen screen)
        {
            int xt = 4;
            int yt = 18;
            if (WalkDist > 0)
            {
                if (Random.NextInt(2) == 0)
                {
                    xt += 2;
                }
                else
                {
                    xt += 4;
                }
            }
            else
            {
                xt = 4;
            }
            int xo = X - 8;
            int yo = Y - 11;

            int col = Color.Get(-1, 10, 252, 242);
            if (_lvl == 2)
            {
                col = Color.Get(-1, 200, 262, 232);
            }
            if (_lvl == 3)
            {
                col = Color.Get(-1, 200, 272, 222);
            }
            if (_lvl == 4)
            {
                col = Color.Get(-1, 200, 292, 282);
            }
            if (_fuseLit &&
              (_fuseTime % 6 == 0))
            {
                col = Color.Get(-1, 252, 252, 252);
            }
            if (HurtTime > 0)
            {
                col = Color.Get(-1, 555, 555, 555);
            }
            screen.Render(xo + 0, yo + 0, xt + yt * 32, col, 0);
            screen.Render(xo + 8, yo + 0, xt + 1 + yt * 32, col, 0);
            screen.Render(xo + 0, yo + 8, xt + (yt + 1) * 32, col, 0);
            screen.Render(xo + 8, yo + 8, xt + 1 + (yt + 1) * 32, col, 0);
        }

        public override void TouchedBy(Entity entity)
        {
            if ((entity is Player))
            {
                if (_fuseTime == 0)
                {
                    Sound.PlaySound("fuse");
                    _fuseTime = _maxFuseTime;
                    _fuseLit = true;
                }
                entity.Hurt(this, 1, Dir);
            }
        }

        protected override void Die()
        {
            base.Die();
            if (Level.Player != null)
            {
                Level.Player.Score += 50 * _lvl;
            }
        }
    }
}
