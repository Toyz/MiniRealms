using System.Collections.Generic;
using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.OptionItems;
using MiniRealms.Screens.UIMenus;

namespace MiniRealms.Screens.DebugScreens
{
    public class DebugMenu : ScrollingMenu
    {
        private static List<Option> _options;

        public DebugMenu(Menu parent) : base(parent)
        {
        }

        public override void Init(McGame game, InputHandler input)
        {
            base.Init(game, input);

            _options = new List<Option>
            {
                //new ChangeMenuOption("Scrolling Menu", new ScrollingMenu(this), game),
                new ChangeMenuOption("Color Test", new ColorTestMenu(this), game),
                new ChangeMenuOption("Main Menu", Parent, game)
            };

            RenderScrollingListTable(_options);
        }

        public override void Render(Screen screen)
        {
            base.Render(screen);

            string title = "Debug Options";
            Font.Draw(title, screen,  GameConts.ScreenMiddleWidth - (title.Length * 8 / 2), 1 * 8, Color.White);
        }
    }
}
