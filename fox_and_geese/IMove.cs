using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fox_and_geese
{
    public interface IMove
    {
        /// <summary>
        /// Метод проверки мест для хода
        /// </summary>
        /// <param name="actual_position"></param>
        /// <returns>коллекция List<Position></returns>
        List<Position> CheckPossibleMoves(Position actual_position);

        /// <summary>
        /// Метод смены позиции при выполнении хода
        /// </summary>
        /// <param name="actual_position"></param>
        /// <returns>объект Position</returns>
        Position Step(Position actual_position);

        /// <summary>
        /// Метод показывает возможные места для хода
        /// </summary>
        /// <param name="actual_position"></param>
        /// <returns>коллекция List<Position></returns>
        List<Position> ShowPossibleMoves(Position actual_position);
    }
}
