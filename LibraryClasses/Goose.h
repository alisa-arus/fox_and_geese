#pragma once

#include "Piece.h"
#include "Move.h"
#include "Board.h"
#include "Position.h"

using namespace System;
using namespace System::Collections::Generic;

namespace fox_and_geese
{
    public ref class Goose : public Piece
    {
    public:
        Goose(Position^ position) : Piece(PlayerType::Goose, position) {}

        virtual List<Move^>^ GetValidMoves(Board^ board) override
        {
            auto moves = gcnew List<Move^>();

            // ходы гуся (только по горизонтали и вертикали)
            array<Position^>^ orthogonalDirections = gcnew array<Position^>
            {
                gcnew Position(-1, 0),  // вверх
                gcnew Position(1, 0),   // вниз
                gcnew Position(0, -1),  // влево
                gcnew Position(0, 1)    // вправо
            };

            for each(Position ^ dir in orthogonalDirections)
            {
                auto newPos = gcnew Position(Position->X + dir->X, Position->Y + dir->Y);
                if (board->IsPositionValid(newPos) && board->GetPieceAt(newPos) == nullptr)
                {
                    moves->Add(gcnew Move(this, Position, newPos, nullptr));
                }
            }

            return moves;
        }

        virtual Piece^ Clone() override
        {
            return gcnew Goose(gcnew Position(Position->X, Position->Y));
        }
    };
}
