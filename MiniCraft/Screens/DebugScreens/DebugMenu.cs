using System;
using System.Collections.Generic;
using System.Diagnostics;
using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.Dialogs;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.OptionItems;
using MiniRealms.Screens.UIMenus;

namespace MiniRealms.Screens.DebugScreens
{
    public class DebugMenu : ScrollingMenu
    {
        public DebugMenu(Menu parent) : base(parent)
        {
        }

        public override void Init(McGame game, InputHandler input)
        {
            base.Init(game, input);
            
            var options = new List<Option>
            {
                new ChangeMenuOption("Color Test", new ColorTestMenu(this), game),
                new ActionOption("Alert Dialog", () =>
                {
                    game.SetMenu(new AlertMenu(this, new[] {"I am a test", "So am i", "But also am i"}, YesAction));
                }),
                new ChangeMenuOption("Main Menu", Parent, game)
            };

            RenderScrollingListTable(options);
        }

        private static void YesAction()
        {
            Debug.WriteLine("Yes was pressed");
        }

        public override void Render(Screen screen)
        {
            base.Render(screen);

            string title = "Debug Options";
            Font.Draw(title, screen,  GameConts.ScreenMiddleWidth - (title.Length * 8 / 2), 1 * 8, Color.White);
        }
    }
}
