namespace MiniRealms.Screens.Options
{
    public class LabelOption : IOption
    {
        public string Name { get; set; }

        public LabelOption(string label)
        {
            Name = label;
        }

        public virtual void Tick(InputHandler input)
        {
        }

        public virtual void Update()
        {
        }
    }
}
