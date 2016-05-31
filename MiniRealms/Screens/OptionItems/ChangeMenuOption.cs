using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.MainScreens;

namespace MiniRealms.Screens.OptionItems
{
    public class ChangeMenuOption : LabelOption
    {
        private readonly Menu _goto;
        private readonly McGame _game;

        public override bool Enabled { get; set; } = true;

        public bool ClickSound { private get; set; } = true;

        public ChangeMenuOption(string label, Menu @goto, McGame game) : base(label)
        {
            _goto = @goto;
            _game = game;
        }

        protected internal override void HandleInput(InputHandler input)
        {
            if (!input.Attack.Clicked && !input.Menu.Clicked) return;
            if (!Enabled) return;
            if(ClickSound)
                GameEffectManager.Play("test");
            _game.SetMenu(new AnimatedTransitionMenu(_goto));
            
        }

        protected internal override void HandleRender()
        {
        }
    }
}
