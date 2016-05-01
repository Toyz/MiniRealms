using MiniCraft.Gfx;
using MiniCraft.Items;

namespace MiniCraft.Entities
{
    public class Furniture : Entity
    {
        private int _pushTime;
        private int _pushDir = -1;
        public int Col, Sprite;
        public string Name;
        private Player _shouldTake;

        public Furniture(string name)
        {
            Name = name;
            Xr = 3;
            Yr = 3;
        }

        public override void Tick()
        {
            if (_shouldTake != null)
            {
                if (_shouldTake.ActiveItem is PowerGloveItem)
                {
                    Remove();
                    _shouldTake.Inventory.Add(0, _shouldTake.ActiveItem);
                    _shouldTake.ActiveItem = new FurnitureItem(this);
                }
                _shouldTake = null;
            }
            if (_pushDir == 0) Move(0, +1);
            if (_pushDir == 1) Move(0, -1);
            if (_pushDir == 2) Move(-1, 0);
            if (_pushDir == 3) Move(+1, 0);
            _pushDir = -1;
            if (_pushTime > 0) _pushTime--;
        }

        public override void Render(Screen screen)
        {
            screen.Render(X - 8, Y - 8 - 4, Sprite * 2 + 8 * 32, Col, 0);
            screen.Render(X - 0, Y - 8 - 4, Sprite * 2 + 8 * 32 + 1, Col, 0);
            screen.Render(X - 8, Y - 0 - 4, Sprite * 2 + 8 * 32 + 32, Col, 0);
            screen.Render(X - 0, Y - 0 - 4, Sprite * 2 + 8 * 32 + 33, Col, 0);
        }

        public override bool Blocks(Entity e)
        {
            return true;
        }

        public override void TouchedBy(Entity entity)
        {
            var player = entity as Player;
            if (player == null || _pushTime != 0) return;
            _pushDir = player.Dir;
            _pushTime = 10;
        }

        public void Take(Player player)
        {
            _shouldTake = player;
        }
    }
}
