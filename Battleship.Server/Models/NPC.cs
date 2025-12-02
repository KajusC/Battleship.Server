using Battleship.Client.Classes;
using Battleship.Server.Classes;
using Battleship.Server.Enums;
using Battleship.Server.Models.Dtos;
using Battleship.Server.Services;
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace Battleship.Server.Models
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
        ShipPlacement shipPlacement;
        public int lastRow;
        public int lastColumn;
        public NPC(Game game)
        {

            _strategy = new StrategySearchRandom();
            this.game = game;
            SetEnemyGrid(10,10);
            lastColumn = 0;
            lastRow = 0;
        }
        public void DecideStrategy()
        {
            TileStatus tile = enemy_grid.GetTile(lastRow, lastColumn);
            if (tile == TileStatus.Destroyed) {
                int direction = getHeading(enemy_grid, lastRow, lastColumn);
                //Console.WriteLine("Direction {0} {1}" , direction , _strategy.GetType());
                if (direction == 0)
                {
                    SetStrategy(new StrategySearchNextShipTile(lastRow, lastColumn));
                }
                else if(_strategy is StrategySearchNextShipTile)
                {
                    ShipOrientation o = direction / 2 == 0 ? ShipOrientation.Vertical : ShipOrientation.Horizontal;
                    int dir = direction % 2;

                    SetStrategy(new StrategyCornerTile(lastRow, lastColumn, dir, o));
                }
            }
            else if(_strategy is StrategyCornerTile)
            {
                var strat = _strategy as StrategyCornerTile;

                int x = strat.c.Row; 
                int y = strat.c.Column;
                if (strat.orientation == ShipOrientation.Horizontal)
                    y += strat.direction * -1;
                else 
                    x += strat.direction * -1;
                SetStrategy(new StrategyCornerTile(x, y, strat.direction * -1, strat.orientation));
            }

        }
        public void Update(ShotOutcome outcome)
        {
            switch (outcome)
            {
                case ShotOutcome.Miss:
                    enemy_grid.SetTile(lastRow, lastColumn, TileStatus.Missed);
                    break;
                case ShotOutcome.Hit:
                    enemy_grid.SetTile(lastRow, lastColumn, TileStatus.Destroyed);
                    break;
                case ShotOutcome.Sunk:
                    enemy_grid.SetTile(lastRow, lastColumn, TileStatus.Destroyed);
                    break;
            }
            DecideStrategy();
        }
        
        public void SetGrid1(int[,] grid)
        {
            grid1 = grid;
        }
        // Usually, the NPC allows replacing a Strategy object at runtime.
        public void SetStrategy(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetMyGrid(int x, int y)
        {
            my_grid = new Grid(x, y, TileStatus.Empty);
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
        public Coordinate DoStrategy()
        {

            Coordinate coor = _strategy.DoStrategy(enemy_grid);
            if (coor.Row == -1)
            {
                SetStrategy(new StrategyCheckerSearch(enemy_grid));
                
                
                coor = _strategy.DoStrategy(enemy_grid);
                if (coor.Row == -1)
                {
                    SetStrategy(new StrategySearchRandom());
                    coor = _strategy.DoStrategy(enemy_grid);
                }

            }
            //Console.WriteLine("strategy {0} coord: {1},{2}", _strategy.GetType().ToString(), coor.Row,coor.Column);
            return coor;
        }
        public int getHeading(Grid grid,int x, int y)
        {
            int direction = 0;
            if (grid.GetSize_x() != x + 1 && grid.GetTile(x + 1, y) == TileStatus.Destroyed)
            {
                //orientation = ShipOrientation.Vertical;
                direction = -1;
            }
            if (grid.GetSize_y() != y + 1 && grid.GetTile(x, y + 1) == TileStatus.Destroyed)
            {
                //orientation = ShipOrientation.Horizontal;
                direction = -3;
            }
            if (x > 0 && grid.GetTile(x - 1, y) == TileStatus.Destroyed)
            {

                //orientation = ShipOrientation.Vertical;
                direction = 1;
            }
            if (y > 0 && grid.GetTile(x, y - 1) == TileStatus.Destroyed)
            {
                //orientation = ShipOrientation.Horizontal;
                direction = 3;
            }
            return direction;
        }

        public List<ShipPlacementDto> ShipPlacements(IReadOnlyDictionary<ShipType, int> Fleet, int Rows, int Columns)
        {
            List<ShipPlacementDto> placements = new List<ShipPlacementDto>();
            //int i = 1;
            Random rand = new Random();
            int gridSize = Rows;
            HashSet<(int, int)> occupied = new();
            foreach (KeyValuePair<ShipType, int> ship in Fleet)
            {
                //placements.Add(new ShipPlacementDto { ShipName = ship.Key, Row = i, Column = 1, Orientation = ShipOrientation.Horizontal.ToString() });
                int length = ship.Value;
                string name = ship.Key.ToString();
                bool placed = false;
                while (!placed)
                {
                    var orientation = rand.Next(2) == 0 ? ShipOrientation.Horizontal : ShipOrientation.Vertical;

                    int row, col;

                    if (orientation == ShipOrientation.Horizontal)
                    {
                        row = rand.Next(0, gridSize);
                        col = rand.Next(0, gridSize - length + 1);
                    }
                    else // Vertical
                    {
                        row = rand.Next(0, gridSize - length + 1);
                        col = rand.Next(0, gridSize);
                    }

                    // Collect all coordinates this ship would occupy
                    var cells = new List<(int, int)>();
                    for (int k = 0; k < length; k++)
                    {
                        int r = orientation == ShipOrientation.Horizontal ? row : row + k;
                        int c = orientation == ShipOrientation.Horizontal ? col + k : col;
                        cells.Add((r, c));
                    }

                    // Check for overlap
                    if (cells.Any(cell => occupied.Contains(cell)))
                        continue;

                    // Mark as occupied
                    foreach (var cell in cells)
                        occupied.Add(cell);

                    // Add to placements
                    placements.Add(new ShipPlacementDto
                    {
                        ShipName = name,
                        Row = row,
                        Column = col,
                        Orientation = orientation.ToString()
                    });

                    placed = true;
                }
                //i++;
            }
            return placements;
        }
}


    public interface IStrategy
    {
        Coordinate DoStrategy(object data);
    }
    
    public class StrategySearchRandom : IStrategy
    {
        public Coordinate DoStrategy(object data)
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
            return new Coordinate( x, y );
        }
    }


    //public class StrategyPlacement : IStrategy
    //{

    //    public object DoStrategy(object data)
    //    {
    //        IReadOnlyDictionary<string, int> FleetDefinition = ShipSpecification.StandardFleet;
    //        List<ShipPlacement> placements = new List<ShipPlacement>();
    //        int i = 1;
    //        foreach (KeyValuePair<string, int> ship in FleetDefinition)
    //        {
    //            placements.Add(new ShipPlacement(ship.Key, new Coordinate(i, 1), ShipOrientation.Horizontal));
    //            i++;
    //        }
    //        //Game game = data as Game;
    //        ////var fleet = _gameService.GetFleetDefinition()
    //        ////.Select(s => new ShipDefinitionDto { Name = s.Key, Length = s.Value })
    //        ////.ToArray();
    //        return placements;
    //    }
    //}
    public class StrategyCheckerSearch : IStrategy
    {
        private List<Coordinate> Coordinates;
        public StrategyCheckerSearch(Grid grid)
        {
            Coordinates = new List<Coordinate>();
            for (int i = 0; i < grid.GetSize_x(); i++)
            {
                for (int j = 0; j < grid.GetSize_y(); j++)
                {
                    if( (i%2==0 && j%2==0) || (i % 2 == 1 && j % 2 == 1))
                    Coordinates.Add(new Coordinate(i, j));
                }
            }
            Random rand = new Random();
            Coordinates = Coordinates.OrderBy(_ => rand.Next()).ToList();
        }
        public Coordinate DoStrategy(object data)
        {
            Grid grid = data as Grid;
            int x, y;
            foreach( Coordinate coor in Coordinates)
            {
                if( grid.GetTile(coor) == TileStatus.Unknown)
                {
                    return coor;
                }
            }
            //
            //Algorithm goes here
            //c = new Coordinate();
            return new Coordinate(-1,-1);
        }
    }

    public class StrategyCornerTile : IStrategy
    {
        public Coordinate c;
        public ShipOrientation orientation;
        public int direction;
        public int count;
        public StrategyCornerTile(int x,int y, int d, ShipOrientation o)
        {
            c= new Coordinate(x,y);
            orientation = o;
            direction = d;
            count = 1;
        }
        public Coordinate DoStrategy(object data)
        {
            Grid grid = data as Grid;
            //Until destroyed
            Coordinate nc;
            if (orientation == ShipOrientation.Horizontal)
            {
                nc = new Coordinate(c.Row, c.Column+direction * count);
            }
            else
            {
                nc = new Coordinate(c.Row + direction*count, c.Column);
            }
            count++;
            if (nc.Row >= 0 && nc.Row < grid.GetSize_x() &&
                nc.Column >= 0 && nc.Column < grid.GetSize_y() &&
                grid.GetTile(nc.Row,nc.Column) == TileStatus.Unknown) return nc;
            else return new Coordinate(-1,-1);
        }
    }

    public class StrategyDestroyTile : IStrategy
    {
        private int x;
        private int y;
        public StrategyDestroyTile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Coordinate DoStrategy(object data)
        {
            return new Coordinate(x, y );
        }
    }
    public class StrategySearchNextShipTile : IStrategy
    {
        public int x;
        public int y;
        private ShipOrientation orientation;
        private int corner; // values -1 0 1 
        private int direction;
        public StrategySearchNextShipTile(int x, int y)
        {
            this.x = x;
            this.y = y;
            direction = 0;
        }
        public Coordinate DoStrategy(object data)
        {
            Grid grid = data as Grid;
            if (grid.GetSize_x() != x + 1 && grid.GetTile(x+1,y) == TileStatus.Unknown)
            {
                return new Coordinate ( x+1, y );
            }
            if (grid.GetSize_y() != y + 1 && grid.GetTile(x, y + 1) == TileStatus.Unknown)
            {
                return new Coordinate ( x , y+1 );
            }
            if (x > 0 && grid.GetTile(x - 1, y) == TileStatus.Unknown)
            {
                return new Coordinate ( x - 1, y );
            }
            if (y > 0 && grid.GetTile(x, y - 1) == TileStatus.Unknown)
            {
                return new Coordinate ( x , y - 1 );
            }
            return new Coordinate ( -1, -1 );
        }
        

    }
}
