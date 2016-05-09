namespace MiniRealms.Screens.Options
{
    public interface IOption
    {
        string Name { get; set; }
        void Tick(InputHandler input);
        void Update();
    }
}
