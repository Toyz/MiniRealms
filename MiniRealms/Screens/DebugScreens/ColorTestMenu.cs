﻿using System;
using System.Collections.Generic;
using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.OptionItems;
using MiniRealms.Screens.UIMenus;

namespace MiniRealms.Screens.DebugScreens
{
    public class ColorTestMenu : ScrollingMenu
    {
        private readonly List<Option> _options = new List<Option>();
        private readonly Random _rand = new Random();

        public ColorTestMenu(Menu parent) : base(parent)
        {
            MaxToShow = 15;
        }
       
        public override void Init(McGame game, InputHandler input)
        {
            base.Init(game, input);

            _options.Clear();
            for (var i = 0; i < _rand.Next(9, 100) + 1; i++)
            {
                var a = -1;
                var b = _rand.Next(1, 555) + 1;
                var c = _rand.Next(1, 555) + 1;
                var d = _rand.Next(1, 555) + 1;

                _options.Add(new LabelOption(Color.GetHex(a, b, c, d))
                {
                    SelectedColor = Color.Get(a, b, c, d),
                    Enabled = true
                });
            }

            _options.Add(new ChangeMenuOption("Debug Menu", Parent, Game));

            RenderScrollingListTable(_options, Location.Right);
        }

        public override void Render(Screen screen)
        {
            base.Render(screen);

            string title = "Color Test Menu";
            Font.Draw(title, screen, GameConts.ScreenMiddleWidth - (title.Length * 8 / 2), 1 * 8, Color.White);
        }
    }
}
