using System;

namespace MiniRealms.Screens.Options
{
    public class ActionOption : LabelOption
    {
        private readonly Action _function;

        public ActionOption(string label, Action function) : base(label)
        {
            _function = function;
        }

        public override void Tick(InputHandler input)
        {
            if (!input.Attack.Clicked && !input.Menu.Clicked) return;
            _function?.Invoke();
        }

        public override void Update()
        {
        }
    }
}
