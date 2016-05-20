using System.Collections.Generic;
using System.Linq;
using MiniRealms.Engine;
using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.Interfaces;

namespace MiniRealms.Screens.UIMenus
{
    public class ScrollingMenu : Menu
    {
        private int _selected;
        private int _selectedItem;
        protected int MaxToShow = 15;

        private static List<Option> _options;
        private List<Option> _visible = new List<Option>();

        protected ScrollingMenu(Menu parent) : base(parent)
        {
        }

        protected void RenderScrollingListTable(List<Option> options)
        {
            _options = options;
            _visible = _options.Page(1, MaxToShow).ToList();
        }

        public override void Tick()
        {
            var index = _options.IndexOf(_options[_selected]);

            if (Input.Up.Clicked)
            {
                SoundEffectManager.Play("menu_move");
                _selected--;
                _selectedItem--;

                if (_selected < 0)
                {
                    _selected = 0;
                }

                if (_selectedItem < 0)
                {
                    var item = _options.Skip(_selected).Take(1).FirstOrDefault();

                    if (item != null)
                    {
                        if (_options.IndexOf(item) != index)
                        {
                            _visible.RemoveAt(_visible.Count - 1);
                            _visible.Insert(0, item);
                        }
                        _selectedItem = 0;
                    }
                }
            }

            if (Input.Down.Clicked)
            {
                SoundEffectManager.Play("menu_move");
                _selected++;
                _selectedItem++;

                if (_selected > _options.Count - 1)
                {
                    _selected = _options.Count - 1;
                }

                if (_selectedItem > MaxToShow - 1)
                {
                    var item = _options.Skip(_selected).Take(1).FirstOrDefault();

                    if (item != null)
                    {
                        if (_options.IndexOf(item) != index)
                        {
                            _visible.RemoveAt(0);
                            _visible.Insert(_visible.Count, item);
                        }
                        _selectedItem = _visible.Count - 1;
                    }
                }

            }


            _options[_selected].HandleInput(Input);
        }

        public override void Render(Screen screen)
        {
            screen.Clear(0);

            for (var i = 0; i < _visible.Count; i++)
            {
                var option = _visible[i];
                var index = _options.IndexOf(option);

                var msg = option.Text;
                var col = Color.DarkGrey;
                if (index == _selected)
                {
                    msg = option.SelectedText;
                    col = option.SelectedColor;
                    option.HandleRender();
                }
                Font.Draw(msg, screen, (screen.W - msg.Length*8)/2, (GameConts.ScreenMiddleHeight + (i*10) - ((_visible.Count*8)/2)), col);
            }
        }
    }
}
