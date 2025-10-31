namespace Battleship.Server.Models.Builder
{
    public interface IBuilder
    {
        public Game Build();

        public void addGridSize();


    }
}
