using System;
using UnityEditor;
using UnityEngine;
using Space = Blink.KEK.SpaceSystem.Space;
using Vector2 = System.Numerics.Vector2;

namespace Blink.KEK.RectangularSystem
{
    public class RectangularGridBehaviour : MonoBehaviour
    {
        [Min(1)]
        [SerializeField] private int rowCount = 10;
        [Min(1)]
        [SerializeField] private int columnCount = 10;

        [SerializeField] private RectTransform gridRect;

        Space space = new Space();
        //RectangularGrid rectangularGrid = new RectangularGrid(10, 10);
        //Space newSpace = new Space();

        private void Start()
        {
            Initialize(rowCount,columnCount);
            //newSpace.GlobalPosition = new Vector2(5, 10);
            //newSpace.GlobalScale = new Vector2(2, 1);
            //newSpace.GlobalRotation = 0.5f;
            space.GlobalScale = CellSize;


            //rectangularGrid = new RectangularGrid(rowCount, columnCount);

        }


        public static System.Numerics.Vector2 CellSize { get; private set; }
        public static System.Numerics.Vector2 GridPosition { get; private set; }


        Vector3 topLeftInWorldSpace;
        Vector3 bottomRightInWorldSpace;

        public void Initialize(int rowCount, int columnCount)
        {
            //GridPosition = gridRect.transform.position.ToVector2();
            //rowCount--;
            Vector3[] worldCorners = new Vector3[4];
            gridRect.GetWorldCorners(worldCorners);

             topLeftInWorldSpace = worldCorners[1];
             bottomRightInWorldSpace = worldCorners[3];

            Vector2 gridSize = new Vector2(Math.Abs(bottomRightInWorldSpace.x - topLeftInWorldSpace.x), Math.Abs(bottomRightInWorldSpace.y - topLeftInWorldSpace.y));

            //CellSize = Math.Min(gridSize.y / rowCount, gridSize.x / columnCount);
            CellSize = new System.Numerics.Vector2( gridSize.X / columnCount, gridSize.Y / rowCount);
        }
        //private void Update()
        //{
        //    if (rectangularGrid.RowCount != rowCount || rectangularGrid.ColumnCount != columnCount)
        //    {
        //        Initialize(rowCount, columnCount);

        //        space.GlobalScale = CellSize;
        //        rectangularGrid = new RectangularGrid(rowCount, columnCount, space);
        //    }

        //    rectangularGrid.Space.GlobalScale = new Vector2(transform.lossyScale.x, transform.lossyScale.y);
        //    rectangularGrid.Space.GlobalPosition = new Vector2(transform.position.x, transform.position.y);
        //    rectangularGrid.Space.GlobalRotation = transform.eulerAngles.z * Mathf.Deg2Rad;

        //    var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    if (rectangularGrid.IsPositionOnGrid(new Vector2(worldPos.x, worldPos.y), out var cell))
        //        Debug.Log(cell.GridPosition);

        //}

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.DrawWireDisc(topLeftInWorldSpace,Vector3.back,1);
            Handles.DrawWireDisc(bottomRightInWorldSpace, Vector3.back, 1);
            //GridDrawer.DrawGrid(rectangularGrid);
        }
#endif
    }
}
