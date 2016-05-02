using System;
using System.Collections.Generic;
using MiniCraft.Gfx;
using MiniCraft.Items;
using MiniCraft.Levels;
using MiniCraft.Levels.Tiles;

namespace MiniCraft.Entities
{
    public class Entity
    {
        protected readonly Random Random = new Random();
        public int X, Y;
        public int Xr = 6;
        public int Yr = 6;
        public bool Removed;
        public Level Level;

        public virtual void Render(Screen screen)
        {
        }

        public virtual void Tick()
        {
        }

        public virtual void Remove()
        {
            Removed = true;
        }

        public virtual void Init(Level level)
        {
            Level = level;
        }

        public virtual bool Intersects(int x0, int y0, int x1, int y1) => !(X + Xr < x0 || Y + Yr < y0 || X - Xr > x1 || Y - Yr > y1);

        public virtual bool Blocks(Entity e) => false;

        public virtual void Hurt(Mob mob, int dmg, int attackDir)
        {
        }

        public virtual void Hurt(Tile tile, int x, int y, int dmg)
        {
        }

        public virtual bool Move(int xa, int ya)
        {
            if (xa == 0 && ya == 0) return true;
            var stopped = !(xa != 0 && Move2(xa, 0));
            if (ya != 0 && Move2(0, ya)) stopped = false;
            if (stopped) return false;
            int xt = X >> 4;
            int yt = Y >> 4;
            Level.GetTile(xt, yt).SteppedOn(Level, xt, yt, this);
            return true;
        }

        protected virtual bool Move2(int xa, int ya)
        {
            if (xa != 0 && ya != 0) throw new InvalidOperationException("Move2 can only move along one axis at a time!");

            int xto0 = (X - Xr) >> 4;
            int yto0 = (Y - Yr) >> 4;
            int xto1 = (X + Xr) >> 4;
            int yto1 = (Y + Yr) >> 4;

            int xt0 = (X + xa - Xr) >> 4;
            int yt0 = (Y + ya - Yr) >> 4;
            int xt1 = (X + xa + Xr) >> 4;
            int yt1 = (Y + ya + Yr) >> 4;
            for (int yt = yt0; yt <= yt1; yt++)
                for (int xt = xt0; xt <= xt1; xt++)
                {
                    if (xt >= xto0 && xt <= xto1 && yt >= yto0 && yt <= yto1) continue;
                    Level.GetTile(xt, yt).BumpedInto(Level, xt, yt, this);
                    if (Level.GetTile(xt, yt).MayPass(Level, xt, yt, this)) continue;
                    return false;
                }
            List<Entity> wasInside = Level.GetEntities(X - Xr, Y - Yr, X + Xr, Y + Yr);
            List<Entity> isInside = Level.GetEntities(X + xa - Xr, Y + ya - Yr, X + xa + Xr, Y + ya + Yr);
            foreach (var e in isInside)
            {
                if (e != this)
                    e.TouchedBy(this);
            }
            isInside.RemoveAll(wasInside);
            for (int i = 0; i < isInside.Count; i++)
            {
                var e = isInside.Get(i);
                if (e == this) continue;

                if (!e.Blocks(this)) continue;
                return false;
            }

            X += xa;
            Y += ya;
            return true;
        }

        //TODO: was protected
        public virtual void TouchedBy(Entity entity)
        {
        }

        public virtual bool IsBlockableBy(Mob mob) => true;

        public virtual void TouchItem(ItemEntity itemEntity)
        {
        }

        public virtual bool CanSwim() => false;

        public virtual bool Interact(Player player, Item item, int attackDir) => item.Interact(player, this, attackDir);

        public virtual bool Use(Player player, int attackDir) => false;

        public virtual int GetLightRadius() => 0;
    }
}
