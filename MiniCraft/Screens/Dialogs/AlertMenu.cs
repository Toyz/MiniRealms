using System;
using System.Collections.Generic;
using System.Linq;
using MiniRealms.Engine;
using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.UIMenus;

namespace MiniRealms.Screens.Dialogs
{

    public class AlertMenu : ScrollingMenu
    {
        private readonly string _title;
        private readonly List<string> _body;
        private readonly Action _yesAction;
        private readonly Action _noAction;
        private int _selected = 1;

        public AlertMenu(Menu parent, string[] body, Action yesAction, Action noAction = null) : base(parent)
        {
            _title = "Are you sure?";
            _body = body.ToList();
            //to fix a minor issue
            _body.Add(Utils.SpacesCenter(" ", 18));
            _body.Add(Utils.SpacesCenter(" ", 18));
            _yesAction = yesAction;
            _noAction = noAction;

            ShowNagger = false;
        }

        public override void Tick()
        {
            if (Input.Left.Clicked)
            {
                _selected = 0;
            }

            if (Input.Right.Clicked)
            {
                _selected = 1;
            }

            if (!Input.Menu.Clicked) return;
            if (_selected == 1)
            {
                if (_noAction == null)
                {
                    Game.SetMenu(Parent);
                }
                else
                {
                    _noAction?.Invoke();
                }
            }

            if (_selected == 0)
            {
                _yesAction?.Invoke();
            }
        }

        public override void Render(Screen screen)
        {
            var xx = (GameConts.Width - 20 * 8) / 2;
            var yy = ((GameConts.Height - (_body.Count * 8 / 2) ) - 8) / 2;

            RenderLeftMenuItem(xx, yy, 20, _body.Count, _body.ToArray(), Color.Get(5, 333, 333, 333), screen, true);
            Font.Draw(_title, screen, (xx + ((_title.Length * 8) / 2)) - 20, yy - (_body.Count + 1 * 8) + 3, Color.Get(5, 5, 5, 550));

            Font.Draw("Yes", screen, 14 * 8, yy + (_body.Count - 1) * 8 , _selected == 0 ? Color.Yellow : Color.DarkGrey);
            Font.Draw("No", screen, 24 * 8, yy + (_body.Count - 1) * 8, _selected == 1 ? Color.Yellow : Color.DarkGrey);
        }
    }
}
