using System;
using MiniRealms.Engine.Audio.Sounds;

namespace MiniRealms.Screens.OptionItems
{
    public class ActionOption : LabelOption
    {
        private readonly Action _function;

        public override bool Enabled { get; set; } = true;

        public bool ClickSound { private get; set; } = true;

        public ActionOption(string label, Action function) : base(label)
        {
            _function = function;
        }

        protected internal override void HandleInput(InputHandler input)
        {
            if (!input.Attack.Clicked && !input.Menu.Clicked) return;
            if (!Enabled) return;
            if(ClickSound)
                SoundEffectManager.Play("test");
            _function?.Invoke();
        }

        protected internal override void HandleRender()
        {
        }
    }
}
