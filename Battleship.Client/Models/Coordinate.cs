using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Client.Models
{
    class Coordinate
    {
        public Coordinate(int Row, int Collumn) {
            this.Row = Row;
            this.Column = Collumn;
        }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
