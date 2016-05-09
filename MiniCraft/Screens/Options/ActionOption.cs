using System;

namespace MiniRealms.Screens.Options
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
            _function?.Invoke();
        }

        public override void HandleRender()
        {
        }
    }
}
