using System.Collections.Generic;
using MiniRealms.Engine;
using MiniRealms.Engine.Gfx;
using MiniRealms.Engine.UI;
using MiniRealms.Objects.ScoreSystem;
using MiniRealms.Screens.DebugScreens;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.OptionItems;
using MiniRealms.Screens.UIMenus;

namespace MiniRealms.Screens.MainScreens
{
    public class TitleMenu : ScrollingMenu
    {
        private List<Score> _score;
        private Label _titleLabel;

        public TitleMenu() : base(null)
        {
        }

        public override void Init(McGame game, InputHandler input)
        {
            base.Init(game, input);

            ScoreBoardManager.Load();
            _score = ScoreBoardManager.Scores.Score;
            _titleLabel = new Label(Game.UiManager, GameConts.Name, (GameConts.ScreenMiddleWidth - (GameConts.Name.Length * 8 / 2)), 15, Color.White);
            Game.UiManager.Add(_titleLabel);

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

            _titleLabel.Color = Game.TickCount / 20 % 2 == 0 ? Color.White : Color.Yellow;
            

            //left menu
            RenderLeftMenuItems(screen);

            //Bottom render
            var xx = (GameConts.Width - "(Arrow keys,X and C)".Length * 8) / 2;

            Font.Draw("(Arrow keys,X and C)", screen, xx, screen.H - 8, Color.DarkGrey);
        }

        private void RenderLeftMenuItems(Screen screen)
        {
            if (_score.Count > 0)
            {
                for (var i = 0; i < _score.Count; i++)
                {
                    Score s = _score[i];

                    int seconds = s.TimeTookMs/60;
                    int minutes = seconds/60;
                    int hours = minutes/60;
                    minutes %= 60;
                    seconds %= 60;

                    var ts = hours > 0
                        ? hours + "h" + (minutes < 10 ? "0" : "") + minutes + "m"
                        : minutes + "m " + (seconds < 10 ? "0" : "") + seconds + "s";


                    var l = new List<string>
                    {
                        Utils.SpacesCenter(ts, 21, 0, -2),
                        $"Score:{Utils.SpacesPushleft(s.AcScore.ToString(), 21, 6)}",
                        $"Mode:{Utils.SpacesPushleft(s.Difficulty, 21, 5)}",
                    };

                    RenderLeftMenuItem(15, (GameConts.Height / 4) + i * 38, 21, l.Count, l.ToArray(), Color.Get(5, 333, 333, 333), screen);
                }
            }
            else
            {
                RenderLeftMenuItem(15, (GameConts.Height / 4) + 2 * 22, 21, 1, Utils.SpacesCenter("No scores", 21, 0, 3), Color.Get(5, 333, 333, 333), screen);
            }
        }
    }
}
