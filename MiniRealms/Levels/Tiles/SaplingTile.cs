using System.Collections.Generic;
using MiniRealms.Engine.Gfx;
using MiniRealms.Entities;

namespace MiniRealms.Levels.Tiles
{
    public class SaplingTile : Tile
    {
        private readonly Tile _onType;
        private readonly Tile _growsTo;

        public SaplingTile(TileId id, Tile onType, Tile growsTo)
            : base(id)
        {
            _onType = onType;
            _growsTo = growsTo;
            ConnectsToSand = onType.ConnectsToSand;
            ConnectsToGrass = onType.ConnectsToGrass;
            ConnectsToWater = onType.ConnectsToWater;
            ConnectsToLava = onType.ConnectsToLava;
        }

        public override void Render(Screen screen, Level level, int x, int y)
        {
            _onType.Render(screen, level, x, y);
            int col = Color.Get(10, 40, 50, -1);
            //screen.Render(x * 16 + 4, y * 16 + 4, 11 + 3 * 32, col, 0);
            screen.Render(x * 16 + 4, y * 16 + 4, Sprites[0].Img, col, 0);
        }

        public override void Tick(Level level, int x, int y)
        {
            int age = level.GetData(x, y) + 1;
            if (age > 100)
            {
                level.SetTile(x, y, _growsTo, 0);
            }
            else
            {
                level.SetData(x, y, age);
            }
        }

        public override void Hurt(Level level, int x, int y, Mob source, int dmg, int attackDir) => level.SetTile(x, y, _onType, 0);
    }
}
