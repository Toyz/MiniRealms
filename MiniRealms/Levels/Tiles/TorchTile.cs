using MiniRealms.Engine;
using MiniRealms.Engine.Gfx;
using MiniRealms.Entities;
using MiniRealms.Items;
using MiniRealms.Items.Resources;

namespace MiniRealms.Levels.Tiles
{
    class TorchTile : DirtTile
    {
        private int _color = Color.Get(-1, 10, 531, 555);

        public TorchTile(TileId tileId) : base(tileId, SpriteSheet.GetSprites(TileId.Dirt))
        {
            Tiles[(byte)tileId] = this;
            ConnectsToGrass = true;
            ConnectsToSand = true;
        }

        public override void Render(Screen screen, Level level, int x, int y)
        {
            base.Render(screen, level, x, y);

            screen.Render(x * 16 + 0, y * 16 + 0, 6 + 5 * 32, _color, 0);
        }

        public override void Tick(Level level, int xt, int yt)
        {
            base.Tick(level, xt, yt);

            _color = Random.Nextbool() ? Color.Get(-1, 10, 531, 555) : Color.Get(-1, 10, 531, 550);
        }

        public override bool Interact(Level level, int x, int y, Player player, Item item, int attackDir)
        {
            var toolItem = item as PowerGloveItem;
            if (toolItem == null) return false;
            if (!player.PayStamina(1)) return false;
            level.Add(new ItemEntity(new ResourceItem(Resource.Torch), x * 16 + Random.NextInt(10) + 3, y * 16 + Random.NextInt(10) + 3));
            level.SetTile(x, y, Dirt, 0);
            return true;
        }

        public override int GetLightRadius(Level level, int x, int y) => 5;
    }
}
