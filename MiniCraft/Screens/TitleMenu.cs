using System.Threading.Tasks;
using MiniRealms.Gfx;
using MiniRealms.Sounds;

namespace MiniRealms.Screens
{
    public class TitleMenu : Menu
    {
        public bool ShowErrorAlert { get; set; }
        public string ErrorAlertBody { get; set; }

        private int _selected;

        private static readonly string[] Options = { "Start game", "How to play", "Mods & Addons", "Options" };

        public override void Tick()
        {
            if (Game.IsLoadingWorld) return;

            if (Input.CloseKey.Clicked) ShowErrorAlert = false;


            if (Input.Up.Clicked)
            {
                _selected--;
                ShowErrorAlert = false;
            }
            if (Input.Down.Clicked)
            {
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
                Sound.Test.Play();
                Game.LoadingText = "World Creation";
                Game.IsLoadingWorld = true;
                Task.Run(() =>
                {
                    Game.SetupLevel(512, 512);
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

            /*int h = 2;
            int w = 13;
            int titleColor = Color.Get(0, 010, 131, 551);
            int xo = (screen.W - w * 8) / 2;
            int yo = 24;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    screen.Render(xo + x * 8, yo + y * 8, x + (y + 6) * 32, titleColor, 0);
                }
            }*/

            string mg = "MiniRealms";

            Font.Draw(mg, screen, (screen.W - mg.Length * 8) / 2, 20,
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
