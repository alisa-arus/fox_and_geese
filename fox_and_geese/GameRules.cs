using System.Collections.Generic;
using System.Linq;


namespace fox_and_geese
{
    public class GameRules
    {
        private static GameRules instance;

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

            // Победа лисы: достигла противоположной стороны (нижней части креста)
            if (fox.Position.X >= 5 && fox.Position.Y >= 2 && fox.Position.Y <= 4)
                return PlayerType.Fox;

            // Победа лисы: съела достаточно гусей (обычно 5-6 в классической версии)
            int initialGeeseCount = 17; // Начальное количество гусей
            int capturedGeese = initialGeeseCount - geese.Count;
            if (capturedGeese >= 6)
                return PlayerType.Fox;

            // Проверка победы гусей: лиса не может сделать ход
            var foxMoves = fox.GetValidMoves(board);
            if (foxMoves.Count == 0)
                return PlayerType.Goose;

            // Проверка победы гусей: все гуси заблокированы (редкий случай)
            bool anyGooseCanMove = geese.Any(goose => goose.GetValidMoves(board).Count > 0);
            if (!anyGooseCanMove && geese.Count > 0)
                return PlayerType.Fox;

            // Игра продолжается
            return PlayerType.Fox; // Возвращаем Fox как "нет победителя"
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
    }
}
