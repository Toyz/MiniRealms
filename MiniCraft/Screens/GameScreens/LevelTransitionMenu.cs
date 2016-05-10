using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.Interfaces;

namespace MiniRealms.Screens.GameScreens
{

    public class LevelTransitionMenu : Menu
    {
        private readonly int _dir;
        private readonly bool _isAbs;
        private int _time;

        public LevelTransitionMenu(int dir, bool isAbs = false)
        {
            _dir = dir;
            _isAbs = isAbs;
        }

        public override void Tick()
        {
            _time += 2;
            if (_time == 30) Game.ChangeLevel(_dir, _isAbs);
            if (_time == 60) Game.SetMenu(null);
        }

        public override void Render(Screen screen)
        {
            for (int x = 0; x < (GameConts.Width / 8) + 1; x++)
            {
                for (int y = 0; y < (GameConts.Height / 8) + 1; y++)
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
