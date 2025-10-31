using Battleship.Server.Classes;
using Battleship.Server.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Client.Classes
{



    // The Strategy interface declares operations common to all supported
    // versions of some algorithm.
    //
    // The NPC uses this interface to call the algorithm defined by Concrete
    // Strategies.

    // Concrete Strategies implement the algorithm while following the base
    // Strategy interface. The interface makes them interchangeable in the
    // NPC.
    //public class Game
    //{
    //    int[,] grid;

    //    int shipTiles;
    //    public Game(int[,] Grid) { 
    //        this.grid = Grid;
    //        shipTiles = 0;
    //        for (int i = 0; i < grid.GetLength(0); i++)
    //        {
    //            for (int j = 0; j < grid.GetLength(0); j++)
    //            {
    //                if(grid[i, j] >= 1)
    //                {
    //                    shipTiles++;
    //                }
    //            }
    //        }
    //    }
    //    public TileStatus GetResponse(int x, int y)
    //    {
    //        if (grid[x, y] == -1) return TileStatus.Empty;
            
            
    //        if (grid[x, y] >= 2) 
    //        {
    //            grid[x, y] -= 1;
    //            return TileStatus.Damaged; 
    //        }
    //        else
    //        {
    //            grid[x, y] = 0;
    //            shipTiles--;
    //            if(CheckGameOver()) return TileStatus.GameEnd;
    //            return TileStatus.Destroyed;

    //        }
    //    }

    //    public bool CheckGameOver()
    //    {
    //        if(shipTiles== 0) return true;
    //        return false;
    //    }
    //}
    //class Program2
    //{
    //    static void Main2(string[] args)
    //    {
    //        var NPC = new NPC();
            
    //        int[,] grid1 =  { { -1, -1, 3}, { -1, -1, 3 } , { -1, -1, 3 } };
    //        Game game = new Game(grid1);
    //        int[,] grid2 = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, };
    //        Console.WriteLine("Client: Strategy is set to ConcreteStrategySearchRandom");
    //        NPC.SetStrategy(new ConcreteStrategySearchRandom());
    //        NPC.SetGrid1(grid1);
    //        NPC.SetEnemyGrid(3,3);
    //        NPC.SetMyGrid(3, 3);

    //        NPC.SetGame(game);

    //        bool lastShipTile = false;
    //        bool gameEnd = false;
    //        int i = 0;
    //        while (NPC.DoStrategy())
    //        {
    //        }
    //    }
    //}
}


