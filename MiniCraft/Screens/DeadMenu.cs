using MiniCraft.Gfx;

namespace MiniCraft.Screens
{

    public class DeadMenu : Menu
    {
        private int _inputDelay = 60;

        public override void Tick()
        {
            if (_inputDelay > 0)
                _inputDelay--;
            else if (Input.Attack.Clicked || Input.Menu.Clicked)
            {
                Game.Level = null;
                Game.Player = null;
                Game.Levels = null;

                Game.SetMenu(new TitleMenu());
            }
        }

        public override void Render(Screen screen)
        {
            Font.RenderFrame(screen, "", 3, 4, 21, 10);
            Font.Draw("You died! Aww!", screen, 5 * 8, 5 * 8, Color.Get(-1, 555, 555, 555));

            int seconds = Game.GameTime / 60;
            int minutes = seconds / 60;
            int hours = minutes / 60;
            minutes %= 60;
            seconds %= 60;

            var timeString = hours > 0
                ? hours + "h" + (minutes < 10 ? "0" : "") + minutes + "m"
                : minutes + "m " + (seconds < 10 ? "0" : "") + seconds + "s";
            Font.Draw("Time:", screen, 5 * 8, 6 * 8, Color.Get(-1, 555, 555, 555));
            Font.Draw(timeString, screen, (5 + 5) * 8, 6 * 8, Color.Get(-1, 550, 550, 550));
            Font.Draw("Score:", screen, 5 * 8, 7 * 8, Color.Get(-1, 555, 555, 555));
            Font.Draw("" + Game.Player.Score, screen, (5 + 6) * 8, 7 * 8, Color.Get(-1, 550, 550, 550));
            Font.Draw("Press C to lose", screen, 5 * 8, 9 * 8, Color.Get(-1, 333, 333, 333));
        }
    }
}
