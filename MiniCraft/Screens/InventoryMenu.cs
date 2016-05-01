using MiniCraft.Entities;
using MiniCraft.Gfx;
using MiniCraft.Items;

namespace MiniCraft.Screens
{
    public class InventoryMenu : Menu
    {
        private readonly Player _player;
        private int _selected;

        public InventoryMenu(Player player)
        {
            _player = player;

            if (player.ActiveItem == null) return;
            player.Inventory.Items.Add(0, player.ActiveItem);
            player.ActiveItem = null;
        }

        public override void Tick()
        {
            if (Input.Menu.Clicked) Game.SetMenu(null);

            if (Input.Up.Clicked) _selected--;
            if (Input.Down.Clicked) _selected++;

            int len = _player.Inventory.Items.Size();
            if (len == 0) _selected = 0;
            if (_selected < 0) _selected += len;
            if (_selected >= len) _selected -= len;

            if (!Input.Attack.Clicked || len <= 0) return;
            Item item = _player.Inventory.Items.Remove(_selected);
            _player.ActiveItem = item;
            Game.SetMenu(null);
        }

        public override void Render(Screen screen)
        {
            Font.RenderFrame(screen, "inventory", 1, 1, 12, 11);
            RenderItemList(screen, 1, 1, 12, 11, _player.Inventory.Items, _selected);
        }
    }
}
