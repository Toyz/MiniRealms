using System;
using MiniRealms.Entities;
using MiniRealms.Gfx;

namespace MiniRealms.Levels.Tiles
{

    public class LavaTile : Tile
    {
        public LavaTile(TileId id)
            : base(id)
        {
            ConnectsToSand = true;
            ConnectsToLava = true;
        }

        private Random _wRandom = new Random();

        public override void Render(Screen screen, Level level, int x, int y)
        {
            _wRandom = new Random((int)((TickCount + (x / 2 - y) * 4311) / 10 * 54687121L + x * 3271612L + y * 3412987161L));
            //TODO: wRandom.setSeed((tickCount + (x / 2 - y) * 4311) / 10 * 54687121L + x * 3271612L + y * 3412987161L);
            int col = Color.Get(500, 500, 520, 550);
            int transitionColor1 = Color.Get(3, 500, level.DirtColor - 111, level.DirtColor);
            int transitionColor2 = Color.Get(3, 500, level.SandColor - 110, level.SandColor);

            bool u = !level.GetTile(x, y - 1).ConnectsToLava;
            bool d = !level.GetTile(x, y + 1).ConnectsToLava;
            bool l = !level.GetTile(x - 1, y).ConnectsToLava;
            bool r = !level.GetTile(x + 1, y).ConnectsToLava;

            bool su = u && level.GetTile(x, y - 1).ConnectsToSand;
            bool sd = d && level.GetTile(x, y + 1).ConnectsToSand;
            bool sl = l && level.GetTile(x - 1, y).ConnectsToSand;
            bool sr = r && level.GetTile(x + 1, y).ConnectsToSand;

            if (!u && !l)
            {
                screen.Render(x * 16 + 0, y * 16 + 0, _wRandom.NextInt(4), col, _wRandom.NextInt(4));
            }
            else
                screen.Render(x * 16 + 0, y * 16 + 0, (l ? 14 : 15) + (u ? 0 : 1) * 32, (su || sl) ? transitionColor2 : transitionColor1, 0);

            if (!u && !r)
            {
                screen.Render(x * 16 + 8, y * 16 + 0, _wRandom.NextInt(4), col, _wRandom.NextInt(4));
            }
            else
                screen.Render(x * 16 + 8, y * 16 + 0, (r ? 16 : 15) + (u ? 0 : 1) * 32, (su || sr) ? transitionColor2 : transitionColor1, 0);

            if (!d && !l)
            {
                screen.Render(x * 16 + 0, y * 16 + 8, _wRandom.NextInt(4), col, _wRandom.NextInt(4));
            }
            else
                screen.Render(x * 16 + 0, y * 16 + 8, (l ? 14 : 15) + (d ? 2 : 1) * 32, (sd || sl) ? transitionColor2 : transitionColor1, 0);
            if (!d && !r)
            {
                screen.Render(x * 16 + 8, y * 16 + 8, _wRandom.NextInt(4), col, _wRandom.NextInt(4));
            }
            else
                screen.Render(x * 16 + 8, y * 16 + 8, (r ? 16 : 15) + (d ? 2 : 1) * 32, (sd || sr) ? transitionColor2 : transitionColor1, 0);
        }

        public override bool MayPass(Level level, int x, int y, Entity e) => e.CanSwim();

        public override void Tick(Level level, int xt, int yt)
        {
            int xn = xt;
            int yn = yt;

            if (Random.Nextbool())
                xn += Random.NextInt(2) * 2 - 1;
            else
                yn += Random.NextInt(2) * 2 - 1;

            if (level.GetTile(xn, yn) == Hole)
            {
                level.SetTile(xn, yn, this, 0);
            }
        }

        public override int GetLightRadius(Level level, int x, int y) => 6;
    }
}
