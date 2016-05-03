using MiniRealms.Engine.Gfx;

namespace MiniRealms.Screens
{

    public class LevelTransitionMenu : Menu
    {
        private readonly int _dir;
        private int _time;

        public LevelTransitionMenu(int dir)
        {
            _dir = dir;
        }

        public override void Tick()
        {
            _time += 2;
            if (_time == 30) Game.ChangeLevel(_dir);
            if (_time == 60) Game.SetMenu(null);
        }

        public override void Render(Screen screen)
        {
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 15; y++)
                {
                    int dd = (y + x % 2 * 2 + x / 3) - _time;
                    if (dd >= 0 || dd <= -30) continue;
                    if (_dir > 0)
                        screen.Render(x * 8, y * 8, 0, 0, 0);
                    else
                        screen.Render(x * 8, screen.H - y * 8 - 8, 0, 0, 0);
                }
            }
        }
    }

}
