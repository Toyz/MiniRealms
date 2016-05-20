using System.Collections.Generic;
using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.OptionItems;

namespace MiniRealms.Screens.DebugScreens
{
    public class DebugMenu : Menu
    {
        private McGame _game;

        private int _selected;

        private static List<Option> _options;

        public DebugMenu(Menu parent) : base(parent)
        {
        }

        public override void Init(McGame game, InputHandler input)
        {
            base.Init(game, input);

            _game = game;

            _options = new List<Option>
            {
                //new ChangeMenuOption("Scrolling Menu", new ScrollingMenu(this), game),
                new ChangeMenuOption("Color Test", new ColorTestMenu(this), game),
                new ChangeMenuOption("Main Menu", Parent, game)
            };
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


            _options[_selected].HandleInput(Input);
        }

        public override void Render(Screen screen)
        {
            screen.Clear(0);

            string title = "Debug Options";
            Font.Draw(title, screen,  GameConts.ScreenMiddleWidth - (title.Length * 8 / 2), 1 * 8, Color.White);

            for (var i = 0; i < _options.Count; i++)
            {
                var option = _options[i];
                var msg = option.Text;
                var col = Color.DarkGrey;
                if (i == _selected)
                {
                    msg = option.SelectedText;
                    col = Color.White;
                    option.HandleRender();
                }
                Font.Draw(msg, screen, (screen.W - msg.Length * 8) / 2, (GameConts.ScreenThirdHeight + (i * 10) - ((_options.Count * 8) / 2)), col);
            }
        }
    }
}
