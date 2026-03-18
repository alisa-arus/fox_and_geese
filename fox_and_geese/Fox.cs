using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fox_and_geese
{
    public class Fox : Player, IMove
    {
        public List<Position> CheckPossibleMoves(Position actual_position)
        {
            throw new NotImplementedException();
        }

        public List<Position> ShowPossibleMoves(Position actual_position)
        {
            List<Position> possible_moves = CheckPossibleMoves(actual_position);
            foreach (var pos in possible_moves)
            {
                MessageBox.Show(pos.ToString());
            }
            return possible_moves;
        }

        public Position Step(Position actual_position)
        {
            return new Position();
        }
    }
}
