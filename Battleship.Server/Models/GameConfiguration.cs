using System;

namespace Battleship.Server.Models
{
    public enum GameMode
    {
        quick,
        standard,
        Epic,
        Massive
    }
    public interface IGameConfigurationPrototype
    {
        IGameConfigurationPrototype DeepClone();
        IGameConfigurationPrototype ShallowClone();
        int BoardSize { get; set; }
        IReadOnlyDictionary<ShipType, int> FleetDefinition { get; init; }
        int TimeLimit { get; set; }
        GameMode Mode { get; set; }
        int MaxPlayers { get; set; } // Always 2 for now

    }
    public class GameConfiguration : IGameConfigurationPrototype
    {
        public GameMode Mode { get; set; } = GameMode.standard;
        public int BoardSize { get; set; } = 10;
        public required IReadOnlyDictionary<ShipType, int> FleetDefinition { get; init; }
        public int MaxPlayers { get; set; } = 2;
        public int TimeLimit { get; set; } = 60;

        public IGameConfigurationPrototype DeepClone()
        {
            return new GameConfiguration
            {
                Mode = this.Mode,
                BoardSize = this.BoardSize,
                FleetDefinition = new Dictionary<ShipType, int>(this.FleetDefinition),
                MaxPlayers = this.MaxPlayers,
                TimeLimit = this.TimeLimit
            };
        }

        public IGameConfigurationPrototype ShallowClone()
        {
            return (IGameConfigurationPrototype)this.MemberwiseClone();
        }
    }

    public static class GameConfigurationPrototypes
    {
        public static IGameConfigurationPrototype QuickGame = new GameConfiguration
        {
            Mode = GameMode.quick,
            BoardSize = 8,
            FleetDefinition = new Dictionary<ShipType, int>
            {
                [ShipType.Destroyer] = 2,
                [ShipType.Submarine] = 1
            },
            TimeLimit = 30,
            MaxPlayers = 2
        };

        public static IGameConfigurationPrototype StandardGame = new GameConfiguration
        {
            Mode = GameMode.standard,
            BoardSize = 10,
            FleetDefinition = ShipSpecification.StandardFleet,
            TimeLimit = 60,
            MaxPlayers = 2
        };


        public static IGameConfigurationPrototype EpicGame = new GameConfiguration
        {
            Mode = GameMode.Epic,
            BoardSize = 12,
            FleetDefinition = new Dictionary<ShipType, int>
            {
                [ShipType.Carrier] = 1,
                [ShipType.Battleship] = 1,
                [ShipType.Cruiser] = 2,
                [ShipType.Submarine] = 2,
                [ShipType.Destroyer] = 3
            },
            TimeLimit = 90,
            MaxPlayers = 2
        };

        public static IGameConfigurationPrototype MassiveGame = new GameConfiguration
        {
            Mode = GameMode.Massive,
            BoardSize = 15,
            FleetDefinition = new Dictionary<ShipType, int>
            {
                [ShipType.Carrier] = 2,
                [ShipType.Battleship] = 2,
                [ShipType.Cruiser] = 3,
                [ShipType.Submarine] = 3,
                [ShipType.Destroyer] = 4
            },
            TimeLimit = 120,
            MaxPlayers = 2
        };

        public static IGameConfigurationPrototype GetByMode(GameMode mode)
        {
            return mode switch
            {
                GameMode.quick => QuickGame,
                GameMode.Epic => EpicGame,
                GameMode.Massive => MassiveGame,
                GameMode.standard => StandardGame,
                _ => StandardGame
            };
        }

        public static IGameConfigurationPrototype GetByName(string? modeName)
        {
            if (string.IsNullOrWhiteSpace(modeName))
                return StandardGame;

            if (Enum.TryParse<GameMode>(modeName, true, out var mode))
                return GetByMode(mode);

            return StandardGame;
        }

        public static IEnumerable<string> GetAvailableModes()
        {
            return Enum.GetNames<GameMode>();
        }
    }
}