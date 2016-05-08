using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Engine.Gfx;

namespace MiniRealms.Entities.Particles
{
    public class SmashParticle : Entity
    {
        private int _time;

        public SmashParticle(int x, int y)
        {
            X = x;
            Y = y;
            SoundManager.Play("monsterhurt");
        }

        public override void Tick()
        {
            _time++;
            if (_time > 10)
            {
                Remove();
            }
        }

        public override void Render(Screen screen)
        {
            int col = Color.Get(-1, 555, 555, 555);
            screen.Render(X - 8, Y - 8, 5 + 12 * 32, col, 2);
            screen.Render(X - 0, Y - 8, 5 + 12 * 32, col, 3);
            screen.Render(X - 8, Y - 0, 5 + 12 * 32, col, 0);
            screen.Render(X - 0, Y - 0, 5 + 12 * 32, col, 1);
        }
    }
}
