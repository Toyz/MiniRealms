using System.Collections.Generic;
using MiniRealms.Engine;
using MiniRealms.Engine.Gfx;
using MiniRealms.Engine.LevelGens;
using MiniRealms.Engine.UI.Objects;
using MiniRealms.Screens.Dialogs;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.MainScreens;
using MiniRealms.Screens.OptionItems;
using MiniRealms.Screens.UIMenus;

namespace MiniRealms.Screens.GameScreens
{
    public class PauseGameMenu : ScrollingMenu
    {
        private List<string> _lines;
        private bool _showAlert;
        private Label _titleLabel;

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

            RenderScrollingListTable(options, Location.Right);

            const int width = 16;
            var seconds = Game.GameTime/60;
            var minutes = seconds/60;
            var hours = minutes/60;
            minutes %= 60;
            seconds %= 60;

            var ts =
                $"{(hours > 0 ? hours + ":" + (minutes < 10 ? "0" : "") + minutes + ":" : minutes + ":" + (seconds < 10 ? "0" : "") + seconds)}";

            _lines = new List<string>
            {
                Utils.SpacesCenter(ts, width, 0, 1),
                $"Score:{Utils.SpacesPushleft(Game.Player.Score.ToString(), width, 6)}",
                $"Mode:{Utils.SpacesPushleft(McGame.Difficulty.ShortName, width, 5)}",
                $"Size:{Utils.SpacesPushleft($"{GameConts.Instance.MaxWidth}x{GameConts.Instance.MaxHeight}", width, 5)}"
            };

            _titleLabel = new Label(Game.UiManager, "Game is Paused", (GameConts.ScreenMiddleWidth - ("Game is Paused".Length * 8 / 2)), 15, Color.White);
            Game.UiManager.Add(_titleLabel);

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
            _showAlert = !_showAlert;

            Game.SetMenu(new AlertMenu(this, new[] { "No progress is saved" },
                () =>
                {
                    Game.ResetGame();
                    Game.Level = null;
                    Game.Player = null;
                    Game.Levels = null;
                    LevelGen.R = null;
                    Game.SetMenu(new TitleMenu());
                }));
        }

        public override void Render(Screen screen)
        {
            screen.Clear(0);
            base.Render(screen);

            RenderLeftMenuItem(18, (GameConts.Height / 4) + 1 * 22, 16, _lines.Count, _lines.ToArray(), Color.Get(5, 333, 333, 333), screen);

            var xx = (GameConts.Width - "(Press \"Escape\" to close)".Length * 8) / 2;

            Font.Draw("(Press \"Escape\" to close)", screen, xx, screen.H - 8, Color.DarkGrey);
        }
    }
}