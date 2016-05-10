using System;
using MiniRealms.Engine.Audio.Sounds;

namespace MiniRealms.Screens.OptionItems
{
    public class ActionOption : LabelOption
    {
        private readonly Action _function;
        public bool Enabled { get; set; } = true;

        public ActionOption(string label, Action function) : base(label)
        {
            _function = function;
        }

        public override void HandleInput(InputHandler input)
        {
            if (!input.Attack.Clicked && !input.Menu.Clicked) return;
            if (!Enabled) return;
            SoundEffectManager.Play("test");
            _function?.Invoke();
        }

        public override void HandleRender()
        {
        }
    }
}
