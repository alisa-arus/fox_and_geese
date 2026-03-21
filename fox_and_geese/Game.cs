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
        private bool isCaptureSequence; // Флаг для последовательности захватов
        private int lastCaptureCount; // Счетчик захватов в текущей последовательности

        public Game()
        {
            Board = new Board(7); // Поле 7x7 с крестом 3x3 в середине
            CurrentTurn = PlayerType.Goose; // Гуси ходят первыми
            State = GameState.Active;
            rules = GameRules.Instance;
            moveHistory = new Stack<Move>();
            isCaptureSequence = false;
            lastCaptureCount = 0;
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

            // Запоминаем, был ли захват
            bool wasCapture = move.CapturedPiece != null;

            // Проверяем дополнительные захваты для лисы
            if (CurrentTurn == PlayerType.Fox && wasCapture)
            {
                var fox = Board.GetFox();
                var additionalCaptures = fox.GetCaptureMoves(Board);

                if (additionalCaptures.Any())
                {
                    // Если есть дополнительные захваты, лиса ходит снова
                    isCaptureSequence = true;
                    lastCaptureCount++;
                    return true;
                }
                else
                {
                    lastCaptureCount++;
                }
            }

            isCaptureSequence = false;
            lastCaptureCount = 0;

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
                // Отменяем все ходы в последовательности захвата
                while (moveHistory.Count > 0)
                {
                    var lastMove = moveHistory.Pop();
                    lastMove.Undo(Board);

                    // Если предыдущий ход был сделан другим игроком, останавливаемся
                    if (moveHistory.Count > 0 && moveHistory.Peek().MovedPiece.Type != lastMove.MovedPiece.Type)
                        break;
                }

                CurrentTurn = moveHistory.Count > 0 ?
                    moveHistory.Peek().MovedPiece.Type : PlayerType.Goose;
                State = GameState.Active;
                isCaptureSequence = false;
                lastCaptureCount = 0;
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

        public bool IsCaptureSequence()
        {
            return isCaptureSequence;
        }

        public int GetCaptureCount()
        {
            return lastCaptureCount;
        }
    }
}
