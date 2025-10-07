using Battleship.Server.Enums;
using Battleship.Server.Models;

namespace Battleship.Server.Classes
{
    public class Grid
    {   
        private int size_x;
        private int size_y;
        private TileStatus[ , ] Tiles;
        public Grid(int x, int y, TileStatus tileStatus) { 
            size_x = x;
            size_y = y;
            Tiles = new TileStatus[ x, y ];
            for( int i = 0; i < x; i++ ) 
            {
                for (int j = 0; j < y; j++)
                {
                    Tiles[i, j] = tileStatus;
                }
            }
        }
        public void SetTile(int x, int y, TileStatus status)
        {
            Tiles[x, y] = status;
        }
        public void SetTiles(int x, int y, TileStatus status, ShipOrientation direction, int length)
        {
            for (int i = 0; i < length; i++)
            {
                if(direction == ShipOrientation.Horizontal)
                    Tiles[x, y+i] = status;
                else if(direction == ShipOrientation.Vertical)
                    Tiles[x+i, y] = status;

            }
        }
        public TileStatus GetTile(int x, int y)
        {
            return Tiles[x, y];
        }
        public int GetSize_x()
        {
            return size_x;
        }
        public int GetSize_y()
        {
            return size_y;
        }
    }
}
