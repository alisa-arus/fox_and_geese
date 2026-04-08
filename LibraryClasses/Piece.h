#pragma once

using namespace System;
using namespace System::Collections::Generic;

namespace fox_and_geese
{
    // Перечисление типов игроков
    public enum class PlayerType
    {
        Fox,
        Goose
    };

    public ref class Piece abstract
    {
    private:
        PlayerType _type;
        Position^ _position;

    public:
        property PlayerType Type
        {
            PlayerType get() { return _type; }
        }

            property Position^ Position
        {
            Position ^ get() { return _position; }
            void set(Position ^ value) { _position = value; }
        }

            // Конструктор
            Piece(PlayerType type, Position^ position)
        {
            _type = type;
            _position = position;
        }

        // Абстрактные методы
        virtual List<Move^>^ GetValidMoves(Board^ board) = 0;
        virtual Piece^ Clone() = 0;
    };
}