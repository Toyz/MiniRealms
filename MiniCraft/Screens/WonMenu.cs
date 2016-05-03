﻿using MiniRealms.Engine.Gfx;
using MiniRealms.Engine.LevelGens;

namespace MiniRealms.Screens
{

    public class WonMenu : Menu
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
                LevelGen.R = null;
                Game.SetMenu(new TitleMenu());
            }
        }

        public override void Render(Screen screen)
        {
            Font.RenderFrame(screen, "", 1, 3, 18, 9);
            Font.Draw("You won! Yay!", screen, 2 * 8, 4 * 8, Color.Get(-1, 555, 555, 555));

            int seconds = Game.GameTime / 60;
            int minutes = seconds / 60;
            int hours = minutes / 60;
            minutes %= 60;
            seconds %= 60;

            var timeString = hours > 0
                ? hours + "h" + (minutes < 10 ? "0" : "") + minutes + "m"
                : minutes + "m " + (seconds < 10 ? "0" : "") + seconds + "s";
            Font.Draw("Time:", screen, 2 * 8, 5 * 8, Color.Get(-1, 555, 555, 555));
            Font.Draw(timeString, screen, (2 + 5) * 8, 5 * 8, Color.Get(-1, 550, 550, 550));
            Font.Draw("Score:", screen, 2 * 8, 6 * 8, Color.Get(-1, 555, 555, 555));
            Font.Draw("" + Game.Player.Score, screen, (2 + 6) * 8, 6 * 8, Color.Get(-1, 550, 550, 550));
            Font.Draw("Press C to win", screen, 2 * 8, 8 * 8, Color.Get(-1, 333, 333, 333));
        }
    }

}
