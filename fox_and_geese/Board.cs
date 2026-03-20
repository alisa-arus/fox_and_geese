using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fox_and_geese
{
    public class Board
    {
        // игровое поле
        private string[,] grid = new string[7, 7];
        // гуси
        private int geeseCount = 13;
        public int GeeseCount
        {
            get => geeseCount;
            private set
            {
                if (value < 1)
                {
                    geeseCount = 1;
                }
                else if (value > 13)
                {
                    geeseCount = 13;
                }
                else
                {
                    geeseCount = value;
                }
            }
        }

        public Board()
        {
            InitializeBoard();
        }
        // первичная расстановка фигур на поле
        private void InitializeBoard()
        {
            for (int row = 0; row < 7; row++)
            {
                for(int col = 0; col < 7; col++)
                {
                    if (IsAvailable(row, col))
                    {
                        // расставляем гусей
                        if (row < 3 || (row == 3 && (col < 2 || col > 4)))
                        {
                            grid[row, col] = "Гусь";
                        }
                        // лиса в центре
                        else if (row == 3 && col == 3)
                        {
                            grid[row, col] = "Лиса";
                        }
                        // пустые игровые поля
                        else
                        {
                            grid[row, col] = "пусто";
                        }
                    }
                    // позиции вне игрового поля
                    else
                    {
                        grid[row, col] = null;
                    }
                }
            }
        }
        /// <summary>
        /// Метод проверяет принадлежность позиции игровому полю (нахождение внутри "креста")
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool IsAvailable(int row, int col)
        {
            if (row < 0 || row >= 7 || col < 0 || col >= 7)
            {
                return false;
            }
            return (col >= 2 && col <= 4) || (row >= 2 && row <= 4);
        }

        // получение названия фигуры по координатам
        public string GetFigure(int row, int col)
        {
            return IsAvailable(row, col) ? grid[row, col] : "пусто";
        }

        // перемещение фигуры
        public void Move(Point from, Point to)
        {
            grid[to.X, to.Y] = grid[from.X, from.Y];
            grid[from.X, from.Y] = null;
        }

        // удаление фигуры (лиса рубит гуся)
        public void RemoveFigure(Point pos)
        {
            if (IsAvailable(pos.X, pos.Y))
            {
                grid[pos.X, pos.Y] = null;
            }
        }

        // удаление гуся
        public void Remove(int row, int col)
        {
            if (grid[row, col] == "Гусь")
            {
                GeeseCount--;
                grid[row, col] = null;
            }
        }

        // проверка пустого места
        public bool IsEmpty(int row, int col)
        {
            return IsAvailable(row, col) && grid[row, col] == null;
        }

        // найти лису
        public Point FindFox()
        {
            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (grid[row, col] == "Лиса")
                    {
                        return new Point(row, col);
                    }
                }
            }
            return Point.Empty;
        }
    }
}
