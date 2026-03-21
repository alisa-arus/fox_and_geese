using System;
using System.Collections.Generic;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;


namespace fox_and_geese
{
    public class Board
    {
        private Dictionary<Position, Piece> pieces;
        public int Size { get; }
        private HashSet<Position> validPositions;

        public Board(int size = 7)
        {
            Size = size;
            pieces = new Dictionary<Position, Piece>();
            validPositions = new HashSet<Position>();
            InitializeValidPositions();
            InitializeBoard();
        }

        private void InitializeValidPositions()
        {
            // Создаем поле 7x7, где доступны только клетки, образующие крест 3x3 в середине
            // Крест имеет ширину 3 клетки: центральная горизонталь и центральная вертикаль

            // Центральная горизонталь (строки 2,3,4)
            for (int row = 2; row <= 4; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    validPositions.Add(new Position(row, col));
                }
            }

            // Центральная вертикаль (столбцы 2,3,4)
            for (int col = 2; col <= 4; col++)
            {
                for (int row = 0; row < Size; row++)
                {
                    validPositions.Add(new Position(row, col));
                }
            }

            // Удаляем углы креста (клетки, которые не входят в крест 3x3)
            // Оставляем только те клетки, которые находятся на расстоянии не более 2 от центра
            var center = new Position(3, 3);
            var toRemove = validPositions
                .Where(pos => Math.Abs(pos.X - center.X) > 2 && Math.Abs(pos.Y - center.Y) > 2)
                .ToList();

            foreach (var pos in toRemove)
            {
                validPositions.Remove(pos);
            }
        }

        private void InitializeBoard()
        {
            // Размещаем лису в центре
            var foxPos = new Position(3, 3);
            var fox = new Fox(foxPos);
            PlacePiece(fox, foxPos);

            // Размещаем 13 гусей на всех доступных клетках, кроме центра
            List<Position> availablePositions = validPositions
                .Where(p => !p.Equals(foxPos))
                .OrderBy(p => Math.Abs(p.X - 3) + Math.Abs(p.Y - 3)) // Сначала ближние к центру
                .ThenBy(p => p.X)
                .ThenBy(p => p.Y)
                .ToList();

            int geeseToPlace = Math.Min(13, availablePositions.Count);
            for (int i = 0; i < geeseToPlace; i++)
            {
                var goose = new Goose(availablePositions[i]);
                PlacePiece(goose, availablePositions[i]);
            }
        }

        public void PlacePiece(Piece piece, Position pos)
        {
            if (IsPositionValid(pos))
            {
                pieces[pos] = piece;
                piece.Position = pos;
            }
        }

        public void RemovePiece(Position pos)
        {
            pieces.Remove(pos);
        }

        public Piece GetPieceAt(Position pos)
        {
            pieces.TryGetValue(pos, out var piece);
            return piece;
        }

        public bool IsPositionValid(Position pos)
        {
            return pos.X >= 0 && pos.X < Size &&
                   pos.Y >= 0 && pos.Y < Size &&
                   validPositions.Contains(pos);
        }

        public Fox GetFox()
        {
            return pieces.Values.OfType<Fox>().FirstOrDefault();
        }

        public List<Goose> GetGeese()
        {
            return pieces.Values.OfType<Goose>().ToList();
        }

        public Board Clone()
        {
            var newBoard = new Board(Size);
            newBoard.pieces.Clear();

            foreach (var piece in pieces.Values)
            {
                var clonedPiece = piece.Clone();
                newBoard.PlacePiece(clonedPiece, clonedPiece.Position);
            }

            return newBoard;
        }

        public List<Position> GetValidPositions()
        {
            return validPositions.ToList();
        }

        public int GetGeeseCount()
        {
            return GetGeese().Count;
        }

        public bool IsCenterPosition(Position pos)
        {
            return pos.X >= 2 && pos.X <= 4 && pos.Y >= 2 && pos.Y <= 4;
        }
    }
}