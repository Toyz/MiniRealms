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
        protected enum Location
        {
           Left,
           Right,
           Center
        }

        private int _selected;
        private int _selectedItem;
        protected int MaxToShow = 15;
        private static List<Option> _options;
        private List<Option> _visible = new List<Option>();
        private Location _renderLocation;


        protected ScrollingMenu(Menu parent) : base(parent)
        {
        }

        protected void RenderScrollingListTable(List<Option> options, Location renderLocation = Location.Center)
        {
            _renderLocation = renderLocation;
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
                    col = option.Enabled ? option.SelectedColor : Color.DarkGrey;
                    option.HandleRender();
                }

                if (_renderLocation == Location.Center)
                {
                    Font.Draw(msg, screen, (screen.W - msg.Length*8)/2,
                        (GameConts.ScreenMiddleHeight + (i*10) - ((_visible.Count*8)/2)), col);
                }else if (_renderLocation == Location.Left)
                {
                    Font.Draw(msg, screen, 10, 
                        (GameConts.ScreenMiddleHeight + (i * 10) - ((_visible.Count * 8) / 2)), col);
                }
                else if (_renderLocation == Location.Right)
                {
                    Font.Draw(msg, screen, screen.W - (msg.Length * 8) - 10 ,
                        (GameConts.ScreenMiddleHeight + (i * 10) - ((_visible.Count * 8) / 2)), col);
                }
            }
        }

        protected void RenderLeftMenuItem(int x1, int y1, int w, int h, string msg, int color, Screen screen)
        {
            RenderLeftMenuItem(x1, y1, w, h, new []{ msg }, color, screen);
        }

        protected void RenderLeftMenuItem(int x1, int y1, int w, int h, string[] msg, int color, Screen screen, bool trans = false)
        {
            var xx = x1;
            var yy = y1;

            for(var ss = 0; ss < msg.Length; ss++)
            {
                var s = msg[ss];
                if (w > s.Length)
                {
                    var spaces = w - s.Length;
                    for (var i = 0; i < spaces; i++)
                    {
                        s += " ";
                    }

                    msg[ss] = s;
                }
            }

            screen.Render(xx - 8, yy - 8 , 0 + 13 * 32, Color.Get(!trans ? 0 : -1, 1, 5, 445), 0);
            screen.Render(xx + w * 8, yy - 8, 0 + 13 * 32, Color.Get(!trans ? 0 : -1, 1, 5, 445), 1);
            screen.Render(xx - 8, yy + 8 *h, 0 + 13 * 32, Color.Get(!trans ? 0 : -1, 1, 5, 445), 2);
            screen.Render(xx + w * 8, yy + 8 * h, 0 + 13 * 32, Color.Get(!trans ? 0 : -1, 1, 5, 445), 3);
            for (var x = 0; x < w; x++)
            {
                screen.Render(xx + x * 8, yy - 8, 1 + 13 * 32, Color.Get(!trans ? 0 : -1, 1, 5, 445), 0);
                screen.Render(xx + x * 8, yy + 8 * h, 1 + 13 * 32, Color.Get(!trans ? 0 : -1, 1, 5, 445), 2);
            }
            for (var y = 0; y < h; y++)
            {
                screen.Render(xx - 8, yy + y * 8, 2 + 13 * 32, Color.Get(!trans ? 0 : -1, 1, 5, 445), 0);
                screen.Render(xx + w * 8, yy + y * 8, 2 + 13 * 32, Color.Get(!trans ? 0 : -1, 1, 5, 445), 1);
            }

            for (var ss = 0; ss < msg.Length; ss++)
            {
                var s = msg[ss];

                Font.Draw(s, screen, xx, yy + ss * 8, color);
            }
        }
    }
}
