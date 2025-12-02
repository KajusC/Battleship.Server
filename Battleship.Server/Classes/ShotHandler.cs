using Battleship.Server.Models.Dtos;
using Battleship.Server.Services;

namespace Battleship.Server.Classes
{
    public abstract class ShotHandler
    {
        protected static GameService gameService;

        protected ShotHandler next;
        protected double value;

        bool valid;
        protected void setNext(ShotHandler next) { this.next = next; }

        void handle(ShotRequestDto req)
        {
            if (process(req) && next != null)
            {
                next.handle(req);
            }
        }

        public abstract bool process(ShotRequestDto req);
    }
    class ValidateShot : ShotHandler
    {

        public override bool process(ShotRequestDto req)
        {
            setNext(new ValidateBoundsHandler());
            return true;
        }
    }

    class ValidateBoundsHandler : ShotHandler
    {
        public override bool process(ShotRequestDto req) {
            setNext(new CheckAlreadyShotHandler());
            return req.Row >= 0 && req.Column >= 0;
        }
    }

    class CheckAlreadyShotHandler : ShotHandler
    {
        public override bool process(ShotRequestDto req) {
            return true;
            //return !req.board[req.x][req.y].wasShot;
        }
    }

    class HitDetectionHandler : ShotHandler
    {
        public override bool process(ShotRequestDto req) {
            // tikrinti, ar pataikė į laivą
            return true;
        }
    }
}
