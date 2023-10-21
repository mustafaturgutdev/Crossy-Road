using System.Collections.Generic;

namespace GridSystem.Square
{
    public readonly struct RectangularGridPlacementModel<T>
    {
        public int RowCount { get; }
        public int ColumnCount { get; }
        public List<(GridVector, T)> CellValuePairs { get; }

        public RectangularGridPlacementModel(int rowCount, int columnCount, List<(GridVector, T)> cellValuePairs)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            CellValuePairs = cellValuePairs;
        }
    }
}
