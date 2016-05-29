using System.Collections.Generic;
using System.IO;
using MiniRealms.Engine;
using MiniRealms.Engine.Gfx;
using MiniRealms.Engine.ScoreSystem;
using MiniRealms.Engine.UI.Objects;
using MiniRealms.Screens.DebugScreens;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.OptionItems;
using MiniRealms.Screens.UIMenus;

namespace MiniRealms.Screens.MainScreens
{
    public class TitleMenu : ScrollingMenu
    {
        private List<Score> _score;
        private Label _bottomLabel;
        private static string[] _credits;
        private int _currentCredit = 1;

        public TitleMenu() : base(null)
        {
        }

        public override void Init(McGame game, InputHandler input)
        {
            base.Init(game, input);

            if(_credits == null)
                _credits = File.Exists(Path.Combine(Game.Content.RootDirectory, "credits.txt")) ? 
                    File.ReadAllLines(Path.Combine(Game.Content.RootDirectory, "credits.txt")) : new[] {"Credits are missing why?"};

            ScoreBoardManager.Load();
            _score = ScoreBoardManager.Scores.Score;
            _bottomLabel = new Label(Game.UiManager) {Text = _credits[0], Color = Color.Get(-100, 222, 222, 222)};
            _bottomLabel.X = -(_bottomLabel.Text.Length * 8);
            _bottomLabel.Y = GameConts.Height - 8;

            Game.UiManager.Add(_bottomLabel);

            var options = new List<Option>
            {
                new ChangeMenuOption("New Game", new NewGameMenu(this), Game),
                new ChangeMenuOption("How to play", new InstructionsMenu(this), Game),
                new LabelOption("Mods") {Enabled = false},
#if DEBUG
                new ChangeMenuOption("Debug", new DebugMenu(this), Game),
#endif
                new ChangeMenuOption("Options", new OptionsMenu(this), Game),
                new ActionOption("Exit", () => Game.Exit()) { ClickSound = false }
            };
            
            RenderScrollingListTable(options, Location.Right);
        }

        public override void Tick()
        {
            base.Tick();

            //Bottom message ticker
            if (Game.GameTime / 20 % 2 == 0)
            {
                _bottomLabel.X += 1;
                if (_bottomLabel.X > GameConts.Width + _bottomLabel.Text.Length)
                {
                    _currentCredit++;

                    if (_currentCredit > (_credits.Length - 1))
                    {
                        _currentCredit = 0;
                    }

                    _bottomLabel.Text = _credits[_currentCredit];
                    _bottomLabel.X = -(_bottomLabel.Text.Length * 8);
                }
            }
        }

        public override void Render(Screen screen)
        {
            //right menu
            base.Render(screen);

            int h = 3;
            int w = 14;
            int titleColor = Game.TickCount / 20 % 2 == 0 ? Color.Get(-1, 010, 131, 551) : Color.Get(-1, 010, 131, 549);
            int xo = (screen.W - w * 8) / 2;
            int yo = 5;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    screen.Render(xo + x * 8, yo + y * 8, x + (y + 26) * 32, titleColor, 0);
                }
            }

            //left menu
            RenderLeftMenuItems(screen);
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
                        ? hours + ":" + (minutes < 10 ? "0" : "") + minutes + ":"
                        : minutes + ":" + (seconds < 10 ? "0" : "") + seconds;


                    var l = new string[]
                    {
                        $"You Won:{Utils.SpacesPushleft($"{(s.YouWon ? "Completed" : "Failed")}", 21, 8)}",
                        $"Mode:{Utils.SpacesPushleft($"{s.Difficulty} - {ts}", 21, 5)}",
                        $"Score:{Utils.SpacesPushleft(s.AcScore.ToString(), 21, 6)}"
                    };

                    RenderLeftMenuItem(10, (GameConts.Height / 4) + i * 38, 21, l.Length, l, Color.Get(5, 333, 333, 333), screen);
                }
            }
            else
            {
                RenderLeftMenuItem(15, (GameConts.Height / 4) + 2 * 22, 21, 1, Utils.SpacesCenter("No scores", 21, 0, 3), Color.Get(5, 333, 333, 333), screen);
            }
        }
    }
}
