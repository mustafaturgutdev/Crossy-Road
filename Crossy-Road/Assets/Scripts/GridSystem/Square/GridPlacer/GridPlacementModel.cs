using System.Collections.Generic;

namespace GridSystem.Square
{
    public readonly struct GridPlacementModel<T>
    {
        public int RowCount { get; }
        public int ColumnCount { get; }
        public List<(GridVector, T)> CellValuePairs { get; }

        public GridPlacementModel(int rowCount, int columnCount, List<(GridVector, T)> cellValuePairs)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            CellValuePairs = cellValuePairs;
        }
    }
}
