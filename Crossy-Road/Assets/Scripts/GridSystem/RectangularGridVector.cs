using System;
using System.Runtime.CompilerServices;

namespace Blink.KEK.RectangularSystem
{
    public struct RectangularGridVector
    {
        public int Row { get; }
        public int Column { get; }

        public RectangularGridVector(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public static RectangularGridVector Down { get; } = new RectangularGridVector(-1, 0);
        public static RectangularGridVector Zero { get; } = new RectangularGridVector(0, 0);
        public static RectangularGridVector One { get; } = new RectangularGridVector(1, 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(RectangularGridVector v1, RectangularGridVector v2)
        {
            float dx = v1.Row - v2.Row;
            float dy = v1.Column - v2.Column;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangularGridVector operator +(RectangularGridVector a, RectangularGridVector b) => new RectangularGridVector(a.Row + b.Row, a.Column + b.Column);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangularGridVector operator -(RectangularGridVector a, RectangularGridVector b) => new RectangularGridVector(a.Row - b.Row, a.Column - b.Column);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangularGridVector operator *(RectangularGridVector a, int b) => new RectangularGridVector(a.Row * b, a.Column * b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangularGridVector operator /(RectangularGridVector a, int b) => new RectangularGridVector(a.Row / b, a.Column / b);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(RectangularGridVector a, RectangularGridVector b) => a.Row == b.Row && a.Column == b.Column;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(RectangularGridVector a, RectangularGridVector b) => a.Row != b.Row || a.Column != b.Column;



        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => HashCode.Combine(Row, Column);
        public override string ToString() => $"{Row},{Column}";
    }
}
