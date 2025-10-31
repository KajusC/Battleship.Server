namespace Battleship.Server.Models.Builder
{
    public class StandartGameBuilder : IBuilder
    {
        private int gridSize;
        public void addGridSize()
        {
            gridSize = 10;
        }

        public Game Build()
        {
            return new Game(gridSize);
        }
    }
}
