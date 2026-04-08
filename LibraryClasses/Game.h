#pragma once

#include "Board.h"
#include "GameRules.h"
#include "Move.h"
#include "Fox.h"
#include "Goose.h"

using namespace System;
using namespace System::Collections::Generic;

namespace fox_and_geese
{
    public enum class GameState
    {
        Active,
        FoxWon,
        GeeseWon
    };

    public ref class Game
    {
    private:
        Board^ _board;
        PlayerType _currentTurn;
        GameState _state;
        GameRules^ _rules;
        Stack<Move^>^ _moveHistory;
        bool _isCaptureSequence;
        int _lastCaptureCount;
        bool _isFirstMove;

    public:
        property Board^ Board
        {
            Board ^ get() { return _board; }
        }

            property PlayerType CurrentTurn
        {
            PlayerType get() { return _currentTurn; }
        }

            property GameState State
        {
            GameState get() { return _state; }
        }

            property PlayerType Winner
        {
            PlayerType get()
            {
                if (_state == GameState::FoxWon)
                    return PlayerType::Fox;
                if (_state == GameState::GeeseWon)
                    return PlayerType::Goose;
                return PlayerType::Fox; // значение по умолчанию
            }
        }

            Game()
        {
            _board = gcnew Board(7);
            _currentTurn = PlayerType::Goose;
            _state = GameState::Active;
            _rules = GameRules::Instance;
            _moveHistory = gcnew Stack<Move^>();
            _isCaptureSequence = false;
            _lastCaptureCount = 0;
            _isFirstMove = true;
        }

        bool MakeMove(Move^ move)
        {
            if (_state != GameState::Active)
                return false;

            if (!_rules->IsMoveValid(move, _board, _currentTurn))
                return false;

            move->Execute(_board);
            _moveHistory->Push(move);

            bool wasCapture = move->CapturedPiece != nullptr;

            if (_isFirstMove)
            {
                _isFirstMove = false;
            }

            if (_currentTurn == PlayerType::Fox && wasCapture)
            {
                auto fox = _board->GetFox();
                auto additionalCaptures = fox->GetCaptureMoves(_board);

                if (additionalCaptures->Count > 0)
                {
                    _isCaptureSequence = true;
                    _lastCaptureCount++;
                    return true;
                }
                else
                {
                    _lastCaptureCount++;
                }
            }

            _isCaptureSequence = false;
            _currentTurn = (_currentTurn == PlayerType::Fox) ? PlayerType::Goose : PlayerType::Fox;

            auto winner = _rules->CheckWinCondition(_board);
            if (winner.HasValue)
            {
                _state = (winner.Value == PlayerType::Fox) ? GameState::FoxWon : GameState::GeeseWon;
            }

            _lastCaptureCount = 0;
            return true;
        }

        void UndoMove()
        {
            if (_moveHistory->Count > 0)
            {
                while (_moveHistory->Count > 0)
                {
                    auto lastMove = _moveHistory->Pop();
                    lastMove->Undo(_board);

                    if (_moveHistory->Count > 0 && _moveHistory->Peek()->MovedPiece->Type != lastMove->MovedPiece->Type)
                        break;
                }

                _currentTurn = (_moveHistory->Count > 0) ?
                    _moveHistory->Peek()->MovedPiece->Type : PlayerType::Goose;
                _state = GameState::Active;
                _isCaptureSequence = false;
                _lastCaptureCount = 0;
                _isFirstMove = (_moveHistory->Count == 0);
            }
        }

        bool IsGameOver()
        {
            return _state != GameState::Active;
        }

        bool IsCaptureSequence()
        {
            return _isCaptureSequence;
        }

        int GetCaptureCount()
        {
            return _lastCaptureCount;
        }

        int GetCapturedGeeseCount()
        {
            return _rules->GetInitialGeeseCount() - _board->GetGeeseCount();
        }
    };
}
