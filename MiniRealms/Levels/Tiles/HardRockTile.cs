﻿using MiniRealms.Engine;
using MiniRealms.Engine.Gfx;
using MiniRealms.Entities;
using MiniRealms.Entities.Particles;
using MiniRealms.Items;
using MiniRealms.Items.Resources;

namespace MiniRealms.Levels.Tiles
{

    public class HardRockTile : Tile
    {
        public HardRockTile(TileId id)
            : base(id)
        {
        }

        public override void Render(Screen screen, Level level, int x, int y)
        {
            int col = Color.Get(334, 334, 223, 223);
            int transitionColor = Color.Get(001, 334, 445, level.DirtColor);

            bool u = level.GetTile(x, y - 1) != this;
            bool d = level.GetTile(x, y + 1) != this;
            bool l = level.GetTile(x - 1, y) != this;
            bool r = level.GetTile(x + 1, y) != this;

            bool ul = level.GetTile(x - 1, y - 1) != this;
            bool dl = level.GetTile(x - 1, y + 1) != this;
            bool ur = level.GetTile(x + 1, y - 1) != this;
            bool dr = level.GetTile(x + 1, y + 1) != this;

            if (!u && !l)
            {
                if (!ul)
                    screen.Render(x * 16 + 0, y * 16 + 0, 0, col, 0);
                else
                    screen.Render(x * 16 + 0, y * 16 + 0, 7 + 0 * 32, transitionColor, 3);
            }
            else
                screen.Render(x * 16 + 0, y * 16 + 0, (l ? 6 : 5) + (u ? 2 : 1) * 32, transitionColor, 3);

            if (!u && !r)
            {
                if (!ur)
                    screen.Render(x * 16 + 8, y * 16 + 0, 1, col, 0);
                else
                    screen.Render(x * 16 + 8, y * 16 + 0, 8 + 0 * 32, transitionColor, 3);
            }
            else
                screen.Render(x * 16 + 8, y * 16 + 0, (r ? 4 : 5) + (u ? 2 : 1) * 32, transitionColor, 3);

            if (!d && !l)
            {
                if (!dl)
                    screen.Render(x * 16 + 0, y * 16 + 8, 2, col, 0);
                else
                    screen.Render(x * 16 + 0, y * 16 + 8, 7 + 1 * 32, transitionColor, 3);
            }
            else
                screen.Render(x * 16 + 0, y * 16 + 8, (l ? 6 : 5) + (d ? 0 : 1) * 32, transitionColor, 3);
            if (!d && !r)
            {
                if (!dr)
                    screen.Render(x * 16 + 8, y * 16 + 8, 3, col, 0);
                else
                    screen.Render(x * 16 + 8, y * 16 + 8, 8 + 1 * 32, transitionColor, 3);
            }
            else
                screen.Render(x * 16 + 8, y * 16 + 8, (r ? 4 : 5) + (d ? 0 : 1) * 32, transitionColor, 3);
        }

        public override bool MayPass(Level level, int x, int y, Entity e)
        {
            return false;
        }

        public override void Hurt(Level level, int x, int y, Mob source, int dmg, int attackDir)
        {
            Hurt(level, x, y, 0);
        }

        public override bool Interact(Level level, int xt, int yt, Player player, Item item, int attackDir) {
		if (item is ToolItem) {
			ToolItem tool = (ToolItem) item;
			if (tool.ObjectType == ToolType.Pickaxe && tool.Level == 4) {
				if (player.PayStamina(4 - tool.Level)) {
					Hurt(level, xt, yt, Random.NextInt(10) + (tool.Level) * 5 + 10);
					return true;
				}
			}
		}
		return false;
	}

        public void Hurt(Level level, int x, int y, int dmg)
        {
            int damage = level.GetData(x, y) + dmg;
            level.Add(new SmashParticle(x * 16 + 8, y * 16 + 8));
            level.Add(new TextParticle("" + dmg, x * 16 + 8, y * 16 + 8, Color.Get(-1, 500, 500, 500)));
            if (damage >= 200)
            {
                int count = Random.NextInt(4) + 1;
                for (int i = 0; i < count; i++)
                {
                    level.Add(new ItemEntity(new ResourceItem(Resource.Stone), x * 16 + Random.NextInt(10) + 3, y * 16 + Random.NextInt(10) + 3));
                }
                count = Random.NextInt(2);
                for (int i = 0; i < count; i++)
                {
                    level.Add(new ItemEntity(new ResourceItem(Resource.Coal), x * 16 + Random.NextInt(10) + 3, y * 16 + Random.NextInt(10) + 3));
                }
                level.SetTile(x, y, Dirt, 0);
            }
            else
            {
                level.SetData(x, y, damage);
            }
        }

        public override void Tick(Level level, int xt, int yt)
        {
            int damage = level.GetData(xt, yt);
            if (damage > 0) level.SetData(xt, yt, damage - 1);
        }
    }
}
