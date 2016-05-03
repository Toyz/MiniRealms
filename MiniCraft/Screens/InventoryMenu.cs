using MiniRealms.Engine.Gfx;
using MiniRealms.Entities;
using MiniRealms.Items;

namespace MiniRealms.Screens
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
            Item i = _player.Inventory.Items[_selected];
            int amount = _player.Inventory.Count(i);

            Font.RenderFrame(screen, "inventory", 1, 2, 12, 11);
            Font.RenderFrame(screen, i.GetName(), 13, 2, 23, 5);

            if (amount > 99)
            {
                Font.RenderFrame(screen, "Amount", 13, 6, 23, 8);
                Font.Draw(amount.ToString("N0"), screen, 14*8, 7*8 + 2, Color.DarkGrey);
            }

            Font.Draw(i.GetName(), screen, 14 * 8, 3 * 8, Color.White);

            RenderItemList(screen, 1, 2, 12, 11, _player.Inventory.Items, _selected);
        }
    }
}
