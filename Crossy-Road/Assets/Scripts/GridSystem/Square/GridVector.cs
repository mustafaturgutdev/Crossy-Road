using System;
using System.Runtime.CompilerServices;

namespace GridSystem.Square
{
    public struct GridVector
    {
        public int Row { get; }
        public int Column { get; }

        public GridVector(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public static GridVector Down { get; } = new GridVector(-1, 0);
        public static GridVector Zero { get; } = new GridVector(0, 0);
        public static GridVector One { get; } = new GridVector(1, 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(GridVector v1, GridVector v2)
        {
            float dx = v1.Row - v2.Row;
            float dy = v1.Column - v2.Column;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GridVector operator +(GridVector a, GridVector b) => new GridVector(a.Row + b.Row, a.Column + b.Column);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GridVector operator -(GridVector a, GridVector b) => new GridVector(a.Row - b.Row, a.Column - b.Column);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GridVector operator *(GridVector a, int b) => new GridVector(a.Row * b, a.Column * b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GridVector operator /(GridVector a, int b) => new GridVector(a.Row / b, a.Column / b);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(GridVector a, GridVector b) => a.Row == b.Row && a.Column == b.Column;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(GridVector a, GridVector b) => a.Row != b.Row || a.Column != b.Column;



        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => HashCode.Combine(Row, Column);
        public override string ToString() => $"{Row},{Column}";
    }
}
