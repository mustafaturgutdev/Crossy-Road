using Blink.KEK.SpaceSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Blink.KEK.RectangularSystem
{
    public class RectangularGrid : IEnumerable<RectangularGridCell>
    {
        private readonly RectangularGridCell[,] rectangularGrid;
        private readonly Vector3 scale;

        public int RowCount => rectangularGrid.GetLength(0);
        public int ColumnCount => rectangularGrid.GetLength(1);
        public int CellCount => RowCount * ColumnCount;
        public GameObject GameObject { get; } = new GameObject("Grid");
        public RectangularGridCell this[int rowIndex, int columIndex] => rectangularGrid[rowIndex, columIndex];
        public RectangularGridCell this[RectangularGridVector gridPosition] => rectangularGrid[gridPosition.Row, gridPosition.Column];


        public RectangularGrid(int rowCount, int columnCount, Vector3 scale,Vector3 position)
        {

            rectangularGrid = new RectangularGridCell[rowCount, columnCount];

            GameObject.transform.position = position;
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                for (int columIndex = 0; columIndex < columnCount; columIndex++)
                    rectangularGrid[rowIndex, columIndex] = CreateCell(rowIndex, columIndex, scale);
            this.scale = scale;
        }

        private RectangularGridCell CreateCell(int rowIndex, int columnIndex,Vector3 cellSize)
        {
            float cellWidth = cellSize.x;
            float cellHeight = cellSize.y;
            float gridWidth = cellWidth * ColumnCount;
            float gridHeight = cellHeight * RowCount;

            float x = -(gridWidth / 2.0f) + (columnIndex * cellWidth) + (cellWidth / 2.0f);
            float y = -(gridHeight / 2.0f) + (rowIndex * cellHeight) + (cellHeight / 2.0f);

            return new RectangularGridCell(new RectangularGridVector(rowIndex, columnIndex), new Vector2(x, y), GameObject.transform);
        }

        public bool IsPositionOnGrid(Vector2 position, out RectangularGridCell cell)
        {
            int columnIndex = (int)Math.Floor((position.x - GameObject.transform.position.x + scale.x * ColumnCount / 2.0f) / scale.x);
            int rowIndex = (int)Math.Floor((position.y - GameObject.transform.position.y + scale.y * RowCount / 2.0f) / scale.y);

            return IsPositionOnGrid(new RectangularGridVector(rowIndex, columnIndex), out cell);
        }


        public bool IsPositionOnGrid(RectangularGridVector gridPosition, out RectangularGridCell cell)
        {
            if (gridPosition.Row >= 0 &&
                gridPosition.Row < RowCount &&
                gridPosition.Column >= 0 &&
                gridPosition.Column < ColumnCount)
            {
                cell = rectangularGrid[gridPosition.Row, gridPosition.Column];
                return true;
            }
            cell = default;
            return false;
        }

        public IEnumerable<RectangularGridCell> GetNeighborCells(RectangularGridCell cell)
        {
            int row = cell.GridPosition.Row;
            int column = cell.GridPosition.Column;
            if (IsPositionOnGrid(new RectangularGridVector(row, column + 1), out var neighborCell))
                yield return neighborCell;
            if (IsPositionOnGrid(new RectangularGridVector(row + 1, column), out neighborCell))
                yield return neighborCell;
            if (IsPositionOnGrid(new RectangularGridVector(row, column - 1), out neighborCell))
                yield return neighborCell;
            if (IsPositionOnGrid(new RectangularGridVector(row - 1, column), out neighborCell))
                yield return neighborCell;
        }

        public bool AreCellsNeighbor(params RectangularGridCell[] cells)
        {
            int length = cells.Length;
            if (length < 2) return false;

            for (int i = 0; i < length; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    RectangularGridVector positionDiff = cells[i].GridPosition - cells[j].GridPosition;
                    int row = Math.Abs(positionDiff.Row);
                    int column = Math.Abs(positionDiff.Column);
                    if ((row == 1 && column == 0) || (row == 0 && column == 1)) continue;
                    return false;
                }
            }
            return true;
        }
        public IEnumerator<RectangularGridCell> GetEnumerator()
        {
            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
                for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
                    yield return rectangularGrid[rowIndex, columnIndex];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
