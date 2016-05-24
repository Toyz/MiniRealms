using System.Collections.Generic;
using System.Diagnostics;
using MiniRealms.Engine.Gfx;
using MiniRealms.Engine.UI;
using MiniRealms.Engine.UI.Objects;
using MiniRealms.Screens.Dialogs;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.OptionItems;
using MiniRealms.Screens.UIMenus;

namespace MiniRealms.Screens.DebugScreens
{
    public class DebugMenu : ScrollingMenu
    {
        private Label _titleLabel;

        public DebugMenu(Menu parent) : base(parent)
        {
        }

        public override void Init(McGame game, InputHandler input)
        {
            base.Init(game, input);
            
            var options = new List<Option>
            {
                new ChangeMenuOption("Color Test", new ColorTestMenu(this), game),
                new ChangeMenuOption("UI Object Test", new UiObjectTestMenu(this), game),
                new ActionOption("Alert Dialog", () =>
                {
                    game.SetMenu(new AlertMenu(this, new[] {"I am a test", "So am i", "But also am i"}, YesAction));
                }),
                new ChangeMenuOption("Main Menu", Parent, game)
            };

            _titleLabel = new Label(Game.UiManager, "Debug Tools", (GameConts.ScreenMiddleWidth - ("Debug Tools".Length * 8 / 2)), 15, Color.White);
            Game.UiManager.Add(_titleLabel);

            RenderScrollingListTable(options);
        }

        private void YesAction()
        {
            Debug.WriteLine("Yes was pressed");
            Game.SetMenu(this);
        }
    }
}
