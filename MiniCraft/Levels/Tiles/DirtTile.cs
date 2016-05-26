using System.Collections.Generic;
using MiniRealms.Engine;
using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Engine.Gfx;
using MiniRealms.Entities;
using MiniRealms.Items;
using MiniRealms.Items.Resources;

namespace MiniRealms.Levels.Tiles
{
    public class DirtTile : Tile
    {
        private readonly Sprite[] _sprites;

        public DirtTile(TileId id)
            : base(id)
        { 
            _sprites = Sprites;
        }

        protected DirtTile(TileId id, List<Sprite> parentSprits) : base(id)
        {
            _sprites = parentSprits.ToArray();
        }


        public override void Render(Screen screen, Level level, int x, int y)
        {
            int col = Color.Get(level.DirtColor, level.DirtColor, level.DirtColor - 111, level.DirtColor - 111);
            screen.Render(x * 16 + 0, y * 16 + 0, _sprites[0].Img, col, 0);
            screen.Render(x * 16 + 8, y * 16 + 0, _sprites[1].Img, col, 0);
            screen.Render(x * 16 + 0, y * 16 + 8, _sprites[2].Img, col, 0);
            screen.Render(x * 16 + 8, y * 16 + 8, _sprites[3].Img, col, 0);
        }

        public override bool Interact(Level level, int xt, int yt, Player player, Item item, int attackDir)
        {
            var toolItem = item as ToolItem;
            if (toolItem == null) return false;
            ToolItem tool = toolItem;
            if (tool.ObjectType == ToolType.Shovel && player.PayStamina(4 - tool.Level))
            {
                level.SetTile(xt, yt, Hole, 0);
                level.Add(new ItemEntity(new ResourceItem(Resource.Dirt), xt*16 + Random.NextInt(10) + 3,
                    yt*16 + Random.NextInt(10) + 3));
                SoundEffectManager.Play("monsterhurt");
                return true;
            }
            if (tool.ObjectType != ToolType.Hoe || !player.PayStamina(4 - tool.Level)) return false;
            level.SetTile(xt, yt, Farmland, 0);
            SoundEffectManager.Play("monsterhurt");
            return true;
        }
    }
}
