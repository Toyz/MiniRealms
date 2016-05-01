namespace MiniCraft.Items
{
    public class ToolType
    {
        public static ToolType Shovel = new ToolType("Shvl", 0);
        public static ToolType Hoe = new ToolType("Hoe", 1);
        public static ToolType Sword = new ToolType("Swrd", 2);
        public static ToolType Pickaxe = new ToolType("Pick", 3);
        public static ToolType Axe = new ToolType("Axe", 4);

        public readonly string Name;
        public readonly int Sprite;

        private ToolType(string name, int sprite)
        {
            Name = name;
            Sprite = sprite;
        }
    }
}
