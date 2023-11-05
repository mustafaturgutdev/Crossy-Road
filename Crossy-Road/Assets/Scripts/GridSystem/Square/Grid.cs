using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem.Square
{
    public class InfinityGrid<T> : IEnumerable<Cell> where T : IPlacable
    {
        private readonly Dictionary<GridVector, Cell> gridPositionCellPairs = new();
        private readonly Dictionary<Cell, T> cellPlacablePairs = new();

        //düzenlenecek
        public Cell this[GridVector gridPosition] => gridPositionCellPairs[gridPosition];


        public GameObject GameObject { get; } = new GameObject("Grid");
        private readonly Vector3 cellSize;

        public InfinityGrid(Vector3 cellSize)
        {
            this.cellSize = cellSize;
        }

        public bool TryGetValue(Cell cell, out T value)
        {
            return cellPlacablePairs.TryGetValue(cell, out value) && value != null;
        }
        public bool TryGetValue(GridVector gridPosition, out T value)
        {
            value = default(T);
            return gridPositionCellPairs.TryGetValue(gridPosition, out Cell cell) && TryGetValue(cell, out value);
        }

        private Cell CreateCell(GridVector gridPosition)
        {
            float x = cellSize.x * gridPosition.Column;
            float z = cellSize.y * gridPosition.Row;

            return new Cell(gridPosition, new Vector3(x, 0, z), GameObject.transform);
        }

        public Vector3 GetWorldPosition(GridVector gridPosition)
        {
            return new Vector3(cellSize.x * gridPosition.Column, 0, cellSize.y * gridPosition.Row);
        }
        public GridVector GetGridPosition(Vector3 worldPosition)
        {
            int row = Mathf.FloorToInt(worldPosition.z / cellSize.y);
            int column = Mathf.FloorToInt(worldPosition.x / cellSize.x);

            return new GridVector(row, column);
        }

        public void Place(T value, GridVector gridPosition)
        {
            if (gridPositionCellPairs.TryGetValue(gridPosition, out Cell cell))
            {
                cellPlacablePairs[cell] = value;
                value.Cell = cell;
            }
            else
            {
                cell = CreateCell(gridPosition);
                gridPositionCellPairs.Add(gridPosition, cell);
                cellPlacablePairs.Add(cell, value);
                value.Cell = cell;
            }
        }

        public bool DisPlace(T value)
        {
            if (value != null && value.Cell != null && cellPlacablePairs.TryGetValue(value.Cell, out _))
            {
                Cell cell = value.Cell;
                cellPlacablePairs.Remove(cell);
                gridPositionCellPairs.Remove(cell.GridPosition);
                value.Cell = default;
                cell.Dispose();
                return true;
            }

            return false;
        }

        public bool DisPlace(GridVector gridVector, out T value)
        {
            if (gridPositionCellPairs.TryGetValue(gridVector, out Cell cell) && cellPlacablePairs.TryGetValue(cell, out value))
            {
                cellPlacablePairs.Remove(cell);
                gridPositionCellPairs.Remove(cell.GridPosition);
                value.Cell = default;
                cell.Dispose();
                return true;
            }
            value = default;
            return false;
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            foreach (var pair in gridPositionCellPairs)
                yield return pair.Value;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }



    public class Grid : IEnumerable<Cell>
    {
        private readonly Cell[,] rectangularGrid;
        private readonly Vector3 scale;

        public int RowCount => rectangularGrid.GetLength(0);
        public int ColumnCount => rectangularGrid.GetLength(1);
        public int CellCount => RowCount * ColumnCount;
        public GameObject GameObject { get; } = new GameObject("Grid");
        public Cell this[int rowIndex, int columIndex] => rectangularGrid[rowIndex, columIndex];
        public Cell this[GridVector gridPosition] => rectangularGrid[gridPosition.Row, gridPosition.Column];


        public Grid(int rowCount, int columnCount, Vector3 scale, Vector3 position)
        {

            rectangularGrid = new Cell[rowCount, columnCount];

            GameObject.transform.position = position;
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                for (int columIndex = 0; columIndex < columnCount; columIndex++)
                    rectangularGrid[rowIndex, columIndex] = CreateCell(rowIndex, columIndex, scale);
            this.scale = scale;
        }

        private Cell CreateCell(int rowIndex, int columnIndex, Vector3 cellSize)
        {
            float cellWidth = cellSize.x;
            float cellHeight = cellSize.y;
            float gridWidth = cellWidth * ColumnCount;
            float gridHeight = cellHeight * RowCount;

            float x = -(gridWidth / 2.0f) + (columnIndex * cellWidth) + (cellWidth / 2.0f);
            float y = -(gridHeight / 2.0f) + (rowIndex * cellHeight) + (cellHeight / 2.0f);

            return new Cell(new GridVector(rowIndex, columnIndex), new Vector2(x, y), GameObject.transform);
        }

        public bool IsPositionOnGrid(Vector2 position, out Cell cell)
        {
            int columnIndex = (int)Math.Floor((position.x - GameObject.transform.position.x + scale.x * ColumnCount / 2.0f) / scale.x);
            int rowIndex = (int)Math.Floor((position.y - GameObject.transform.position.y + scale.y * RowCount / 2.0f) / scale.y);

            return IsPositionOnGrid(new GridVector(rowIndex, columnIndex), out cell);
        }


        public bool IsPositionOnGrid(GridVector gridPosition, out Cell cell)
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

        public IEnumerable<Cell> GetNeighborCells(Cell cell)
        {
            int row = cell.GridPosition.Row;
            int column = cell.GridPosition.Column;
            if (IsPositionOnGrid(new GridVector(row, column + 1), out var neighborCell))
                yield return neighborCell;
            if (IsPositionOnGrid(new GridVector(row + 1, column), out neighborCell))
                yield return neighborCell;
            if (IsPositionOnGrid(new GridVector(row, column - 1), out neighborCell))
                yield return neighborCell;
            if (IsPositionOnGrid(new GridVector(row - 1, column), out neighborCell))
                yield return neighborCell;
        }

        public bool AreCellsNeighbor(params Cell[] cells)
        {
            int length = cells.Length;
            if (length < 2) return false;

            for (int i = 0; i < length; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    GridVector positionDiff = cells[i].GridPosition - cells[j].GridPosition;
                    int row = Math.Abs(positionDiff.Row);
                    int column = Math.Abs(positionDiff.Column);
                    if ((row == 1 && column == 0) || (row == 0 && column == 1)) continue;
                    return false;
                }
            }
            return true;
        }
        public IEnumerator<Cell> GetEnumerator()
        {
            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
                for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
                    yield return rectangularGrid[rowIndex, columnIndex];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class Grid<T> : IEnumerable<T> where T : IPlacable
    {
        private readonly Dictionary<Cell, T> _CellPlacablePairs = new();

        public Grid(Grid grid)
        {
            foreach (Cell cell in grid)
                _CellPlacablePairs.Add(cell, default);
        }

        public bool TryGetValue(Cell cell, out T value)
        {
            return _CellPlacablePairs.TryGetValue(cell, out value) && value != null;
        }

        public bool Place(T value, Cell cell)
        {

            if (_CellPlacablePairs.TryGetValue(cell, out _))
            {
                value.Cell = cell;
                _CellPlacablePairs[cell] = value;
                return true;
            }

            return false;
        }

        public bool DisPlace(T value)
        {
            if (value != null && value.Cell != null && _CellPlacablePairs.TryGetValue(value.Cell, out _))
            {
                _CellPlacablePairs[value.Cell] = default;
                value.Cell = default;
                return true;
            }

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var pair in _CellPlacablePairs)
                if (pair.Value != null)
                    yield return pair.Value;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
