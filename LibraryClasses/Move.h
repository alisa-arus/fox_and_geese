#pragma once

#include "Piece.h"
#include "Position.h"
#include "Board.h"

using namespace System;

namespace fox_and_geese
{
    public ref class Move
    {
    private:
        Piece^ _movedPiece;
        Position^ _from;
        Position^ _to;
        Piece^ _capturedPiece;

    public:
        property Piece^ MovedPiece
        {
            Piece ^ get() { return _movedPiece; }
        }

            property Position^ From
        {
            Position ^ get() { return _from; }
        }

            property Position^ To
        {
            Position ^ get() { return _to; }
        }

            property Piece^ CapturedPiece
        {
            Piece ^ get() { return _capturedPiece; }
        }

            Move(Piece^ piece, Position^ from, Position^ to, Piece^ capturedPiece)
        {
            _movedPiece = piece;
            _from = from;
            _to = to;
            _capturedPiece = capturedPiece;
        }

        void Execute(Board^ board)
        {
            board->RemovePiece(_from);
            _movedPiece->Position = _to;
            board->PlacePiece(_movedPiece, _to);

            if (_capturedPiece != nullptr)
            {
                board->RemovePiece(_capturedPiece->Position);
            }
        }

        void Undo(Board^ board)
        {
            board->RemovePiece(_to);
            _movedPiece->Position = _from;
            board->PlacePiece(_movedPiece, _from);

            if (_capturedPiece != nullptr)
            {
                board->PlacePiece(_capturedPiece, _capturedPiece->Position);
            }
        }

        virtual String^ ToString() override
        {
            if (_capturedPiece != nullptr)
            {
                return String::Format("{0} moves from {1} to {2} (captures {3})",
                    _movedPiece->GetType()->Name, _from, _to, _capturedPiece->GetType()->Name);
            }
            else
            {
                return String::Format("{0} moves from {1} to {2}",
                    _movedPiece->GetType()->Name, _from, _to);
            }
        }
    };
}
