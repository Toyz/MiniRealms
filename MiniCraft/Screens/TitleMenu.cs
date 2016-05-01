using MiniCraft.Gfx;
using MiniCraft.Sounds;

namespace MiniCraft.Screens
{
    public class TitleMenu : Menu
    {
        private int _selected;

        private static readonly string[] Options = { "Start game", "How to play", "Options" };

        public override void Tick()
        {
            if (Input.Up.Clicked) _selected--;
            if (Input.Down.Clicked) _selected++;

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
                Game.ResetGame();
                Game.SetMenu(null);


            }
            if (_selected == 1) Game.SetMenu(new InstructionsMenu(this));

        }

        public override void Render(Screen screen)
        {
            screen.Clear(0);

            int h = 2;
            int w = 13;
            int titleColor = ColorHelper.Get(0, 010, 131, 551);
            int xo = (screen.W - w * 8) / 2;
            int yo = 24;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    screen.Render(xo + x * 8, yo + y * 8, x + (y + 6) * 32, titleColor, 0);
                }
            }

            for (int i = 0; i < Options.Length; i++)
            {
                string msg = Options[i];
                int col = ColorHelper.Get(0, 222, 222, 222);
                if (i == _selected)
                {
                    msg = "> " + msg + " <";
                    col = ColorHelper.Get(0, 555, 555, 555);
                }
                Font.Draw(msg, screen, (screen.W - msg.Length * 8) / 2, (8 + i) * 8, col);
            }

            Font.Draw("(Arrow keys,X and C)", screen, 0, screen.H - 8, ColorHelper.Get(0, 111, 111, 111));
        }
    }
}
