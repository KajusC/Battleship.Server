using Battleship.Client.Classes;
using Battleship.Client.DesignPatterns;
using Battleship.Server.Enum;
using System.Collections;

namespace Battleship.Server.Classes
{
    class NPC
    {
        // Daryti NPC kuris zaidzia


        // The NPC maintains a reference to one of the Strategy objects. The
        // NPC does not know the concrete class of a strategy. It should
        // work with all strategies via the Strategy interface.
        private IStrategy _strategy;
        private int[,] grid1;

        private Grid my_grid;

        private Grid enemy_grid;
        private Game game;

        public NPC()
        { }

        // Usually, the NPC accepts a strategy through the constructor, but
        // also provides a setter to change it at runtime.
        public NPC(IStrategy strategy)
        {
            _strategy = strategy;
        }
        public void SetGrid1(int[,] grid)
        {
            this.grid1 = grid;
        }
        // Usually, the NPC allows replacing a Strategy object at runtime.
        public void SetStrategy(IStrategy strategy)
        {
            _strategy = strategy;
        }
        public void SetMyGrid(int x, int y)
        {
            my_grid = new Grid(x,y,TileStatus.Empty);
        }
        public void SetEnemyGrid(int x, int y)
        {
            enemy_grid = new Grid(x, y, TileStatus.Unknown);
        }
        // The NPC delegates some work to the Strategy object instead of
        // implementing multiple versions of the algorithm on its own.
        public void SetGame(Game game)
        {
            this.game = game;
        }

        public bool DoStrategy()
        {
            object obj = _strategy.DoStrategy(enemy_grid);
            int[] xy = (int[])obj;
            if (xy.Length == 0) {//no next ship tile
                SetStrategy(new ConcreteStrategySearchRandom());
                obj = _strategy.DoStrategy(enemy_grid);
                xy = (int[])obj;
            }
            int x = xy[0];
            int y = xy[1];
            TileStatus tileStatus = game.GetResponse(x, y);
            
            Console.WriteLine("({0};{1}) {2}", x, y,tileStatus.ToString());
            switch (tileStatus)
            {
                case TileStatus.Damaged:
                    SetStrategy(new ConcreteStrategyDestroyTile(x, y));
                    break;

                case TileStatus.Destroyed:
                    SetStrategy(new ConcreteStrategySearchNextShipTile(x,y));
                    break;

                case TileStatus.GameEnd:
                    return false;
                    break;

            }
            return true;
        }
    }

    
    public interface IStrategy
    {
        object DoStrategy(object data);
    }
    public class ConcreteStrategySearchRandom : IStrategy
    {
        public object DoStrategy(object data)
        {
            int x, y;
            Grid grid = data as Grid;
            Random rand = new Random();

            x = rand.Next(0, grid.GetSize_x());
            y = rand.Next(0, grid.GetSize_y());

            while (grid.GetTile(x, y) != TileStatus.Unknown)
            {
                x = rand.Next(0, grid.GetSize_x());
                y = rand.Next(0, grid.GetSize_y());
            }
            return new int[] { x, y };
        }
    }



    public class ConcreteStrategySearchDiagonal : IStrategy
    {
        public object DoStrategy(object data)
        {
            var grid = data as int[,];
            int x, y;
            Random rand = new Random();

            x = rand.Next(0, 3);
            y = rand.Next(0, 3);
            return new int[] { x, y };

        }
    }

    public class ConcreteStrategyDestroyTile : IStrategy
    {
        private int x;
        private int y;
        public ConcreteStrategyDestroyTile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public object DoStrategy(object data)
        {
            Console.WriteLine(x);
            Console.WriteLine(y);
            return new int[] { x, y };
        }
    }
    public class ConcreteStrategySearchNextShipTile : IStrategy
    {
        private int x;
        private int y;

        public ConcreteStrategySearchNextShipTile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public object DoStrategy(object data)
        {
            Grid grid = data as Grid;
            if(grid.GetSize_x() != x + 1 && grid.GetTile(x+1,y) == TileStatus.Unknown)
            {
                return new int[] { x+1, y };
            }
            if (grid.GetSize_y() != y + 1 && grid.GetTile(x, y + 1) == TileStatus.Unknown)
            {
                return new int[] { x , y+1 };
            }
            if (x > 0 && grid.GetTile(x - 1, y) == TileStatus.Unknown)
            {
                return new int[] { x - 1, y };
            }
            if (y > 0 && grid.GetTile(x, y + 1) == TileStatus.Unknown)
            {
                return new int[] { x , y - 1 };
            }
            return new int[] { x, y };
        }
    }
}
