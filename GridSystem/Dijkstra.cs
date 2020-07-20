using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FS.GridSystem
{
    public class Dijkstra
    {
        public class RowPath
        {
            public List<Grid.Row> listOfRows = new List<Grid.Row>();

            public int costOfPath = 0;

            public Grid.Row lastRow;

            public RowPath() { }

            public RowPath(RowPath tp)
            {
                listOfRows = tp.listOfRows.ToList();
                costOfPath = tp.costOfPath;
                lastRow = tp.lastRow;
            }

            public void AddRow(Grid.Row t)
            {
                costOfPath += t.movementCost;
                listOfRows.Add(t);
                lastRow = t;
            }

            public void AddStaticRow(Grid.Row t)
            {
                costOfPath += 1;
                listOfRows.Add(t);
                lastRow = t;
            }
        }

        public static List<Grid.Row> GetBestPath(Grid grid, Grid.Row start, Grid.Row end, float maxCost = 9999)
        {
            return RowGetBestPath(grid, start, end, new GridPosition[0], maxCost);
        }


        private static List<Grid.Row> RowGetBestPath(Grid grid, Grid.Row start, Grid.Row end, GridPosition[] occupied, float maxCost = 9999)
        {
            List<Grid.Row> closed = new List<Grid.Row>();
            List<RowPath> open = new List<RowPath>();

            RowPath originPath = new RowPath();
            originPath.AddRow(start);

            open.Add(originPath);

            while (open.Count > 0)
            {
                open = open.OrderBy(x => x.costOfPath).ToList();
                RowPath current = open[0];
                open.Remove(open[0]);

                if (closed.Contains(current.lastRow))
                {
                    continue;
                }
                if (current.lastRow == end)
                {
                    current.listOfRows.Distinct();
                    current.listOfRows.Remove(start);

                    if (current.listOfRows.Count > maxCost)
                        return null;
                    return current.listOfRows;
                }

                closed.Add(current.lastRow);

                foreach (Grid.Row t in current.lastRow.GetNeighbors(grid))
                {
                    if (t.impassible || occupied.Contains(t.position) || t.occuped || System.Math.Abs(t.height - current.lastRow.height) > 1) continue;
                    RowPath newRowPath = new RowPath(current);
                    newRowPath.AddRow(t);
                    open.Add(newRowPath);
                }
            }
            return null;
        }
    }
}
