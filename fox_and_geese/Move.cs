namespace fox_and_geese
{
    public class Move
    {
        public Piece MovedPiece { get; }
        public Position From { get; }
        public Position To { get; }
        public Piece CapturedPiece { get; }

        public Move(Piece piece, Position from, Position to, Piece capturedPiece)
        {
            MovedPiece = piece;
            From = from;
            To = to;
            CapturedPiece = capturedPiece;
        }

        public void Execute(Board board)
        {
            board.RemovePiece(From);
            MovedPiece.Position = To;
            board.PlacePiece(MovedPiece, To);

            if (CapturedPiece != null)
            {
                board.RemovePiece(CapturedPiece.Position);
            }
        }

        public void Undo(Board board)
        {
            board.RemovePiece(To);
            MovedPiece.Position = From;
            board.PlacePiece(MovedPiece, From);

            if (CapturedPiece != null)
            {
                board.PlacePiece(CapturedPiece, CapturedPiece.Position);
            }
        }
    }
}
