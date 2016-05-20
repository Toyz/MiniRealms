using System.Collections.Generic;
using MiniRealms.Engine.Gfx;
using MiniRealms.Engine.LevelGens;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.MainScreens;
using MiniRealms.Screens.OptionItems;
using MiniRealms.Screens.UIMenus;

namespace MiniRealms.Screens.GameScreens
{
    public class PauseGameMenu : ScrollingMenu
    {

        public PauseGameMenu(Menu parent) : base(parent)
        {
        }

        public override void Init(McGame game, InputHandler input)
        {
            base.Init(game, input);

            var options = new List<Option>
            {
                new ChangeMenuOption("Options", new OptionsMenu(this, "Go back"), game),
                new ChangeMenuOption("Back to Game", null, game),
                new ActionOption("Return to Title", ReturnToTitleAction)
            };

            RenderScrollingListTable(options);
        }

        public override void Tick()
        {
            base.Tick();

            if (Input.CloseKey.Clicked)
            {
                Game.SetMenu(new AnimatedTransitionMenu(null));
            }
        }

        private void ReturnToTitleAction()
        {
            Game.ResetGame();
            Game.Level = null;
            Game.Player = null;
            Game.Levels = null;
            LevelGen.R = null;
            Game.SetMenu(new TitleMenu());
        }

        public override void Render(Screen screen)
        {
            screen.Clear(0);
            base.Render(screen);

            string title = "Game is Paused";
            Font.Draw(title, screen, GameConts.ScreenMiddleWidth - (title.Length * 8 / 2), 1 * 8, Color.White);

            var xx = (GameConts.Width - "(Press \"Escape\" to close)".Length * 8) / 2;

            Font.Draw("(Press \"Escape\" to close)", screen, xx, screen.H - 8, Color.DarkGrey);
        }
    }
}