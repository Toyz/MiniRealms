using System;
using System.Collections.Generic;
using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.Options;

namespace MiniRealms.Screens
{
    public class OptionsMenu : Menu
    {
        private readonly McGame _game;

        private int _selected;

        private static List<IOption> _options;
        private readonly ActionOption _fullScreenOption;

        public OptionsMenu(Menu parent, McGame game)
        {
            _game = game;
            _fullScreenOption = new ActionOption(
                $"Full Screen: {(game.GDM.IsFullScreen ? "Yes" : "No")}", FullScreenActionToggle);
            _options = new List<IOption>
            {
                new VolumeContol(),
                _fullScreenOption,
                new ActionOption("Main Menu", () => Game.SetMenu(parent))
            };
        }

        public void FullScreenActionToggle()
        {
            McGame mcGame = _game;

            mcGame.GDM.IsFullScreen = !mcGame.GDM.IsFullScreen;
            mcGame.GDM.ApplyChanges();
            _fullScreenOption.Name = $"Full Screen: {(mcGame.GDM.IsFullScreen ? "Yes" : "No")}";
        }

        public override void Tick()
        {
            if (Input.Up.Clicked)
            {
                _selected--;
                SoundEffectManager.Play("menu_move");
            }

            if (Input.Down.Clicked)
            {
                SoundEffectManager.Play("menu_move");
                _selected++;
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
                    msg = "> " + msg + " <";
                    col = Color.White;
                    _options[i].Update();
                }
                Font.Draw(msg, screen, (screen.W - msg.Length * 8) / 2, GameConts.ScreenMiddleHeight + (i * 8) - 20, col);
            }
        }
    }
}
