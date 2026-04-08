#pragma once

#include "Move.h"
#include "Board.h"
#include "Piece.h"
#include "Fox.h"
#include "Goose.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Linq;

namespace fox_and_geese
{
    public ref class GameRules
    {
    private:
        static GameRules^ _instance = nullptr;
        static const int INITIAL_GEESE_COUNT = 13;
        static const int FOX_WIN_CONDITION = 8;

        GameRules() {}

    public:
        static property GameRules^ Instance
        {
            GameRules ^ get()
            {
                if (_instance == nullptr)
                    _instance = gcnew GameRules();
                return _instance;
            }
        }

            bool IsMoveValid(Move^ move, Board^ board, PlayerType currentTurn)
        {
            if (move->MovedPiece->Type != currentTurn)
                return false;

            auto validMoves = move->MovedPiece->GetValidMoves(board);

            // Проверяем, есть ли ход с такой же целевой позицией
            for each(Move ^ validMove in validMoves)
            {
                if (validMove->To->Equals(move->To))
                    return true;
            }
            return false;
        }

        Nullable<PlayerType> CheckWinCondition(Board^ board)
        {
            auto fox = board->GetFox();
            auto geese = board->GetGeese();

            // проверяем победу лисы (осталось 8 или меньше гусей)
            if (geese->Count <= FOX_WIN_CONDITION)
            {
                return Nullable<PlayerType>(PlayerType::Fox);
            }

            // проверяем победу гусей (лиса заблокирована)
            if (fox->GetValidMoves(board)->Count == 0)
            {
                return Nullable<PlayerType>(PlayerType::Goose);
            }

            // продолжаем игру
            return Nullable<PlayerType>();
        }

        int GetGeeseToCapture()
        {
            return INITIAL_GEESE_COUNT - FOX_WIN_CONDITION;
        }

        int GetInitialGeeseCount()
        {
            return INITIAL_GEESE_COUNT;
        }

        int GetFoxWinCondition()
        {
            return FOX_WIN_CONDITION;
        }
    };
}
