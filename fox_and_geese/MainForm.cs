using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace fox_and_geese
{
    public partial class MainForm : Form
    {
        private const int CELL_SIZE = 65;
        private Color enabledCellColor = Color.SandyBrown;

        private Game game;
        private Button newGameButton;
        private Button undoButton;
        private Label statusLabel;
        private Label geeseCountLabel;
        private Label captureCountLabel;
        private Panel gamePanel;
        private Position selectedPosition;
        private RoundButton[,] cells;

        public MainForm()
        {
            InitializeComponent();
            InitializeUIElements();
            InitializeGame();
        }

        private void InitializeUIElements()
        {
            gamePanel = new Panel
            {
                Location = new Point(25, 35),
                Size = new Size(7 * CELL_SIZE + 10, 7 * CELL_SIZE + 10),
                BackColor = Color.DimGray
            };

            statusLabel = new Label
            {
                Location = new Point(20, 10),
                Size = new Size(400, 30),
                Font = new Font("Arial", 12, FontStyle.Bold),
                Text = "Ход гусей",
            };

            geeseCountLabel = new Label
            {
                Location = new Point(25, 7 * CELL_SIZE + 60),
                Size = new Size(200, 25),
                Font = new Font("Arial", 10),
                Text = "Осталось гусей: 13",
                ForeColor = Color.White
            };

            captureCountLabel = new Label
            {
                Location = new Point(25, 7 * CELL_SIZE + 85),
                Size = new Size(200, 25),
                Font = new Font("Arial", 10),
                Text = "Съедено гусей: 0",
                ForeColor = Color.White
            };

            newGameButton = new Button
            {
                Location = new Point(300, 7 * CELL_SIZE + 60),
                Size = new Size(100, 35),
                Text = "Новая игра",
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            newGameButton.Click += NewGameButton_Click;

            undoButton = new Button
            {
                Location = new Point(400, 7 * CELL_SIZE + 60),
                Size = new Size(80, 35),
                Text = "Отмена",
                BackColor = Color.LightCoral,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            undoButton.Click += UndoButton_Click;

            this.Controls.Add(gamePanel);
            this.Controls.Add(statusLabel);
            this.Controls.Add(geeseCountLabel);
            this.Controls.Add(captureCountLabel);
            this.Controls.Add(newGameButton);
            this.Controls.Add(undoButton);
        }

        private void InitializeGame()
        {
            game = new Game();
            selectedPosition = null;
            CreateBoard();
            UpdateBoard();
            UpdateStatus();
            UpdateCounters();
        }

        private void CreateBoard()
        {
            cells = new RoundButton[7, 7];
            gamePanel.Controls.Clear();

            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    RoundButton cell = new RoundButton
                    {
                        Size = new Size(CELL_SIZE, CELL_SIZE),
                        Location = new Point(col * CELL_SIZE, row * CELL_SIZE),
                        FlatStyle = FlatStyle.Flat,
                        Tag = new Position(row, col),
                        Font = new Font("Segoe UI Emoji", 28)
                    };

                    cell.Click += Cell_Click;

                    // проверяем доступность поля для игры
                    var pos = new Position(row, col);
                    var tempBoard = new Board(7);
                    if (tempBoard.IsPositionValid(pos))
                    {
                        cell.BackColor = enabledCellColor;
                        cell.Enabled = true;
                    }
                    else
                    {
                        cell.Visible = false;
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
                MessageBox.Show("Игра окончена! Начните новую игру.",
                                "Конец игры",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            RoundButton clickedCell = sender as RoundButton;
            Position clickedPos = (Position)clickedCell.Tag;

            if (!game.Board.IsPositionValid(clickedPos))
                return;

            if (selectedPosition == null)
            {
                var piece = game.Board.GetPieceAt(clickedPos);
                if (piece != null && piece.Type == game.CurrentTurn)
                {
                    selectedPosition = clickedPos;
                    HighlightValidMoves(clickedPos);
                }
                else if (piece != null)
                {
                    MessageBox.Show($"Сейчас ходят {GetPlayerName(game.CurrentTurn)}!",
                                    "Не ваша очередь",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                }
            }
            else
            {
                var piece = game.Board.GetPieceAt(selectedPosition);
                if (piece != null)
                {
                    var move = new Move(piece, selectedPosition, clickedPos, null);

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
                        UpdateCounters();

                        if (game.IsGameOver())
                        {
                            ShowGameOverMessage();
                        }
                        else if (game.IsCaptureSequence())
                        {
                            int captureCount = game.GetCaptureCount();
                            statusLabel.Text = $"Лиса рубит! Съедено гусей: {captureCount} (осталось: {game.Board.GetGeeseCount()})";
                            statusLabel.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        string errorMessage = "Недопустимый ход!\n\n" +
                                              "Гуси ходят только по горизонтали и вертикали\n" +
                                              "Лиса ходит по горизонтали, вертикали и диагонали\n" +
                                              "Лиса может рубить гусей, перепрыгивая через них\n" +
                                              "При рубке лиса может ходить несколько раз подряд\n" +
                                              "Цель лисы: есть гусей, пока не останется 8 шт.)\n" +
                                              "Цель гусей: заблокировать лису";

                        MessageBox.Show(errorMessage,
                                        "Ошибка",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
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

                    // если ход - рубка, то добавляем специальный индикатор
                    if (move.CapturedPiece != null)
                    {
                        cells[move.To.X, move.To.Y].Text = "🔥";
                        cells[move.To.X, move.To.Y].Font = new Font("Segoe UI Emoji", 20);
                    }
                    else
                    {
                        cells[move.To.X, move.To.Y].Text = "";
                    }
                }
            }

            // подсвечиваем выбранную фигуру
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
                    cells[row, col].BackColor = enabledCellColor;
                    cells[row, col].FlatAppearance.BorderSize = 1;
                    cells[row, col].FlatAppearance.BorderColor = Color.Black;
                    cells[row, col].Text = "";
                    cells[row, col].Font = new Font("Segoe UI Emoji", 28);
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
                    else
                    {
                        cells[row, col].Text = "●";
                        cells[row, col].ForeColor = Color.DarkGoldenrod;
                        cells[row, col].Font = new Font("Segoe UI Emoji", 20);
                    }
                }
            }
        }

        private void UpdateStatus()
        {
            if (game.IsGameOver())
            {
                var winner = game.GetWinner();
                statusLabel.Text = winner == PlayerType.Fox ? "Лиса победила!" : "Гуси победили!";
                statusLabel.ForeColor = winner == PlayerType.Fox ? Color.Red : Color.Green;
            }
            else
            {
                statusLabel.Text = game.CurrentTurn == PlayerType.Fox ? "Ход лисы" : "Ход гусей";
                statusLabel.ForeColor = Color.White;
            }
        }

        private void UpdateCounters()
        {
            geeseCountLabel.Text = $"Осталось гусей: {game.Board.GetGeeseCount()}";
            captureCountLabel.Text = $"Съедено гусей: {game.GetCapturedGeeseCount()}";

            // когда лисе остался один гусь до победы
            if (game.GetCapturedGeeseCount() >= 4)
            {
                captureCountLabel.ForeColor = Color.Red;
            }
        }


        private void ShowGameOverMessage()
        {
            var winner = game.GetWinner();
            string message = winner == PlayerType.Fox ?
                $"Лиса победила!\n\nСъедено гусей: {game.GetCapturedGeeseCount()}" :
                $"Гуси победили!\n\nОсталось гусей: {game.Board.GetGeeseCount()}";

            MessageBox.Show(message,
                            "Конец игры",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        private string GetPlayerName(PlayerType player)
        {
            return player == PlayerType.Fox ? "лиса" : "гуси";
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
                UpdateCounters();
            }
            else
            {
                MessageBox.Show("Игра уже окончена. Начните новую игру.",
                                "Нельзя отменить",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }
    }
}
