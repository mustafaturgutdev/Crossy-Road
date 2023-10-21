using System.Collections.Generic;

namespace Blink.KEK.RectangularSystem
{
    public readonly struct RectangularGridPlacementModel<T>
    {
        public int RowCount { get; }
        public int ColumnCount { get; }
        public List<(RectangularGridVector, T)> CellValuePairs { get; }

        public RectangularGridPlacementModel(int rowCount, int columnCount, List<(RectangularGridVector, T)> cellValuePairs)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            CellValuePairs = cellValuePairs;
        }
    }
}
