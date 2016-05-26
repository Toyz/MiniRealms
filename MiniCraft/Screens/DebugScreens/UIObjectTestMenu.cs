using System.Diagnostics;
using MiniRealms.Engine.Gfx;
using MiniRealms.Engine.UI.Objects;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.MainScreens;

namespace MiniRealms.Screens.DebugScreens
{
    public class UiObjectTestMenu : Menu
    {
        private Label _titleLabel;
        private ProgressBar _progBar;

        public UiObjectTestMenu(Menu parent) : base(parent)
        {
        }

        public override void Init(McGame game, InputHandler input)
        {
            base.Init(game, input);

            string text = "Ui Object Testing"; 

            _titleLabel = new Label(Game.UiManager, text, /*(GameConts.ScreenMiddleWidth - (text.Length * 8 / 2))*/ -text.Length, 20, Color.White);
            _progBar = new ProgressBar(Game.UiManager)
            {
                X = 10,
                Y = 30,
                Width = 20,
                Progress = 0
            };

            Game.UiManager.Add(_titleLabel);
            Game.UiManager.Add(_progBar);
        }

        public override void Tick()
        {
            if (Input.Menu.Clicked) 
                Game.SetMenu(new AnimatedTransitionMenu(Parent));

            _titleLabel.Color = Game.TickCount / 20 % 2 == 0 ? Color.White : Color.Yellow;

            if (Game.TickCount / 20 % 2 == 0)
            {
                _progBar.Progress += 1;

                Debug.WriteLine(_titleLabel.X);

                if (_progBar.Progress > _progBar.Max)
                {
                    _progBar.Progress = 0;
                }
            }

            //Will be changed to read from a list of messages to be completely random :3
            if (Game.GameTime/40%2 == 0)
            {
                if (_titleLabel.X > GameConts.Width + (_titleLabel.Text.Length * 8))
                {
                    _titleLabel.X = -(_titleLabel.Text.Length * 8);
                }

                _titleLabel.X += 2;
            }

            Game.UiManager.Tick();
        }

        public override void Render(Screen screen)
        {
            screen.Clear(0);
            Game.UiManager.Render(screen);
        }
    }
}
