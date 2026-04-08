#pragma once

#include "Position.h"
#include "Piece.h"
#include "Fox.h"
#include "Goose.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Linq;

namespace fox_and_geese
{
    public class Board
    {
    private:
        Dictionary<Position^, Piece^>^ pieces;
        HashSet<Position^>^ validPositions;

    public:
        property int Size;

        Board(int size = 7)
        {
            Size = size;
            pieces = gcnew Dictionary<Position^, Piece^>();
            validPositions = gcnew HashSet<Position^>();
            InitializeValidPositions();
            InitializeBoard();
        }
    public:
        void PlacePiece(Piece^ piece, Position^ pos);
        void RemovePiece(Position^ pos);
        Piece^ GetPieceAt(Position^ pos);
        bool IsPositionValid(Position^ pos);
    private:
        void InitializeValidPositions()
        {
            // три строки центральной горизонтали
            for (int row = 2; row <= 4; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    validPositions->Add(gcnew Position(row, col));
                }
            }

            // три столбца центральной вертикали
            for (int col = 2; col <= 4; col++)
            {
                for (int row = 0; row < Size; row++)
                {
                    validPositions->Add(gcnew Position(row, col));
                }
            }

            // удаляем дубликаты и углы
            auto center = gcnew Position(3, 3);
            auto toRemove = gcnew List<Position^>();

            for each(Position ^ pos in validPositions)
            {
                if (Math::Abs(pos->X - center->X) > 2 && Math::Abs(pos->Y - center->Y) > 2)
                {
                    toRemove->Add(pos);
                }
            }

            for each(Position ^ pos in toRemove)
            {
                validPositions->Remove(pos);
            }
        }

        void InitializeBoard()
        {
            // в начале игры лиса помещается в центр
            auto foxPos = gcnew Position(3, 3);
            auto fox = gcnew Fox(foxPos);
            PlacePiece(fox, foxPos);

            // в начале игры все 13 гусей размещаются на трёх верхних рядах
            int geesePlaced = 0;
            int targetGeese = 13;

            // заполняем верхние три ряда игрового поля
            for (int row = 0; row < 3 && geesePlaced < targetGeese; row++)
            {
                for (int col = 0; col < Size && geesePlaced < targetGeese; col++)
                {
                    auto pos = gcnew Position(row, col);
                    // проверяем, что позиция доступна и не занята лисой
                    if (IsPositionValid(pos) && !pos->Equals(foxPos))
                    {
                        // проверяем, что на этой позиции еще нет фигуры
                        if (GetPieceAt(pos) == nullptr)
                        {
                            auto goose = gcnew Goose(pos);
                            PlacePiece(goose, pos);
                            geesePlaced++;
                        }
                    }
                }
            }
        }

    public:
        void PlacePiece(Piece^ piece, Position^ pos)
        {
            if (IsPositionValid(pos))
            {
                pieces[pos] = piece;
                piece->Position = pos;
            }
        }

        void RemovePiece(Position^ pos)
        {
            pieces->Remove(pos);
        }

        Piece^ GetPieceAt(Position^ pos)
        {
            Piece^ piece = nullptr;
            pieces->TryGetValue(pos, piece);
            return piece;
        }

        bool IsPositionValid(Position^ pos)
        {
            return pos->X >= 0 && pos->X < Size && pos->Y >= 0 && pos->Y < Size && validPositions->Contains(pos);
        }

        Fox^ GetFox()
        {
            for each(Piece ^ piece in pieces->Values)
            {
                Fox^ fox = dynamic_cast<Fox^>(piece);
                if (fox != nullptr)
                    return fox;
            }
            return nullptr;
        }

        List<Goose^>^ GetGeese()
        {
            auto geese = gcnew List<Goose^>();
            for each(Piece ^ piece in pieces->Values)
            {
                Goose^ goose = dynamic_cast<Goose^>(piece);
                if (goose != nullptr)
                    geese->Add(goose);
            }
            return geese;
        }

        int GetGeeseCount()
        {
            return GetGeese()->Count;
        }
    };
}