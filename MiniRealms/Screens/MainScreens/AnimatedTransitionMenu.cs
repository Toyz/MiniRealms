using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.Interfaces;

namespace MiniRealms.Screens.MainScreens
{

    public class AnimatedTransitionMenu : Menu
    {
        private readonly int _transitionTime;
        private readonly int _dir;
        private readonly int _color;
        private readonly bool _spawnMenu;
        private int _time;

        public AnimatedTransitionMenu(Menu menu, int transitionTime = 60, int dir = 0, int color = 0, bool spawnMenu = false) : base(menu)
        {
            _transitionTime = transitionTime;
            _dir = dir;
            _color = color;
            _spawnMenu = spawnMenu;
        }

        public override void Tick()
        {
            _time += 2;
            if (_time == _transitionTime)
            {
                Game.SetMenu(!_spawnMenu ? new AnimatedTransitionMenu(Parent, _transitionTime, 1, Color.Grey, true) : Parent);
            }
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
                        screen.Render(x * 8, y * 8, 0, _color, 0);
                    else
                        screen.Render(x * 8, screen.H - y * 8 - 8, 0, _color, 0);
                }
            }
        }
    }

}
