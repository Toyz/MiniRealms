namespace MiniRealms.Screens.Options
{
    public class LabelOption : IOption
    {
        public string Text { get; set; }

        public LabelOption(string label)
        {
            Text = label;
        }

        public virtual void Tick(InputHandler input)
        {
        }

        public virtual void Update()
        {
        }
    }
}
