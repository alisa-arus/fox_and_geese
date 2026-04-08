#pragma once

#include "Piece.h"
#include "Move.h"
#include "Board.h"
#include "Position.h"

using namespace System;
using namespace System::Collections::Generic;

namespace fox_and_geese
{
    public ref class Fox : public Piece
    {
    public:
        Fox(Position^ position) : Piece(PlayerType::Fox, position)
        {
        }

        virtual List<Move^>^ GetValidMoves(Board^ board) override
        {
            auto moves = gcnew List<Move^>();

            // лиса может ходить во всех направлени€х: горизонталь, вертикаль, диагональ
            array<Position^>^ allDirections = gcnew array<Position^>
            {
                // ходы по горизонтали и вертикали
                gcnew Position(-1, 0), gcnew Position(1, 0),
                gcnew Position(0, -1), gcnew Position(0, 1),
                // ходы по диагонали
                gcnew Position(-1, -1), gcnew Position(-1, 1),
                gcnew Position(1, -1), gcnew Position(1, 1)
            };

            // обычные ходы (на соседнюю клетку)
            for each(Position ^ dir in allDirections)
            {
                auto newPos = gcnew Position(Position->X + dir->X, Position->Y + dir->Y);
                if (board->IsPositionValid(newPos) && board->GetPieceAt(newPos) == nullptr)
                {
                    moves->Add(gcnew Move(this, Position, newPos, nullptr));
                }
            }

            // ходы рубки (прыжок через гус€)
            auto captureMoves = GetCaptureMoves(board);
            for each(Move ^ move in captureMoves)
            {
                moves->Add(move);
            }

            return moves;
        }

        List<Move^>^ GetCaptureMoves(Board^ board)
        {
            auto captures = gcnew List<Move^>();

            // лиса может рубить во всех направлени€х
            array<Position^>^ allDirections = gcnew array<Position^>
            {
                // рубка по горизонтали и по вертикали
                gcnew Position(-1, 0), gcnew Position(1, 0),
                gcnew Position(0, -1), gcnew Position(0, 1)
                // диагональна€ рубка (раскомментировать если нужна по правилам)
                // gcnew Position(-1, -1), gcnew Position(-1, 1),
                // gcnew Position(1, -1), gcnew Position(1, 1)
            };

            for each(Position ^ dir in allDirections)
            {
                auto adjacentPos = gcnew Position(Position->X + dir->X, Position->Y + dir->Y);
                auto targetPos = gcnew Position(Position->X + dir->X * 2, Position->Y + dir->Y * 2);

                auto adjacentPiece = board->GetPieceAt(adjacentPos);
                if (adjacentPiece != nullptr && adjacentPiece->Type == PlayerType::Goose &&
                    board->IsPositionValid(targetPos) && board->GetPieceAt(targetPos) == nullptr)
                {
                    captures->Add(gcnew Move(this, Position, targetPos, adjacentPiece));
                }
            }
            return captures;
        }

        virtual Piece^ Clone() override
        {
            return gcnew Fox(gcnew Position(Position->X, Position->Y));
        }
    };
}
