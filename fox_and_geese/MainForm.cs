using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fox_and_geese
{
    public partial class MainForm : Form
    {
        private Board gameBoard = new Board();
        private RoundButton[,] buttons = new RoundButton[7, 7];
        private Point? selected = null;
        private bool isFoxTurn = false;
        private Label statusLabel;
        public MainForm()
        {
            this.Size = new Size(520, 570);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.BackColor = Color.Gray;
            this.Text = "Лиса и гуси";
            this.StartPosition = FormStartPosition.CenterScreen;
            statusLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 50,
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White
            };
            this.Controls.Add(statusLabel);

            InitButtons();
            UpdateGUI();
        }

        private void InitButtons()
        {
            for (int row = 0; row < 7; row++)
            {
                for(int col = 0; col < 7; col++)
                {
                    if (gameBoard.IsAvailable(row, col))
                    {
                        var btn = new RoundButton
                        {
                            Size = new Size(60, 60),
                            Location = new Point(col * 60 + 40, row * 60 + 70),
                            Tag = new Point(col, row),
                            FlatStyle = FlatStyle.Flat,
                            BackColor = Color.White,
                            Font = new Font("Arial", 12, FontStyle.Bold)
                        };
                        btn.Click += OnCellClick;
                        buttons[row, col] = btn;
                        this.Controls.Add(btn);
                    }
                }
            }
        }

        private void OnCellClick(object sender, EventArgs e)
        {
            Point p = (Point)((Button)sender).Tag;

            if (selected == null)
            {
                string figure = gameBoard.GetFigure(p.X, p.Y);
                if (figure != null && ((isFoxTurn && figure == "Лиса") || (!isFoxTurn && figure == "Гусь")))
                {
                    selected = p;
                    HighlightMoves(p);
                }
                else
                {
                    if (ValidateAndMove(selected.Value, p))
                    {
                        isFoxTurn = !isFoxTurn;
                        UpdateGUI();
                        CheckWin();
                    }
                    ClearHighlight();
                    selected = null;
                }
            }
        }

        private bool ValidateAndMove(Point from, Point to)
        {
            if (gameBoard.GetFigure(to.X, to.Y) != null)
            {
                return false;
            }
            int dx = to.X - from.X;
            int dy = to.Y - from.Y;
            string p = gameBoard.GetFigure(from.X, from.Y);

            if (p == "Гусь" && Math.Abs(dx) + Math.Abs(dy) == 1 && dy >= 0)
            {
                gameBoard.Move(from, to);
                return true;
            }
            if (p == "Лиса")
            {
                if (Math.Abs(dx) <= 1 && Math.Abs(dy) <= 1)
                {
                    gameBoard.Move(from, to);
                    return true;
                }
                if (Math.Abs(dx) == 2 && dy == 0 || Math.Abs(dy) == 2 && dx == 0)
                {
                    Point mid = new Point(from.X + dx / 2, from.Y + dy / 2);
                    if (gameBoard.GetFigure(mid.X, mid.Y) == "Гусь")
                    {
                        gameBoard.Remove(mid.X, mid.Y);
                        gameBoard.Move(from, to);
                        return true;
                    }
                }
            }
            return false;
        }

        // временный клон логики для подсветки без совершения хода
        private bool ValidateAndMoveNoExec(Point from, Point to)
        {
            if (gameBoard.GetFigure(to.X, to.Y) != null)
            {
                return false;
            }
            int dx = to.X - from.X;
            int dy = to.Y - from.Y;
            string p = gameBoard.GetFigure(from.X, from.Y);
            if (p == "Гусь")
            {
                return Math.Abs(dx) + Math.Abs(dy) == 1 && dy >= 0;
            }
            if (p == "Лиса")
            {
                if (Math.Abs(dx) <= 1 && Math.Abs(dy) <= 1)
                {
                    return true;
                }
                if (Math.Abs(dx) == 2 && dy == 0 || Math.Abs(dy) == 2 && dx == 0)
                {
                    Point mid = new Point(from.X + dx / 2, from.Y + dy / 2);
                    return gameBoard.GetFigure(mid.X, mid.Y) == "Гусь";
                }
            }
            return false;
        }
        
        // подсветка возможных ходов
        private void HighlightMoves(Point from)
        {
            buttons[from.X, from.Y].BackColor = Color.Yellow;
            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (buttons[row, col] != null && ValidateAndMoveNoExec(from, new Point(row, col)))
                    {
                        buttons[row, col].BackColor = Color.LightGreen;
                    }
                }
            }
        }
        
        // убираем подстветку
        private void ClearHighlight()
        {
            foreach (var b in buttons)
            {
                if (b != null)
                {
                    b.BackColor = Color.White;
                }
            }
        }

        private void UpdateGUI()
        {
            for (int row = 0; row < 7; row++)
            {
                for (int col = 0;col < 7; col++)
                {
                    if (buttons[row, col] != null)
                    {
                        string p = gameBoard.GetFigure(row, col);
                        buttons[row, col].Text = p ?? "";
                        buttons[row, col].ForeColor = (p == "Лиса") ? Color.Red : Color.Blue;
                    }
                }
            }
            statusLabel.Text = $"ХОД: {(isFoxTurn ? "ЛИСА" : "ГУСИ")} | Гусей: {gameBoard.GeeseCount}";
        }

        private void CheckWin()
        {
            if (gameBoard.GeeseCount < 8)
            {
                MessageBox.Show("Победила лиса!");
                Application.Restart();
            }
            if (isFoxTurn && IsTrapped())
            {
                MessageBox.Show("Победили гуси!");
                Application.Restart();
            }
        }
        /// <summary>
        /// метод проверки невозможности хода (лиса заперта гусями)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private bool IsTrapped()
        {
            Point p = gameBoard.FindFox();
            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (buttons[row, col] != null && ValidateAndMoveNoExec(p, new Point(row, col)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
