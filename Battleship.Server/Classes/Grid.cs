using Battleship.Server.Enum;

namespace Battleship.Server.Classes
{
    public class Grid
    {   
        private int size_x;
        private int size_y;
        private TileStatus[ , ] Tiles;
        public Grid(int x, int y, TileStatus tileStatus) { 
            
            Tiles = new TileStatus[ x, y ];
            for( int i = 0; i < x; i++ ) 
            {
                for (int j = 0; j < y; j++)
                {
                    Tiles[i, j] = tileStatus;
                }
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
