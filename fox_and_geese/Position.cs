using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fox_and_geese
{
    public class Position
    {
        private byte x;
        private byte y;
        private bool occupied;

        public byte X { get => x; set => x = value; }
        public byte Y { get => y; set => y = value; }
        public bool Occupied { get => occupied; set => occupied = value; }
        public Position(byte _x, byte _y, bool _occ)
        {
            this.x = _x;
            this.y = _y;
            this.occupied = _occ;
        }
        public Position(byte _x, byte _y) : this(_x, _y, false)
        {
            
        }
        public Position() : this(2, 0, false)
        {
            
        }
    }
}
