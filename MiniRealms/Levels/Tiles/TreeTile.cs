using MiniRealms.Engine;
using MiniRealms.Engine.Gfx;
using MiniRealms.Entities;
using MiniRealms.Entities.Particles;
using MiniRealms.Items;
using MiniRealms.Items.Resources;

namespace MiniRealms.Levels.Tiles
{

    public sealed class TreeTile : GrassTile
    {
        private readonly int _color;
        private readonly int _barkColor1;
        private readonly int _barkColor2;
        private readonly Sprite[] _ssprites;

        public TreeTile(TileId id, int color, int barkColor1, int barkColor2)
            : base(id, SpriteSheet.GetSprites(TileId.Grass))
        {
            _ssprites = SpriteSheet.GetSprites(TileId.Tree);

            _color = color;
            _barkColor1 = barkColor1;
            _barkColor2 = barkColor2;

            TileColor = color + barkColor1 + barkColor2;
        }

        public override void Render(Screen screen, Level level, int x, int y)
        {
            int col = _color;//Color.Get(10, 40, 151, 141);
            int barkCol1 = _barkColor1;//Color.Get(10, 40, 430, 141);
            int barkCol2 = _barkColor2;//Color.Get(10, 40, 320, 141);

            bool u = level.GetTile(x, y - 1).Id == Id;
            bool l = level.GetTile(x - 1, y).Id == Id;
            bool r = level.GetTile(x + 1, y).Id == Id;
            bool d = level.GetTile(x, y + 1).Id == Id;
            bool ul = level.GetTile(x - 1, y - 1).Id == Id;
            bool ur = level.GetTile(x + 1, y - 1).Id == Id;
            bool dl = level.GetTile(x - 1, y + 1).Id == Id;
            bool dr = level.GetTile(x + 1, y + 1).Id == Id;

            if (u && ul && l)
            {
                //screen.Render(x * 16 + 0, y * 16 + 0, 10 + 1 * 32, col, 0);
                screen.Render(x * 16 + 0, y * 16 + 0, _ssprites[1].Img, col, 0);
            }
            else
            {
                //screen.Render(x * 16 + 0, y * 16 + 0, 9 + 0 * 32, col, 0);
                screen.Render(x * 16 + 0, y * 16 + 0, _ssprites[4].Img, col, 0);
            }
            if (u && ur && r)
            {
                //screen.Render(x * 16 + 8, y * 16 + 0, 10 + 2 * 32, barkCol2, 0);
                screen.Render(x * 16 + 8, y * 16 + 0, _ssprites[2].Img, barkCol2, 0);
            }
            else
            {
                //screen.Render(x * 16 + 8, y * 16 + 0, 10 + 0 * 32, col, 0);
                screen.Render(x * 16 + 8, y * 16 + 0, _ssprites[0].Img, col, 0);
            }
            if (d && dl && l)
            {
                //screen.Render(x * 16 + 0, y * 16 + 8, 10 + 2 * 32, barkCol2, 0);
                screen.Render(x * 16 + 0, y * 16 + 8, _ssprites[2].Img, barkCol2, 0);
            }
            else
            {
                //screen.Render(x * 16 + 0, y * 16 + 8, 9 + 1 * 32, barkCol1, 0);
                screen.Render(x * 16 + 0, y * 16 + 8, _ssprites[5].Img, barkCol1, 0);
            }
            if (d && dr && r)
            {
                //screen.Render(x * 16 + 8, y * 16 + 8, 10 + 1 * 32, col, 0);
                screen.Render(x * 16 + 8, y * 16 + 8, _ssprites[1].Img, col, 0);
            }
            else
            {
                //screen.Render(x * 16 + 8, y * 16 + 8, 10 + 3 * 32, barkCol2, 0);
                screen.Render(x * 16 + 8, y * 16 + 8, _ssprites[3].Img, barkCol2, 0);
            }
        }

        public override void Tick(Level level, int xt, int yt)
        {
            int damage = level.GetData(xt, yt);
            if (damage > 0) level.SetData(xt, yt, damage - 1);
        }

        public override bool MayPass(Level level, int x, int y, Entity e) => false;

        public override void Hurt(Level level, int x, int y, Mob source, int dmg, int attackDir)
        {
            Hurt(level, x, y, dmg);
        }

        public override bool Interact(Level level, int xt, int yt, Player player, Item item, int attackDir)
        {
            var toolItem = item as ToolItem;
            if (toolItem == null) return false;
            ToolItem tool = toolItem;
            if (tool.ObjectType == ToolType.Axe)
            {
                if (player.PayStamina(4 - tool.Level))
                {
                    Hurt(level, xt, yt, Random.NextInt(10) + (tool.Level) * 5 + 10);
                    return true;
                }
            }
            return false;
        }

        private void Hurt(Level level, int x, int y, int dmg)
        {
            int count = Random.NextInt(10) == 0 ? 1 : 0;
            for (var i = 0; i < count; i++)
            {
                level.Add(new ItemEntity(new ResourceItem(Resource.Apple), x*16 + Random.NextInt(10) + 3,
                    y*16 + Random.NextInt(10) + 3));
            }

            int damage = level.GetData(x, y) + dmg;
            level.Add(new SmashParticle(x*16 + 8, y*16 + 8));
            level.Add(new TextParticle("" + dmg, x*16 + 8, y*16 + 8, Color.Get(-1, 500, 500, 500)));
            if (damage >= 20)
            {
                count = Random.NextInt(2) + 1;
                for (int i = 0; i < count; i++)
                {
                    level.Add(new ItemEntity(new ResourceItem(Resource.Wood), x*16 + Random.NextInt(10) + 3,
                        y*16 + Random.NextInt(10) + 3));
                }
                count = Random.NextInt(Random.NextInt(4) + 1);
                for (int i = 0; i < count; i++)
                {
                    level.Add(new ItemEntity(new ResourceItem(Resource.Acorn), x*16 + Random.NextInt(10) + 3,
                        y*16 + Random.NextInt(10) + 3));
                }

                if (Id == (byte)TileId.SkyTree)
                {
                    count = Random.NextInt(5);
                    if (count == 4)
                    {
                        level.Add(new ItemEntity(new ResourceItem(Resource.Gem), x*16 + Random.NextInt(10) + 3,
                                y*16 + Random.NextInt(10) + 3));
                    }
                }
                level.SetTile(x, y, Grass, 0);
            }
            else
            {
                level.SetData(x, y, damage);
            }
        }
    }
}
