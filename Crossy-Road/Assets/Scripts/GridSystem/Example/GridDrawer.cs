using UnityEditor;
using UnityEngine;

namespace Blink.KEK.RectangularSystem
{
    public static class GridDrawer
    {
        public static void DrawGrid(RectangularGrid rectangularGrid)
        {
            //foreach (RectangularGridCell cell in rectangularGrid)
            //{
            //    System.Numerics.Vector2 pos = cell.Space.GlobalPosition;
            //    Handles.DrawWireDisc(new Vector3(pos.X, pos.Y), Vector3.back, Mathf.Min(rectangularGrid.Space.RelativeScale.X / 2), rectangularGrid.Space.RelativeScale.Y / 2);
            //}
            //System.Numerics.Vector2 scl = rectangularGrid.Space.RelativeScale / 2;
            //Debug.Log(scl);
            //foreach (RectangularGridCell cell in rectangularGrid)
            //{
            //    System.Numerics.Vector2 pos = cell.Space.GlobalPosition;

            //    Handles.color = Color.yellow;
            //    Handles.DrawLine(new Vector3(pos.X,pos.Y), new Vector3(pos.X , pos.Y + scl.Y));
            //    Handles.color = Color.green;
            //    Handles.DrawLine(new Vector3(pos.X, pos.Y), new Vector3(pos.X + scl.X, pos.Y ));
            //    Handles.color = Color.magenta;
            //    Handles.DrawLine(new Vector3(pos.X, pos.Y), new Vector3(pos.X - scl.X, pos.Y ));
            //    Handles.color = Color.blue;
            //    Handles.DrawLine(new Vector3(pos.X, pos.Y), new Vector3(pos.X  , pos.Y - scl.Y));
            //}
            //for (int i = 0; i < rectangularGrid.RowCount; i++)
            //{
            //    RectangularGridCell cell1 = rectangularGrid[i, 0];
            //    RectangularGridCell cell2 = rectangularGrid[i, rectangularGrid.ColumnCount - 1];
            //    System.Numerics.Vector2 pos1 = cell1.Space.GlobalPosition;
            //    System.Numerics.Vector2 pos2 = cell2.Space.GlobalPosition;
            //    Handles.DrawLine(new Vector3(pos1.X, pos1.Y), new Vector3(pos2.X, pos2.Y));
            //}

            //for (int i = 0; i < rectangularGrid.ColumnCount; i++)
            //{
            //    RectangularGridCell cell1 = rectangularGrid[0, i];
            //    RectangularGridCell cell2 = rectangularGrid[rectangularGrid.RowCount - 1, i];
            //    System.Numerics.Vector2 pos1 = cell1.Space.GlobalPosition;
            //    System.Numerics.Vector2 pos2 = cell2.Space.GlobalPosition;
            //    Handles.DrawLine(new Vector3(pos1.X, pos1.Y), new Vector3(pos2.X, pos2.Y));
            //}
            //Draw Canvas

            //int row = rectangularGrid.RowCount;
            //int column = rectangularGrid.ColumnCount;

            //int halfRow = Mathf.FloorToInt(row / 2f);
            //int halfColumn = Mathf.FloorToInt(column / 2f);

            //for (int i = -halfRow; i < halfRow; i++)
            //{
            //    for (int j = -halfColumn; j < halfColumn; j++)
            //    {
            //        Handles.color = Color.white;
            //        Handles.DrawWireDisc(rectangularGrid.GetTilePosition(new Tile(j, i)), normal, 0.3f);
            //        Handles.color = Color.red;
            //        Handles.DrawWireDisc(rectangularGrid.GetVertexPosition(new Vertex(j, i)), normal, 0.1f);
            //        Handles.color = Color.white;

            //        Handles.DrawLine(rectangularGrid.GetVertexPosition(new Vertex(j, i)), rectangularGrid.GetVertexPosition(new Vertex(j, i + 1)));
            //        Handles.DrawLine(rectangularGrid.GetVertexPosition(new Vertex(j, i)), rectangularGrid.GetVertexPosition(new Vertex(j + 1, i)));

            //        Handles.color = Color.green;
            //        Handles.DrawWireDisc(rectangularGrid.GetEdgePosition(new Edge(j, i, "N")), normal, 0.1f);
            //        Handles.DrawWireDisc(rectangularGrid.GetEdgePosition(new Edge(j, i, "W")), normal, 0.1f);

            //        if (i == -halfRow)
            //        {
            //            Handles.DrawWireDisc(rectangularGrid.GetEdgePosition(new Edge(j, -halfRow - 1, "N")), normal, 0.1f);
            //        }
            //        else if (i == halfRow - 1)
            //        {
            //            Handles.color = Color.white;
            //            Handles.DrawLine(rectangularGrid.GetVertexPosition(new Vertex(j, halfRow)), rectangularGrid.GetVertexPosition(new Vertex(j + 1, halfRow)));
            //            Handles.color = Color.red;
            //            Handles.DrawWireDisc(rectangularGrid.GetVertexPosition(new Vertex(j, halfRow)), normal, 0.1f);
            //        }


            //        if (j == halfColumn - 1)
            //        {
            //            Handles.color = Color.green;

            //            Handles.DrawWireDisc(rectangularGrid.GetEdgePosition(new Edge(halfColumn, i, "W")), normal, 0.1f);
            //            Handles.color = Color.white;
            //            Handles.DrawLine(rectangularGrid.GetVertexPosition(new Vertex(halfColumn, i)), rectangularGrid.GetVertexPosition(new Vertex(halfColumn, i + 1)));
            //            Handles.color = Color.red;
            //            Handles.DrawWireDisc(rectangularGrid.GetVertexPosition(new Vertex(halfColumn, i)), normal, 0.1f);
            //        }

            //        if (i == halfRow - 1 && j == halfColumn - 1)
            //        {
            //            Handles.DrawWireDisc(rectangularGrid.GetVertexPosition(new Vertex(halfColumn, halfRow)), normal, 0.1f);
            //        }

            //    }
            //}
        }

    }
}
