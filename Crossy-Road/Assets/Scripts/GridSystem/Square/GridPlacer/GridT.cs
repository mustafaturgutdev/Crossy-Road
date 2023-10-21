using System.Collections;
using System.Collections.Generic;

namespace GridSystem.Square
{
    public class GridT<T> : IEnumerable<T> where T : IPlacable
    {
        private readonly Dictionary<Cell, T> _CellPlacablePairs = new();

        public GridT(Grid grid)
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
