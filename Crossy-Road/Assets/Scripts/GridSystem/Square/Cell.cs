using System;
using UnityEngine;

namespace GridSystem.Square
{
    public class Cell : IDisposable
    {
        public GridVector GridPosition { get; }
        public GameObject GameObject { get; } = new GameObject("Cell");

        public Cell(GridVector gridPosition, Vector3 localPosition, Transform parent)
        {
            GridPosition = gridPosition;
            GameObject.transform.SetParent(parent);
            GameObject.transform.localPosition = localPosition;
        }

        public void Dispose()
        {
            GameObject.Destroy(GameObject);
        }
    }
}
