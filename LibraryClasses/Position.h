#pragma once

using namespace System;

namespace fox_and_geese
{
    public ref class Position : public IEquatable<Position^>
    {
    private:
        int _x;
        int _y;

    public:
        property int X
        {
            int get() { return _x; }
        }

            property int Y
        {
            int get() { return _y; }
        }

            Position(int x, int y)
        {
            _x = x;
            _y = y;
        }

        virtual bool Equals(Object^ obj) override
        {
            Position^ other = dynamic_cast<Position^>(obj);
            if (other != nullptr)
            {
                return _x == other->_x && _y == other->_y;
            }
            return false;
        }

        virtual int GetHashCode() override
        {
            return (_x << 16) ^ _y;
        }

        // Реализация интерфейса IEquatable<Position^>
        virtual bool Equals(Position^ other)
        {
            if (other == nullptr)
                return false;
            return _x == other->_x && _y == other->_y;
        }

        // Перегрузка операторов для удобства
        static bool operator==(Position^ left, Position^ right)
        {
            if (Object::ReferenceEquals(left, right))
                return true;
            if (left == nullptr || right == nullptr)
                return false;
            return left->Equals(right);
        }

        static bool operator!=(Position^ left, Position^ right)
        {
            return !(left == right);
        }

        virtual String^ ToString() override
        {
            return String::Format("({0},{1})", _x, _y);
        }
    };
}
