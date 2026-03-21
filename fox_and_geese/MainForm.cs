using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace fox_and_geese
{
    public partial class MainForm : Form
    {
        private Game game;
        private Button[,] cells;
        private const int CellSize = 65;
        private Panel gamePanel;
        private Label statusLabel;
        private Button newGameButton;
        private Button undoButton;
        private Position selectedPosition;
        private Color enabledCellColor = Color.SandyBrown;
        private Color disabledCellColor = Color.DimGray;

        public MainForm()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            game = new Game();
            selectedPosition = null;
            CreateBoard();
            UpdateBoard();
            UpdateStatus();
        }

        private void CreateBoard()
        {
            cells = new Button[7, 7];
            gamePanel.Controls.Clear();

            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    Button cell = new Button
                    {
                        Size = new Size(CellSize, CellSize),
                        Location = new Point(col * CellSize, row * CellSize),
                        FlatStyle = FlatStyle.Flat,
                        Tag = new Position(row, col),
                        Font = new Font("Segoe UI Emoji", 28)
                    };

                    cell.FlatAppearance.BorderSize = 1;
                    cell.FlatAppearance.BorderColor = Color.Black;
                    cell.Click += Cell_Click;

                    // Проверяем, является ли клетка доступной для игры
                    var pos = new Position(row, col);
                    var tempBoard = new Board(7);
                    if (tempBoard.IsPositionValid(pos))
                    {
                        cell.BackColor = enabledCellColor;
                        cell.Enabled = true;
                    }
                    else
                    {
                        cell.BackColor = disabledCellColor;
                        cell.Enabled = false;
                        cell.Text = "✖";
                        cell.ForeColor = Color.DarkRed;
                    }

                    cells[row, col] = cell;
                    gamePanel.Controls.Add(cell);
                }
            }
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            if (game.IsGameOver())
            {
                MessageBox.Show("Игра окончена! Начните новую игру.", "Конец игры",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Button clickedCell = sender as Button;
            Position clickedPos = (Position)clickedCell.Tag;

            // Проверяем, доступна ли клетка
            if (!game.Board.IsPositionValid(clickedPos))
                return;

            if (selectedPosition == null)
            {
                // Выбор фигуры
                var piece = game.Board.GetPieceAt(clickedPos);
                if (piece != null && piece.Type == game.CurrentTurn)
                {
                    selectedPosition = clickedPos;
                    HighlightValidMoves(clickedPos);
                }
                else if (piece != null)
                {
                    MessageBox.Show($"Сейчас ходят {GetPlayerName(game.CurrentTurn)}!",
                        "Не ваша очередь", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Попытка сделать ход
                var piece = game.Board.GetPieceAt(selectedPosition);
                if (piece != null)
                {
                    var move = new Move(piece, selectedPosition, clickedPos, null);

                    // Проверяем, есть ли захват
                    if (piece is Fox fox)
                    {
                        var captureMoves = fox.GetCaptureMoves(game.Board);
                        var captureMove = captureMoves.FirstOrDefault(m => m.To.Equals(clickedPos));
                        if (captureMove != null)
                            move = captureMove;
                    }

                    if (game.MakeMove(move))
                    {
                        selectedPosition = null;
                        ClearHighlights();
                        UpdateBoard();
                        UpdateStatus();

                        if (game.IsGameOver())
                        {
                            ShowGameOverMessage();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Недопустимый ход!\n" +
                            "Гуси ходят только вперед по диагонали.\n" +
                            "Лиса ходит по диагонали и может есть гусей.",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        selectedPosition = null;
                        ClearHighlights();
                    }
                }
            }
        }

        private void HighlightValidMoves(Position pos)
        {
            ClearHighlights();

            var piece = game.Board.GetPieceAt(pos);
            if (piece == null) return;

            var validMoves = piece.GetValidMoves(game.Board);
            foreach (var move in validMoves)
            {
                if (game.Board.IsPositionValid(move.To))
                {
                    cells[move.To.X, move.To.Y].BackColor = Color.LightGreen;
                    cells[move.To.X, move.To.Y].FlatAppearance.BorderColor = Color.Green;
                    cells[move.To.X, move.To.Y].FlatAppearance.BorderSize = 3;
                }
            }

            // Подсвечиваем выбранную фигуру
            cells[pos.X, pos.Y].BackColor = Color.Gold;
            cells[pos.X, pos.Y].FlatAppearance.BorderColor = Color.Orange;
            cells[pos.X, pos.Y].FlatAppearance.BorderSize = 3;
        }

        private void ClearHighlights()
        {
            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    var pos = new Position(row, col);
                    if (game.Board.IsPositionValid(pos))
                    {
                        cells[row, col].BackColor = enabledCellColor;
                    }
                    else
                    {
                        cells[row, col].BackColor = disabledCellColor;
                    }
                    cells[row, col].FlatAppearance.BorderSize = 1;
                    cells[row, col].FlatAppearance.BorderColor = Color.Black;
                }
            }
        }

        private void UpdateBoard()
        {
            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    var pos = new Position(row, col);
                    var piece = game.Board.GetPieceAt(pos);

                    if (piece != null && game.Board.IsPositionValid(pos))
                    {
                        cells[row, col].Text = piece.Type == PlayerType.Fox ? "🦊" : "🦆";
                        cells[row, col].ForeColor = piece.Type == PlayerType.Fox ? Color.OrangeRed : Color.SaddleBrown;
                    }
                    else if (game.Board.IsPositionValid(pos))
                    {
                        cells[row, col].Text = "●";
                        cells[row, col].ForeColor = Color.DarkGoldenrod;
                        cells[row, col].Font = new Font("Segoe UI Emoji", 20);
                    }
                    else
                    {
                        cells[row, col].Text = "✖";
                        cells[row, col].ForeColor = Color.DarkRed;
                        cells[row, col].Font = new Font("Segoe UI Emoji", 24);
                    }
                }
            }
        }

        private void UpdateStatus()
        {
            if (game.IsGameOver())
            {
                var winner = game.GetWinner();
                statusLabel.Text = winner == PlayerType.Fox ?
                    "🦊 Лиса победила! 🏆" : "🦆 Гуси победили! 🏆";
                statusLabel.ForeColor = winner == PlayerType.Fox ? Color.Red : Color.Green;
                statusLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            }
            else
            {
                statusLabel.Text = game.CurrentTurn == PlayerType.Fox ?
                    "🦊 Ход лисы" : "🦆 Ход гусей";
                statusLabel.ForeColor = Color.Black;
                statusLabel.Font = new Font("Arial", 12, FontStyle.Bold);
            }
        }

        private void ShowGameOverMessage()
        {
            var winner = game.GetWinner();
            string message = winner == PlayerType.Fox ?
                "🦊 Лиса победила! Поздравляем! 🎉" :
                "🦆 Гуси победили! Отличная стратегия! 🎉";
            MessageBox.Show(message, "Конец игры", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string GetPlayerName(PlayerType player)
        {
            return player == PlayerType.Fox ? "лиса 🦊" : "гуси 🦆";
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            InitializeGame();
            UpdateStatus();
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            if (!game.IsGameOver())
            {
                game.UndoMove();
                selectedPosition = null;
                ClearHighlights();
                UpdateBoard();
                UpdateStatus();
            }
            else
            {
                MessageBox.Show("Игра уже окончена. Начните новую игру.",
                    "Нельзя отменить", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
