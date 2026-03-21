using System.Collections.Generic;
using System.Linq;


namespace fox_and_geese
{
    public class GameRules
    {
        private static GameRules instance;
        private const int INITIAL_GEESE_COUNT = 13;

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

        public PlayerType CheckWinCondition(Board board)
        {
            var fox = board.GetFox();
            var geese = board.GetGeese();
            int currentGeeseCount = geese.Count;

            // Победа лисы: съела всех гусей
            if (currentGeeseCount == 0)
                return PlayerType.Fox;

            // Победа лисы: съела достаточно гусей (8 из 13)
            int capturedGeese = INITIAL_GEESE_COUNT - currentGeeseCount;
            if (capturedGeese >= 8)
                return PlayerType.Fox;

            // Проверка победы гусей: лиса не может сделать ход
            if (fox != null)
            {
                var foxMoves = fox.GetValidMoves(board);
                if (foxMoves.Count == 0)
                    return PlayerType.Goose;
            }

            // Проверка победы гусей: все гуси заблокированы
            bool anyGooseCanMove = geese.Any(goose => goose.GetValidMoves(board).Count > 0);
            if (!anyGooseCanMove && geese.Count > 0)
                return PlayerType.Goose;

            // Проверка победы лисы: достигла края креста
            if (fox != null && IsFoxAtEdge(board, fox))
                return PlayerType.Fox;

            // Игра продолжается
            return PlayerType.Fox; // Возвращаем Fox как "нет победителя"
        }

        private bool IsFoxAtEdge(Board board, Fox fox)
        {
            // Проверяем, достигла ли лиса края креста
            var pos = fox.Position;

            // Крайние позиции креста 3x3 на поле 7x7
            return pos.X == 0 || pos.X == 6 || pos.Y == 0 || pos.Y == 6;
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
    }
}
