using System.Diagnostics;
using MiniRealms.Engine;
using MiniRealms.Engine.Gfx;
using MiniRealms.Items;
using MiniRealms.Items.Resources;

namespace MiniRealms.Entities
{

    public class Zombie : Mob
    {
        private int _xa, _ya;
        private readonly int _lvl;
        private int _randomWalkTime;
        private readonly int _shirtColor;
        private readonly int _glowColor;
        private readonly int _defaultGlowColor = 10;
        private readonly int _dmg;
        private readonly bool _isBossMob;

        public Zombie(int lvl)
        {
            _lvl = lvl;

            X = Random.NextInt(64 * 16);
            Y = Random.NextInt(64 * 16);
            int hpLevel = _lvl - McGame.Difficulty.BaseLevel;

            Health = MaxHealth = hpLevel * hpLevel * 10;
            _shirtColor = Random.Next(1, 555) + 1;
            _glowColor = Random.NextInt(McGame.Difficulty.BossMobSpawnRate + 1) >= McGame.Difficulty.BossMobSpawnRate ? Random.Next(10, 555) - 1 : 10;

            _dmg = lvl + 1;

            if (_glowColor != _defaultGlowColor)
            {
                MaxHealth = Random.Next(10, 50);
                Health = MaxHealth = hpLevel * hpLevel * 10 + MaxHealth;

                _dmg = lvl + Random.NextInt(3);

                _isBossMob = true;
            }
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

            int col = Color.Get(-1, _glowColor, _shirtColor, 050);//Color.Get(-1, 10, 252, 050);
            var baseLevel = _lvl - McGame.Difficulty.BaseLevel;

            if (baseLevel == 2) col = Color.Get(-1, _glowColor, _shirtColor, 050);
            if (baseLevel == 3) col = Color.Get(-1, _glowColor, _shirtColor, 050);
            if (baseLevel == 4) col = Color.Get(-1, _glowColor, _shirtColor, 020);
            if (HurtTime > 0)
            {
                col = Color.Get(-1, 555, 555, 555);
            }

            screen.Render(xo + 8 * flip1, yo + 0, xt + yt * 32, col, flip1);
            screen.Render(xo + 8 - 8 * flip1, yo + 0, xt + 1 + yt * 32, col, flip1);
            screen.Render(xo + 8 * flip2, yo + 8, xt + (yt + 1) * 32, col, flip2);
            screen.Render(xo + 8 - 8 * flip2, yo + 8, xt + 1 + (yt + 1) * 32, col, flip2);
        }

        public override void TouchedBy(Entity entity)
        {
            var player = entity as Player;
            if (player == null) return;
            Debug.WriteLine(Health);
            entity.Hurt(this, _dmg, Dir);
        }

        protected override void Die()
        {
            base.Die();

            int count = Random.Next(3) + 1;
            for (int i = 0; i < count; i++)
            {
                Level.Add(new ItemEntity(new ResourceItem(Resource.Cloth), X + Random.NextInt(11) - 5,
                    Y + Random.NextInt(11) - 5));
            }

            if (_isBossMob)
            {
                count = Random.Next(3);
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        Level.Add(new ItemEntity(new ResourceItem(Resource.Gem), X + Random.NextInt(11) - 5,
                            Y + Random.NextInt(11) - 5));
                    }
                }
            }

            if (Level.Player != null)
            {
                Level.Player.Score += 50*_lvl;
            }

        }

    }
}
