﻿using System.Collections.Generic;
using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.DebugScreens;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.OptionItems;
using MiniRealms.Screens.UIMenus;

namespace MiniRealms.Screens.MainScreens
{
    public class TitleMenu : ScrollingMenu
    {
        private bool ShowErrorAlert { get; set; }
        private string ErrorAlertBody { get; set; }

        public TitleMenu() : base(null)
        {
        }

        public override void Init(McGame game, InputHandler input)
        {
            base.Init(game, input);

            var options = new List<Option>
            {
                new ChangeMenuOption("New Game", new NewGameMenu(this), Game),
                new ChangeMenuOption("How to play", new InstructionsMenu(this), Game),
#if DEBUG
                new LabelOption("Mods") {Enabled = false},
                new ChangeMenuOption("Debug", new DebugMenu(this), Game),
#endif
                new ChangeMenuOption("Options", new OptionsMenu(this), Game),
                new ActionOption("Exit", () => Game.Exit()) { ClickSound = false }
            };
            
            RenderScrollingListTable(options, Location.Right);
        }

        public override void Render(Screen screen)
        {
            //right menu
            base.Render(screen);

            //top
            Font.Draw(GameConts.Name, screen, (screen.W - GameConts.Name.Length * 8) / 2, 20, Game.TickCount / 20 % 2 == 0 ? Color.White : Color.Yellow);

            //left menu
            RenderLeftMenuItems(screen);

            //Bottom render
            var xx = (GameConts.Width - "(Arrow keys,X and C)".Length * 8) / 2;

            Font.Draw("(Arrow keys,X and C)", screen, xx, screen.H - 8, Color.DarkGrey);

            if (ShowErrorAlert && !Game.IsLoadingWorld)
            {
                Game.RenderAlertWindow(ErrorAlertBody);
            }
        }

        private void RenderLeftMenuItems(Screen screen)
        {
            for (var i = 0; i < 5; i++)
            {
                RenderLeftMenuItem(15, i, $"UserItem: {i}", screen);
            }
        }

        private void RenderLeftMenuItem(int x1, int y1, string msg, Screen screen)
        {
            var xx = x1;
            var yy = (GameConts.Height / 4) + y1 * 22 + 8;

            const int w = 20;
            const int h = 1;

            if (w > msg.Length)
            {
                var spaces = w - msg.Length;
                for (var i = 0; i < spaces; i++)
                {
                    msg += " ";
                }
            }

            screen.Render(xx - 8, yy - 8, 0 + 13 * 32, Color.Get(0, 1, 5, 445), 0);
            screen.Render(xx + w * 8, yy - 8, 0 + 13 * 32, Color.Get(0, 1, 5, 445), 1);
            screen.Render(xx - 8, yy + 8, 0 + 13 * 32, Color.Get(0, 1, 5, 445), 2);
            screen.Render(xx + w * 8, yy + 8, 0 + 13 * 32, Color.Get(0, 1, 5, 445), 3);
            for (var x = 0; x < w; x++)
            {
                screen.Render(xx + x * 8, yy - 8, 1 + 13 * 32, Color.Get(0, 1, 5, 445), 0);
                screen.Render(xx + x * 8, yy + 8, 1 + 13 * 32, Color.Get(0, 1, 5, 445), 2);
            }
            for (var y = 0; y < h; y++)
            {
                screen.Render(xx - 8, yy + y * 8, 2 + 13 * 32, Color.Get(0, 1, 5, 445), 0);
                screen.Render(xx + w * 8, yy + y * 8, 2 + 13 * 32, Color.Get(0, 1, 5, 445), 1);
            }

            Font.Draw(msg, screen, xx, yy, Color.Get(5, 333, 333, 333));
        }
    }
}
