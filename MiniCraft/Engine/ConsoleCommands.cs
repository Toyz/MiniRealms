using System;
using System.Linq;
using GameConsole.ManualInterpreter;
using MiniRealms.Entities;
using MiniRealms.Items;
using MiniRealms.Items.Resources;

namespace MiniRealms.Engine
{
    public class ConsoleCommands
    {
        private readonly McGame _game;
        public readonly ManualInterpreter ManualInterpreter = new ManualInterpreter();

        public ConsoleCommands(McGame game)
        {
            _game = game;
            ManualInterpreter.RegisterCommand("give-item", GiveItemCommand);
            ManualInterpreter.RegisterCommand("spawn-mob", SpawnMobCommand);
            ManualInterpreter.RegisterCommand("kill-me", KillMeCommand);

            //Normal commands XD
            ManualInterpreter.RegisterCommand("clear", _ =>
            { 
                game.Console.Clear();
            });
        }

        private string KillMeCommand(string[] arg)
        {
            if (_game.Player == null)
            {
                return "Player doesn't exist in current game";
            }

            _game.Player.Health = 0;
            return "Who you kill self D:";
        }

        private string SpawnMobCommand(string[] strings)
        {
            if (_game.Levels?[_game._currentLevel] == null)
            {
                return "World is null, or doesn't exist!";
            }

            if (strings.Length < 1)
            {
                return "Invalid command args";
            }

            switch (strings[0].ToLower())
            {
                case "zombie":
                    _game.Levels?[_game._currentLevel].Add(new Zombie(1)
                    {
                        X = _game.Player.X + 5,
                        Y = _game.Player.Y + 5
                    });
                    return "Summoned a zombie";
                case "slime":
                    _game.Levels?[_game._currentLevel].Add(new Slime(1)
                    {
                        X = _game.Player.X + 5,
                        Y = _game.Player.Y + 5
                    });
                    return "Summoned a slime";
            }
            return "Invalid mod type";
        }

        public string GiveItemCommand(string[] strings)
        {
            if (_game.Player == null)
            {
                return "Player doesn't exist in current game";
            }

            if (strings.Length < 2)
            {
                return "Invalid command args";
            }

            var r = Resource.AllResources;

            var item = r.FirstOrDefault(i => string.Equals(i.Name, strings[0], StringComparison.CurrentCultureIgnoreCase));

            if (item == null)
            {
                return $"Item {strings[0]} doesn't exist!";
            }

            int amount;
            bool count = int.TryParse(strings[1], out amount);

            if (!count)
            {
                return "Invalid number";
            }

            _game.Player.Inventory.Add(new ResourceItem(item, amount));

            return $"Gave {amount} of item {strings[0]}";
        }
    }
}
