using System.Collections.Generic;
using MiniRealms.Screens.Interfaces;

namespace MiniRealms.Screens.OptionItems
{
    public class DifficultyOption : Option
    {
        public class Difficulty
        {

            public Difficulty(string v1, int v2, int v3)
            {
                Name = v1;
                BaseLevel = v2;
                SpawnAmount = v3;
            }

            public string Name { get; }
            public int BaseLevel { get; }
            public int SpawnAmount { get; }
        }

        private readonly List<Difficulty> Sizes = new List<Difficulty>
        {
            new Difficulty("Easy", 0, 5000),
            new Difficulty("Medium", 1, 10000),
            new Difficulty("Hard", 2, 15000),
            new Difficulty("Nightmare", 3, 20000),

        };

        private int _selected;

        public override bool Enabled { get; set; } = true;
        public override string Text { get; set; } = "Difficulty: ";
        public override string SelectedText => $"< {Text} >";

        public DifficultyOption()
        {
            Difficulty s = Sizes[_selected];

            Text = $"Difficulty: {s.Name}";
        }

        protected internal override void HandleInput(InputHandler input)
        {
            if (input.Left.Clicked)
            {
                _selected--;
            }

            if (input.Right.Clicked)
            {
                _selected++;
            }

            int len = Sizes.Count;
            if (_selected < 0) _selected += len;
            if (_selected >= len) _selected -= len;

            Difficulty s = Sizes[_selected];

            Text = $"Difficulty: {s.Name}";
        }

        public Difficulty GetDifficulty()
        {
            return Sizes[_selected];
        }

        protected internal override void HandleRender()
        {
        }
    }
}
