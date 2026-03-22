using System.Collections.Generic;
using System.Linq;


namespace fox_and_geese
{
    public class GameRules
    {
        private static GameRules instance;
        private const int INITIAL_GEESE_COUNT = 13;
        private const int FOX_WIN_CONDITION = 8;

        private GameRules() { }

        public static GameRules Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameRules();
                return instance;
            }
        }

        public bool IsMoveValid(Move move, Board board, PlayerType currentTurn)
        {
            if (move.MovedPiece.Type != currentTurn)
                return false;

            var validMoves = move.MovedPiece.GetValidMoves(board);
            return validMoves.Any(m => m.To.Equals(move.To));
        }

        public PlayerType? CheckWinCondition(Board board)
        {
            var fox = board.GetFox();
            var geese = board.GetGeese();

            // проверяем победу лисы (осталось 8 или меньше гусей)
            if (geese.Count <= FOX_WIN_CONDITION)
            {
                return PlayerType.Fox;
            }
            // проверяем победу гусей (лиса заблокирована)
            if (fox.GetValidMoves(board).Count == 0)
            {
                return PlayerType.Goose;
            }
            // продолжаем игру
            return null;
        }

        public int GetGeeseToCapture()
        {
            return INITIAL_GEESE_COUNT - FOX_WIN_CONDITION;
        }

        public int GetInitialGeeseCount()
        {
            return INITIAL_GEESE_COUNT;
        }

        public int GetFoxWinCondition()
        {
            return FOX_WIN_CONDITION;
        }
    }
}
