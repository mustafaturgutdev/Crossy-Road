using UnityEngine;

namespace GridSystem.Square
{
    public class Cell
    {
        public GridVector GridPosition { get; }
        public GameObject GameObject { get; } = new GameObject("Cell");

        public Cell(GridVector gridPosition,Vector2 localPosition, Transform parent)
        {
            GridPosition = gridPosition;
            GameObject.transform.SetParent(parent);
            GameObject.transform.localPosition = localPosition;
            //Space = parent;
        }
    }
}
