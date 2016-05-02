using MiniCraft.Entities.Particles;
using MiniCraft.Gfx;
using MiniCraft.Levels;
using MiniCraft.Levels.Tiles;
using MiniCraft.Sounds;

namespace MiniCraft.Entities
{
    public class Mob : Entity
    {
        protected int WalkDist;

        public int Dir { get; protected set; }
        public int HurtTime;
        protected int XKnockback, YKnockback;
        public int MaxHealth = 10;
        public int Health = 10;
        public int SwimTimer;
        public int TickTime;

        public Mob()
        {
            X = Y = 8;
            Xr = 4;
            Yr = 3;
        }

        public override void Tick()
        {
            TickTime++;
            if (Level.GetTile(X >> 4, Y >> 4) == Tile.Lava)
            {
                Hurt(this, 4, Dir ^ 1);
            }

            if (Health <= 0)
            {
                Die();
            }
            if (HurtTime > 0) HurtTime--;
        }

        protected virtual void Die()
        {
            Remove();
        }

        public override bool Move(int xa, int ya)
        {
            if (IsSwimming())
            {
                if (SwimTimer++ % 2 == 0) return true;
            }
            if (XKnockback < 0)
            {
                Move2(-1, 0);
                XKnockback++;
            }
            if (XKnockback > 0)
            {
                Move2(1, 0);
                XKnockback--;
            }
            if (YKnockback < 0)
            {
                Move2(0, -1);
                YKnockback++;
            }
            if (YKnockback > 0)
            {
                Move2(0, 1);
                YKnockback--;
            }
            if (HurtTime > 0) return true;
            if (xa == 0 && ya == 0) return base.Move(xa, ya);
            WalkDist++;
            if (xa < 0) Dir = 2;
            if (xa > 0) Dir = 3;
            if (ya < 0) Dir = 1;
            if (ya > 0) Dir = 0;
            return base.Move(xa, ya);
        }

        protected bool IsSwimming()
        {
            Tile tile = Level.GetTile(X >> 4, Y >> 4);
            return tile == Tile.Water || tile == Tile.Lava;
        }

        public override bool Blocks(Entity e) => e.IsBlockableBy(this);

        public override void Hurt(Tile tile, int x, int y, int damage)
        {
            int attackDir = Dir ^ 1;
            DoHurt(damage, attackDir);
        }

        public override void Hurt(Mob mob, int damage, int attackDir)
        {
            DoHurt(damage, attackDir);
        }

        public virtual void Heal(int heal)
        {
            if (HurtTime > 0) return;

            Level.Add(new TextParticle("" + heal, X, Y, Color.Get(-1, 50, 50, 50)));
            Health += heal;
            if (Health > MaxHealth) Health = MaxHealth;
        }

        protected virtual void DoHurt(int damage, int attackDir)
        {
            if (HurtTime > 0) return;

            if (Level.Player != null)
            {
                int xd = Level.Player.X - X;
                int yd = Level.Player.Y - Y;
                if (xd * xd + yd * yd < 80 * 80)
                {
                    Sound.MonsterHurt.Play();
                }
            }
            Level.Add(new TextParticle("" + damage, X, Y, Color.Get(-1, 500, 500, 500)));
            Health -= damage;
            if (attackDir == 0) YKnockback = +6;
            if (attackDir == 1) YKnockback = -6;
            if (attackDir == 2) XKnockback = -6;
            if (attackDir == 3) XKnockback = +6;
            HurtTime = 10;
        }

        public virtual bool FindStartPos(Level level)
        {
            int x = Random.NextInt(level.W);
            int y = Random.NextInt(level.H);
            int xx = x * 16 + 8;
            int yy = y * 16 + 8;

            if (level.Player != null)
            {
                int xd = level.Player.X - xx;
                int yd = level.Player.Y - yy;
                if (xd * xd + yd * yd < 80 * 80) return false;
            }

            int r = level.MonsterDensity * 16;
            if (level.GetEntities(xx - r, yy - r, xx + r, yy + r).Size() > 0) return false;

            if (!level.GetTile(x, y).MayPass(level, x, y, this)) return false;
            X = xx;
            Y = yy;
            return true;
        }
    }

}
