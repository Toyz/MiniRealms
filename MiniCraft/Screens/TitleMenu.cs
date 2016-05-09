using System.Collections.Generic;
using System.Threading.Tasks;
using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.Options;

namespace MiniRealms.Screens
{
    public class TitleMenu : Menu
    {
        public bool ShowErrorAlert { get; set; }
        public string ErrorAlertBody { get; set; }

        private int _selected;

        private static List<IOption> _options;

        public TitleMenu()
        {
            _options = new List<IOption>
            {
                new ActionOption("New Game", NewGameOption),
                new ActionOption("How to play", () =>
                {
                    SoundEffectManager.Play("test");
                    Game.SetMenu(new InstructionsMenu(this));
                }),
                new ActionOption("Options", () =>
                {
                    SoundEffectManager.Play("test");
                    Game.SetMenu(new OptionsMenu(this, Game));
                }),
                new ActionOption("Exit", () => Game.Exit())
            };

#if DEBUG
            _options.Insert(2, new LabelOption("Mods and Addons"));
#endif
        }

        private void NewGameOption()
        {
            SoundEffectManager.Play("test");
            Game.LoadingText = "World Creation";
            Game.IsLoadingWorld = true;
            Game.CurrentLevel = 3;

            Task.Run(() =>
            {
                Game.SetupLevel(GameConts.MaxWidth, GameConts.MaxHeight);
            }).ContinueWith((e) =>
            {
                Game.IsLoadingWorld = false;
                Game.LoadingText = string.Empty;
                Game.ResetGame();
                Game.SetMenu(null);
            });
        }

        public override void Tick()
        {
            if (Game.IsLoadingWorld) return;

            if (Input.CloseKey.Clicked) ShowErrorAlert = false;


            if (Input.Up.Clicked)
            {
                _selected--;
                ShowErrorAlert = false;
                SoundEffectManager.Play("menu_move");
            }
            if (Input.Down.Clicked)
            {
                SoundEffectManager.Play("menu_move");
                _selected++;
                ShowErrorAlert = false;
            }

            int len = _options.Count;
            if (_selected < 0) _selected += len;
            if (_selected >= len) _selected -= len;

            _options[_selected].Tick(Input);
        }

        public override void Render(Screen screen)
        {
            screen.Clear(0);

            Font.Draw(GameConts.Name, screen, (screen.W - GameConts.Name.Length * 8) / 2, 20, Game.TickCount / 20 % 2 == 0 ? Color.White : Color.Yellow);

            for (int i = 0; i < _options.Count; i++)
            {
                string msg = _options[i].Name;
                int col = Color.DarkGrey;
                if (i == _selected)
                {
                    _options[i].Update();
                    msg = "> " + msg + " <";
                    col = Color.White;
                }
                Font.Draw(msg, screen, (screen.W - msg.Length * 8) / 2, GameConts.ScreenMiddleHeight + (i * 8) - 20, col);
            }


            var xx = (GameConts.Width - "(Arrow keys,X and C)".Length * 8) / 2;

            Font.Draw("(Arrow keys,X and C)", screen, xx, screen.H - 8, Color.DarkGrey);

            if (ShowErrorAlert && !Game.IsLoadingWorld)
            {
                Game.RenderAlertWindow(ErrorAlertBody);
            }
        }
    }
}
