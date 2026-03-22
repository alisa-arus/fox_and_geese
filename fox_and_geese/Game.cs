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
        private bool isCaptureSequence;
        private int lastCaptureCount;
        private bool isFirstMove;

        public Game()
        {
            Board = new Board(7);
            CurrentTurn = PlayerType.Goose; // определяем, кто будет ходить первым
            State = GameState.Active;
            rules = GameRules.Instance;
            moveHistory = new Stack<Move>();
            isCaptureSequence = false;
            lastCaptureCount = 0;
            isFirstMove = true;
        }

        public bool MakeMove(Move move)
        {
            if (State != GameState.Active)
                return false;

            if (!rules.IsMoveValid(move, Board, CurrentTurn))
                return false;

            // выполняем ход
            move.Execute(Board);
            moveHistory.Push(move);

            bool wasCapture = move.CapturedPiece != null;

            //  после первого хода сбрасываем его флаг
            if (isFirstMove)
            {
                isFirstMove = false;
            }

            // проверяем возможность дополнительной рубки гусей
            if (CurrentTurn == PlayerType.Fox && wasCapture)
            {
                var fox = Board.GetFox();
                var additionalCaptures = fox.GetCaptureMoves(Board);

                if (additionalCaptures.Any())
                {
                    isCaptureSequence = true;
                    lastCaptureCount++;
                    // не проверяем победу во время серийной рубки
                    return true;
                }
                else
                {
                    lastCaptureCount++;
                }
            }

            isCaptureSequence = false;

            // меняем игрока
            CurrentTurn = CurrentTurn == PlayerType.Fox ? PlayerType.Goose : PlayerType.Fox;

            // проверяем победу только после завершения хода и смены игрока
            var winner = rules.CheckWinCondition(Board);
            if (winner != null)
            {
                if (winner == PlayerType.Fox)
                {
                    State = GameState.FoxWon;
                }
                else
                {
                    State = GameState.GeeseWon;
                }
            }

            lastCaptureCount = 0;

            return true;
        }

        public void UndoMove()
        {
            if (moveHistory.Count > 0)
            {
                while (moveHistory.Count > 0)
                {
                    var lastMove = moveHistory.Pop();
                    lastMove.Undo(Board);

                    if (moveHistory.Count > 0 && moveHistory.Peek().MovedPiece.Type != lastMove.MovedPiece.Type)
                        break;
                }

                CurrentTurn = moveHistory.Count > 0 ?
                    moveHistory.Peek().MovedPiece.Type : PlayerType.Goose;
                State = GameState.Active;
                isCaptureSequence = false;
                lastCaptureCount = 0;
                isFirstMove = moveHistory.Count == 0;
            }
        }

        public PlayerType GetWinner()
        {
            if (State == GameState.FoxWon)
                return PlayerType.Fox;
            if (State == GameState.GeeseWon)
                return PlayerType.Goose;
            return PlayerType.Fox;
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

        public int GetCapturedGeeseCount()
        {
            return rules.GetInitialGeeseCount() - Board.GetGeeseCount();
        }
    }
}
