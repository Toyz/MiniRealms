namespace MiniRealms.Screens.Options
{
    public interface IOption
    {
        string Text { get; set; }
        void Tick(InputHandler input);
        void Update();
    }
}
