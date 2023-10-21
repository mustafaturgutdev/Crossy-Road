using Blink.KEK.SpaceSystem;
using UnityEngine;

namespace Blink.KEK.RectangularSystem
{
    public class RectangularGridCell
    {
        public RectangularGridVector GridPosition { get; }
        public GameObject GameObject { get; } = new GameObject("Cell");

        public RectangularGridCell(RectangularGridVector gridPosition,Vector2 localPosition, Transform parent)
        {
            GridPosition = gridPosition;
            GameObject.transform.SetParent(parent);
            GameObject.transform.localPosition = localPosition;
            //Space = parent;
        }
    }
}
