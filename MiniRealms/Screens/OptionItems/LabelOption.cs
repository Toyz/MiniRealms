using MiniRealms.Screens.Interfaces;

namespace MiniRealms.Screens.OptionItems
{
    public class LabelOption : Option
    {
        public sealed override string Text { get; set; }

        public LabelOption(string label)
        {
            Text = label;
        }

        protected internal override void HandleInput(InputHandler input)
        {
        }

        protected internal override void HandleRender()
        {
        }
    }
}
