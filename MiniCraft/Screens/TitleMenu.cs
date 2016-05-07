using System.Threading.Tasks;
using MiniRealms.Engine.Gfx;
using MiniRealms.Sounds;

namespace MiniRealms.Screens
{
    public class TitleMenu : Menu
    {
        public bool ShowErrorAlert { get; set; }
        public string ErrorAlertBody { get; set; }

        private int _selected;

        private static readonly string[] Options = { "Start game", "How to play", "Mods and Addons", "Options" };

        public override void Tick()
        {
            if (Game.IsLoadingWorld) return;

            if (Input.CloseKey.Clicked) ShowErrorAlert = false;


            if (Input.Up.Clicked)
            {
                _selected--;
                ShowErrorAlert = false;
                Sound.PlaySound("menu_move");
            }
            if (Input.Down.Clicked)
            {
                Sound.PlaySound("menu_move");
                _selected++;
                ShowErrorAlert = false;
            }

            int len = Options.Length;
            if (_selected < 0) _selected += len;
            if (_selected >= len) _selected -= len;

            if (!Input.Attack.Clicked && !Input.Menu.Clicked) return;

            if (_selected < 0)
            {
                _selected = len - 1;
            }

            if (_selected > len - 1)
            {
                _selected = 0;
            }

            if (_selected == 0)
            {
                Sound.PlaySound("test");
                Game.LoadingText = "World Creation";
                Game.IsLoadingWorld = true;
                Game.CurrentLevel = 3;

                Task.Run(() =>
                {
                    Game.SetupLevel(256, 256);
                }).ContinueWith((e) =>
                {
                    Game.IsLoadingWorld = false;
                    Game.LoadingText = string.Empty;
                    Game.ResetGame();
                    Game.SetMenu(null);
                });
            }
            if (_selected == 1) Game.SetMenu(new InstructionsMenu(this));
            if (_selected == 2 || _selected == 3)
            {
                ShowErrorAlert = true;
                ErrorAlertBody = "Coming Soon";
            }
        }


        public override void Render(Screen screen)
        {
            screen.Clear(0);

            Font.Draw(Game.Name, screen, (screen.W - Game.Name.Length * 8) / 2, 20,
                 Game.TickCount / 20 % 2 == 0 ? Color.White : Color.Yellow);

            for (int i = 0; i < Options.Length; i++)
            {
                string msg = Options[i];
                int col = Color.DarkGrey;
                if (i == _selected)
                {
                    msg = "> " + msg + " <";
                    col = Color.White;
                }
                Font.Draw(msg, screen, (screen.W - msg.Length * 8) / 2, (8 + i) * 8, col);
            }


            var xx = (Game.Width - "(Arrow keys,X and C)".Length * 8) / 2;

            Font.Draw("(Arrow keys,X and C)", screen, xx, screen.H - 8, Color.DarkGrey);

            if (ShowErrorAlert && !Game.IsLoadingWorld)
            {
                Game.RenderAlertWindow(ErrorAlertBody);
            }
        }
    }
}
