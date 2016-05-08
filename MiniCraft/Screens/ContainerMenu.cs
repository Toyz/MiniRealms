using MiniRealms.Engine.Gfx;
using MiniRealms.Entities;

namespace MiniRealms.Screens
{
    public class ContainerMenu : Menu
    {
        private readonly Player _player;
        private readonly Inventory _container;
        private int _selected;
        private readonly string _title;
        private int _oSelected;
        private int _window;

        public ContainerMenu(Player player, string title, Inventory container)
        {
            _player = player;
            _title = title;
            _container = container;
        }

        public override void Tick()
        {
            if (Input.Menu.Clicked) Game.SetMenu(null);

            if (Input.Left.Clicked)
            {
                _window = 0;
                int tmp = _selected;
                _selected = _oSelected;
                _oSelected = tmp;
            }
            if (Input.Right.Clicked)
            {
                _window = 1;
                int tmp = _selected;
                _selected = _oSelected;
                _oSelected = tmp;
            }

            Inventory i = _window == 1 ? _player.Inventory : _container;
            Inventory i2 = _window == 0 ? _player.Inventory : _container;

            int len = i.Items.Size();
            if (_selected < 0) _selected = 0;
            if (_selected >= len) _selected = len - 1;

            if (Input.Up.Clicked) _selected--;
            if (Input.Down.Clicked) _selected++;

            if (len == 0) _selected = 0;
            if (_selected < 0) _selected += len;
            if (_selected >= len) _selected -= len;

            if (!Input.Attack.Clicked || len <= 0) return;
            i2.Add(_oSelected, i.Items.Remove(_selected));
            if (_selected >= i.Items.Size()) _selected = i.Items.Size() - 1;
        }

        public override void Render(Screen screen)
        {
            Font.RenderFrame(screen, _title, 1, 1, 14, 11);
            RenderItemList(screen, 1, 1, 14, 11, _container.Items, _window == 0 ? _selected : -_oSelected - 1);

            Font.RenderFrame(screen, "inventory", 15, 1, 13 + 15, 11);
            RenderItemList(screen, 15, 1, 13 + 15, 11, _player.Inventory.Items, _window == 1 ? _selected : -_oSelected - 1);
            screen.SetOffset(0, 0);
        }
    }
}
