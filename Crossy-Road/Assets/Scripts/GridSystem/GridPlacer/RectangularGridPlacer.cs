using System.Collections;
using System.Collections.Generic;

namespace Blink.KEK.RectangularSystem
{
    public class RectangularGridPlacer<T> : IEnumerable<T> where T : IPlacable
    {
        private readonly Dictionary<RectangularGridCell, T> _CellPlacablePairs = new();

        public RectangularGridPlacer(RectangularGrid rectangularGrid)
        {
            foreach (RectangularGridCell cell in rectangularGrid)
                _CellPlacablePairs.Add(cell, default);
        }

        public bool TryGetValue(RectangularGridCell cell, out T value)
        {
            return _CellPlacablePairs.TryGetValue(cell, out value) && value != null;
        }

        public bool Place(T value, RectangularGridCell cell)
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
