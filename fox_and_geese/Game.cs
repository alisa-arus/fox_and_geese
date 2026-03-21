using System.Collections.Generic;
using System.Linq;


namespace fox_and_geese
{
    public class Game
    {
        public Board Board { get; private set; }
        public PlayerType CurrentTurn { get; private set; }
        public GameState State { get; private set; }
        private GameRules rules;
        private Stack<Move> moveHistory;

        public Game()
        {
            Board = new Board(7); // Поле 7x7 в виде креста
            CurrentTurn = PlayerType.Goose; // Гуси ходят первыми
            State = GameState.Active;
            rules = GameRules.Instance;
            moveHistory = new Stack<Move>();
        }

        public bool MakeMove(Move move)
        {
            if (State != GameState.Active)
                return false;

            if (!rules.IsMoveValid(move, Board, CurrentTurn))
                return false;

            // Выполняем ход
            move.Execute(Board);
            moveHistory.Push(move);

            // Проверяем дополнительные захваты для лисы
            if (CurrentTurn == PlayerType.Fox && move.CapturedPiece != null)
            {
                var additionalCaptures = (move.MovedPiece as Fox).GetCaptureMoves(Board);
                if (additionalCaptures.Any())
                {
                    // Если есть дополнительные захваты, лиса ходит снова
                    return true;
                }
            }

            // Смена игрока
            CurrentTurn = CurrentTurn == PlayerType.Fox ? PlayerType.Goose : PlayerType.Fox;

            // Проверка победы
            var winner = rules.CheckWinCondition(Board);
            if (winner == PlayerType.Fox)
                State = GameState.FoxWon;
            else if (winner == PlayerType.Goose)
                State = GameState.GeeseWon;

            return true;
        }

        public void UndoMove()
        {
            if (moveHistory.Count > 0)
            {
                var lastMove = moveHistory.Pop();
                lastMove.Undo(Board);
                CurrentTurn = lastMove.MovedPiece.Type;
                State = GameState.Active;
            }
        }

        public PlayerType GetWinner()
        {
            if (State == GameState.FoxWon)
                return PlayerType.Fox;
            if (State == GameState.GeeseWon)
                return PlayerType.Goose;
            return PlayerType.Fox; // Игра не окончена
        }

        public bool IsGameOver()
        {
            return State != GameState.Active;
        }
    }
}
