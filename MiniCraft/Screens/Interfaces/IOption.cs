namespace MiniRealms.Screens.Interfaces
{
    public interface IOption
    {
        string Text { get; set; }
        void HandleInput(InputHandler input);
        void HandleRender();
    }
}
