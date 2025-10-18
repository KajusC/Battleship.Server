namespace Battleship.Server.Models.Builder
{
    public class AdvancedGameBuilder : IBuilder
    {
        private int gridSize;
        public void addGridSize()
        {
            gridSize = 12;
        }

        public Game Build()
        {
            return new Game(gridSize);
        }
    }
}
