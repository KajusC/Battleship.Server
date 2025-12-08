using Battleship.Server.Models;
using Battleship.Server.Models.Dtos;
using Battleship.Server.Services;
using System.Data.Common;

namespace Battleship.Server.Classes
{
    public abstract class ShotResolverAbstract
    {
        protected GameService gameService;
        protected Game game;
        protected string connectionId;
        protected bool changeTurn = true;
        public ShotResolverAbstract( GameService gameService,string connectionId)
        {
            this.gameService = gameService;
            this.connectionId = connectionId;
            //this.game = game;
        }
        public abstract List<Coordinate> GetCoordinates();
        public abstract void UseAmmunition(GamePlayer gamePlayer, int weaponType);

        public FireResult Resolve(ShotRequestDto request)
        {
            Game game = gameService.GetGameByConnection(connectionId);
            GamePlayer shooter = game.FindPlayerByConnection(connectionId);
            int WeaponType = request.WeaponType;
            Console.WriteLine("{0} {1}", "weapone",request.WeaponType); 
            if (game.Phase != GamePhase.InProgress)
            {
                throw new InvalidOperationException("Game is not active.");
            }

            if (game.CurrentTurnPlayerId != shooter.PlayerId)
            {
                throw new InvalidOperationException("It is not the player's turn.");
            }
            int Row = request.Row;
            int Column = request.Column;
            Coordinate coordinate = new Coordinate(Row, Column);
            var target = game.GetOpponent(shooter.PlayerId) ?? throw new InvalidOperationException("Opponent not found.");
            Console.WriteLine("testas");
            var resolution = target.Board.Shoot(coordinate);

            List<Coordinate> Coordinates = GetCoordinates();


            UseAmmunition(shooter, WeaponType);
            
            foreach (Coordinate co in Coordinates)
            {
                Coordinate c = new Coordinate(Row + co.Row, Column + co.Column);
                if (c.Row >= 0 && c.Row < game._boardSize && c.Column >= 0 && c.Column < game._boardSize)
                {
                    target.Board.Shoot(c, false, false);
                }
            }

            game.Scoreboard.RecordShot(shooter.PlayerId, target.PlayerId, resolution.Outcome);
            var isVictory = target.Board.AreAllShipsSunk;
            if (isVictory)
            {
                game.Phase = GamePhase.Completed;
                game.CurrentTurnPlayerId = shooter.PlayerId;
            }
            else if (resolution.Outcome.Equals(ShotOutcome.Hit) ||
                    resolution.Outcome.Equals(ShotOutcome.Sunk))
            {
                game.CurrentTurnPlayerId = shooter.PlayerId;
            }
            else
            {
                if (changeTurn)
                {
                    game.CurrentTurnPlayerId = target.PlayerId;
                }
                else
                {
                    game.CurrentTurnPlayerId = shooter.PlayerId;

                }
            }

            var result = new FireResult(shooter.PlayerId, target.PlayerId, coordinate, resolution, isVictory, game.CurrentTurnPlayerId,WeaponType);

            game.NotifyScoreboardChange();

            return result;
        }
    }
    public class ShotResolverWeapon0 : ShotResolverAbstract
    {
        public ShotResolverWeapon0(GameService gameService, string connectionId) : base(gameService, connectionId)
        {
        }
        public override List<Coordinate> GetCoordinates()
        {
            List<Coordinate> Coordinates = new List<Coordinate>();
            return Coordinates;
        }

        public override void UseAmmunition(GamePlayer shooter, int weaponType)
        {
            return;
        }
    }
    public class ShotResolverWeapon1 : ShotResolverAbstract
    {
        public ShotResolverWeapon1(GameService gameService, string connectionId) : base(gameService, connectionId)
        {

        }
        public override List<Coordinate> GetCoordinates()
        {
            List<Coordinate> Coordinates = new List<Coordinate>();
            
            Coordinates.Add(new Coordinate(0, 0 + 1));
            Coordinates.Add(new Coordinate(0 + 1, 0));
            Coordinates.Add(new Coordinate(0 - 1, 0));
            Coordinates.Add(new Coordinate(0, 0 - 1));
            
            return Coordinates;
        }

        public override void UseAmmunition(GamePlayer shooter, int weaponType)
        {
            if(shooter.GetWeaponAmmunition(weaponType)==0) throw new Exception("Out of ammo");
            shooter.UseWeaponAmmunition(weaponType);
        }
    }
    public class ShotResolverWeapon2 : ShotResolverAbstract
    {
        public ShotResolverWeapon2(GameService gameService, string connectionId) : base(gameService, connectionId)
        {

        }
        public override List<Coordinate> GetCoordinates()
        {
            List<Coordinate> Coordinates = new List<Coordinate>();

            Coordinates.Add(new Coordinate(0, 0 + 1));
            Coordinates.Add(new Coordinate(0 + 1, 0));
            Coordinates.Add(new Coordinate(0 - 1, 0));
            Coordinates.Add(new Coordinate(0, 0 - 1));
            Coordinates.Add(new Coordinate(0 + 1, 0 + 1));
            Coordinates.Add(new Coordinate(0 + 1, 0 - 1));
            Coordinates.Add(new Coordinate(0 - 1, 0 - 1));
            Coordinates.Add(new Coordinate(0 - 1, 0 + 1));

            return Coordinates;
        }

        public override void UseAmmunition(GamePlayer shooter, int weaponType)
        {
            if (shooter.GetWeaponAmmunition(weaponType) == 0) throw new Exception("Out of ammo");
            shooter.UseWeaponAmmunition(weaponType);
        }
    }
    public class ShotResolverWeapon3 : ShotResolverAbstract
    {
        public ShotResolverWeapon3(GameService gameService, string connectionId) : base(gameService, connectionId)
        {
            changeTurn = false;
        }
        public override List<Coordinate> GetCoordinates()
        {
            List<Coordinate> Coordinates = new List<Coordinate>();
            return Coordinates;
        }

        public override void UseAmmunition(GamePlayer shooter, int weaponType)
        {
            if (shooter.GetWeaponAmmunition(weaponType) == 0) throw new Exception("Out of ammo");
            shooter.UseWeaponAmmunition(weaponType);
        }
    }
}
