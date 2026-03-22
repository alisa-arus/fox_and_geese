using System.Collections.Generic;
using System.Linq;


namespace fox_and_geese
{
    public class GameRules
    {
        private static GameRules instance;
        private const int INITIAL_GEESE_COUNT = 13;
        private const int FOX_WIN_CONDITION = 8; // Лиса побеждает, когда остается 8 гусей (съедено 5)

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
            //int currentGeeseCount = geese.Count;

            // Проверка победы лисы: осталось 8 или меньше гусей
            // Важно: проверяем ТОЛЬКО если игра активна и был совершен хотя бы один ход
            if (geese.Count <= FOX_WIN_CONDITION)
            {
                return PlayerType.Fox;
            }

            // Проверка победы гусей: лиса не может сделать ход
            if (fox.GetValidMoves(board).Count == 0)
            {
                //var foxMoves = fox.GetValidMoves(board);
                //if (foxMoves.Count == 0)
                //{
                    return PlayerType.Goose;
                //}
            }

            // Игра продолжается
            return null;
        }

        //public bool IsFoxWinByGeeseCount(int geeseCount)
        //{
        //    return geeseCount <= FOX_WIN_CONDITION;
        //}

        public int GetGeeseToCapture()
        {
            return INITIAL_GEESE_COUNT - FOX_WIN_CONDITION;
        }

        public List<Move> GetAvailableMoves(Board board, PlayerType player)
        {
            var moves = new List<Move>();

            if (player == PlayerType.Fox)
            {
                var fox = board.GetFox();
                if (fox != null)
                    moves.AddRange(fox.GetValidMoves(board));
            }
            else
            {
                foreach (var goose in board.GetGeese())
                    moves.AddRange(goose.GetValidMoves(board));
            }

            return moves;
        }

        public bool IsPositionOnBoard(Position pos, Board board)
        {
            return board.IsPositionValid(pos);
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
