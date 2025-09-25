namespace Battleship.Server.Models;

public sealed class GamePlayer
{
    public GamePlayer(string playerId, string connectionId, string name, Board board)
    {
        PlayerId = playerId;
        ConnectionId = connectionId;
        Name = name;
        Board = board;
    }

    public string PlayerId { get; }
    public string ConnectionId { get; }
    public string Name { get; }
    public Board Board { get; }
    public bool IsReady { get; private set; }

    public void SetReady() => IsReady = true;
}
