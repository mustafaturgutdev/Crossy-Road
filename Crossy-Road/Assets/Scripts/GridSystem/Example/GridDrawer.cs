using UnityEditor;
using UnityEngine;

namespace GridSystem.Square
{
    public static class GridDrawer
    {
        public static void DrawGrid(Grid grid)
        {
            //foreach (GridCell cell in grid)
            //{
            //    System.Numerics.Vector2 pos = cell.Space.GlobalPosition;
            //    Handles.DrawWireDisc(new Vector3(pos.X, pos.Y), Vector3.back, Mathf.Min(grid.Space.RelativeScale.X / 2), grid.Space.RelativeScale.Y / 2);
            //}
            //System.Numerics.Vector2 scl = grid.Space.RelativeScale / 2;
            //Debug.Log(scl);
            //foreach (GridCell cell in grid)
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
            //for (int i = 0; i < grid.RowCount; i++)
            //{
            //    GridCell cell1 = grid[i, 0];
            //    GridCell cell2 = grid[i, grid.ColumnCount - 1];
            //    System.Numerics.Vector2 pos1 = cell1.Space.GlobalPosition;
            //    System.Numerics.Vector2 pos2 = cell2.Space.GlobalPosition;
            //    Handles.DrawLine(new Vector3(pos1.X, pos1.Y), new Vector3(pos2.X, pos2.Y));
            //}

            //for (int i = 0; i < grid.ColumnCount; i++)
            //{
            //    GridCell cell1 = grid[0, i];
            //    GridCell cell2 = grid[grid.RowCount - 1, i];
            //    System.Numerics.Vector2 pos1 = cell1.Space.GlobalPosition;
            //    System.Numerics.Vector2 pos2 = cell2.Space.GlobalPosition;
            //    Handles.DrawLine(new Vector3(pos1.X, pos1.Y), new Vector3(pos2.X, pos2.Y));
            //}
            //Draw Canvas

            //int row = grid.RowCount;
            //int column = grid.ColumnCount;

            //int halfRow = Mathf.FloorToInt(row / 2f);
            //int halfColumn = Mathf.FloorToInt(column / 2f);

            //for (int i = -halfRow; i < halfRow; i++)
            //{
            //    for (int j = -halfColumn; j < halfColumn; j++)
            //    {
            //        Handles.color = Color.white;
            //        Handles.DrawWireDisc(grid.GetTilePosition(new Tile(j, i)), normal, 0.3f);
            //        Handles.color = Color.red;
            //        Handles.DrawWireDisc(grid.GetVertexPosition(new Vertex(j, i)), normal, 0.1f);
            //        Handles.color = Color.white;

            //        Handles.DrawLine(grid.GetVertexPosition(new Vertex(j, i)), grid.GetVertexPosition(new Vertex(j, i + 1)));
            //        Handles.DrawLine(grid.GetVertexPosition(new Vertex(j, i)), grid.GetVertexPosition(new Vertex(j + 1, i)));

            //        Handles.color = Color.green;
            //        Handles.DrawWireDisc(grid.GetEdgePosition(new Edge(j, i, "N")), normal, 0.1f);
            //        Handles.DrawWireDisc(grid.GetEdgePosition(new Edge(j, i, "W")), normal, 0.1f);

            //        if (i == -halfRow)
            //        {
            //            Handles.DrawWireDisc(grid.GetEdgePosition(new Edge(j, -halfRow - 1, "N")), normal, 0.1f);
            //        }
            //        else if (i == halfRow - 1)
            //        {
            //            Handles.color = Color.white;
            //            Handles.DrawLine(grid.GetVertexPosition(new Vertex(j, halfRow)), grid.GetVertexPosition(new Vertex(j + 1, halfRow)));
            //            Handles.color = Color.red;
            //            Handles.DrawWireDisc(grid.GetVertexPosition(new Vertex(j, halfRow)), normal, 0.1f);
            //        }


            //        if (j == halfColumn - 1)
            //        {
            //            Handles.color = Color.green;

            //            Handles.DrawWireDisc(grid.GetEdgePosition(new Edge(halfColumn, i, "W")), normal, 0.1f);
            //            Handles.color = Color.white;
            //            Handles.DrawLine(grid.GetVertexPosition(new Vertex(halfColumn, i)), grid.GetVertexPosition(new Vertex(halfColumn, i + 1)));
            //            Handles.color = Color.red;
            //            Handles.DrawWireDisc(grid.GetVertexPosition(new Vertex(halfColumn, i)), normal, 0.1f);
            //        }

            //        if (i == halfRow - 1 && j == halfColumn - 1)
            //        {
            //            Handles.DrawWireDisc(grid.GetVertexPosition(new Vertex(halfColumn, halfRow)), normal, 0.1f);
            //        }

            //    }
            //}
        }

    }
}
