using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.Options;
using Color = MiniRealms.Engine.Gfx.Color;
using Font = MiniRealms.Engine.Gfx.Font;
using Point = Microsoft.Xna.Framework.Point;

namespace MiniRealms.Screens
{
    public class NewGameMenu : Menu
    {
        private readonly Menu _parent;
        private int _selected;
        private readonly List<IOption> _options;
        private readonly WorldSizeOption _worldSizeOption;

        public NewGameMenu(Menu parent)
        {
            _parent = parent;

            _worldSizeOption = new WorldSizeOption();
            _options = new List<IOption>
            {
                _worldSizeOption,
                new ActionOption("Create and Start", CreateAndStartWorld),
                new ActionOption("Cancel", () => Game.SetMenu(parent))
            };
        }

        private void CreateAndStartWorld()
        {
            SoundEffectManager.Play("test");
            Game.LoadingText = "World Creation";
            Game.IsLoadingWorld = true;
            Game.CurrentLevel = 3;

            Point s = _worldSizeOption.Sizes[_worldSizeOption.Selected];

            Task.Run(() =>
            {
                Game.SetupLevel(s.X, s.Y);
            }).ContinueWith((e) =>
            {
                Game.IsLoadingWorld = false;
                Game.LoadingText = string.Empty;
                Game.ResetGame();
                Game.SetMenu(null);
            });
        }

        public override void Tick()
        {
            if (Game.IsLoadingWorld) return;
            
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
