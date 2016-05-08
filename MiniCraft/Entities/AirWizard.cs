using System;
using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Engine.Gfx;

namespace MiniRealms.Entities
{

    public class AirWizard : Mob
    {
        private int _xa, _ya;
        private int _randomWalkTime;
        private int _attackDelay;
        private int _attackTime;
        private int _attackType;

        public AirWizard()
        {
            X = Random.NextInt(64 * 16);
            Y = Random.NextInt(64 * 16);
            Health = MaxHealth = 2000;
        }

        public override void Tick()
        {
            base.Tick();

            if (_attackDelay > 0)
            {
                Dir = (_attackDelay - 45) / 4 % 4;
                Dir = (Dir * 2 % 4) + (Dir / 2);
                if (_attackDelay < 45)
                {
                    Dir = 0;
                }
                _attackDelay--;
                if (_attackDelay == 0)
                {
                    _attackType = 0;
                    if (Health < 1000) _attackType = 1;
                    if (Health < 200) _attackType = 2;
                    _attackTime = 60 * 2;
                }
                return;
            }

            if (_attackTime > 0)
            {
                _attackTime--;
                double dir = _attackTime * 0.25 * (_attackTime % 2 * 2 - 1);
                double speed = (0.7) + _attackType * 0.2;
                Level.Add(new Spark(this, Math.Cos(dir) * speed, Math.Sin(dir) * speed));
                return;
            }

            if (Level.Player != null && _randomWalkTime == 0)
            {
                int xd = Level.Player.X - X;
                int yd = Level.Player.Y - Y;
                if (xd * xd + yd * yd < 32 * 32)
                {
                    _xa = 0;
                    _ya = 0;
                    if (xd < 0) _xa = +1;
                    if (xd > 0) _xa = -1;
                    if (yd < 0) _ya = +1;
                    if (yd > 0) _ya = -1;
                }
                else if (xd * xd + yd * yd > 80 * 80)
                {
                    _xa = 0;
                    _ya = 0;
                    if (xd < 0) _xa = -1;
                    if (xd > 0) _xa = +1;
                    if (yd < 0) _ya = -1;
                    if (yd > 0) _ya = +1;
                }
            }
            {
                int speed = (TickTime % 4) == 0 ? 0 : 1;
                if (!Move(_xa * speed, _ya * speed) || Random.NextInt(100) == 0)
                {
                    _randomWalkTime = 30;
                    _xa = (Random.NextInt(3) - 1);
                    _ya = (Random.NextInt(3) - 1);
                }
                if (_randomWalkTime > 0)
                {
                    _randomWalkTime--;
                    if (Level.Player != null && _randomWalkTime == 0)
                    {
                        int xd = Level.Player.X - X;
                        int yd = Level.Player.Y - Y;
                        if (Random.NextInt(4) == 0 && xd * xd + yd * yd < 50 * 50)
                        {
                            if (_attackDelay == 0 && _attackTime == 0)
                            {
                                _attackDelay = 60 * 2;
                            }
                        }
                    }
                }
            }
        }

        protected override void DoHurt(int damage, int attackDir)
        {
            base.DoHurt(damage, attackDir);
            if (_attackDelay == 0 && _attackTime == 0)
            {
                _attackDelay = 60 * 2;
            }
        }

        public override void Render(Screen screen)
        {
            int xt = 8;
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

            int col1 = Color.Get(-1, 100, 500, 555);
            int col2 = Color.Get(-1, 100, 500, 532);
            if (Health < 200)
            {
                if (TickTime / 3 % 2 == 0)
                {
                    col1 = Color.Get(-1, 500, 100, 555);
                    col2 = Color.Get(-1, 500, 100, 532);
                }
            }
            else if (Health < 1000)
            {
                if (TickTime / 5 % 4 == 0)
                {
                    col1 = Color.Get(-1, 500, 100, 555);
                    col2 = Color.Get(-1, 500, 100, 532);
                }
            }
            if (HurtTime > 0)
            {
                col1 = Color.Get(-1, 555, 555, 555);
                col2 = Color.Get(-1, 555, 555, 555);
            }

            screen.Render(xo + 8 * flip1, yo + 0, xt + yt * 32, col1, flip1);
            screen.Render(xo + 8 - 8 * flip1, yo + 0, xt + 1 + yt * 32, col1, flip1);
            screen.Render(xo + 8 * flip2, yo + 8, xt + (yt + 1) * 32, col2, flip2);
            screen.Render(xo + 8 - 8 * flip2, yo + 8, xt + 1 + (yt + 1) * 32, col2, flip2);
        }

        public override void TouchedBy(Entity entity)
        {
            var player = entity as Player;
            if (player == null) return;
            entity.Hurt(this, 3, Dir);
        }

        protected override void Die()
        {
            base.Die();
            if (Level.Player != null)
            {
                Level.Player.Score += 1000;
                Level.Player.GameWon();
            }
            SoundManager.Play("bossdeath");
        }

    }
}
