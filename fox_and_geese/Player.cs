using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fox_and_geese
{
    public abstract class Player
    {
        private Position current_position;
        private List<Position> possible_moves;

        public Position Current_position { get => current_position; set => current_position = value; }
        public List<Position> Possible_moves { get => possible_moves; set => possible_moves = value; }
        protected Player(Position cur_pos, List<Position> pos_mov)
        {
            this.current_position = cur_pos;
            this.possible_moves = pos_mov;
        }
    }
}
