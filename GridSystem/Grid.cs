using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FS.GridSystem
{
    public class Grid
    {
        public enum RangeType
        {
            Movement,
            AttackRange,
            AOE
        }

        public class Row
        {
            public GridPosition position;
            public bool impassible;
            public int movementCost;
            public bool occuped;
            public int height;

            public Row(GridPosition position, int movementCost, bool impassible, int height)
            {
                this.position = position;
                this.movementCost = movementCost;
                this.impassible = impassible;
                this.height = height;
                occuped = false;
            }


            public List<Grid.Row> GetNeighbors(Grid grid)
            {
                List<Grid.Row> neighbors = new List<Grid.Row>();

                if (position.x != 0) // not extrem left in grid
                    neighbors.Add(grid.Rows[(int)(position.x - 1), (int)position.y]); // add left neighbors

                if (position.x != grid.Size.x - 1) // not extrem right in grid
                    neighbors.Add(grid.Rows[(int)(position.x + 1), (int)position.y]); // add right neighbors

                if (position.y != 0) // not extrem bottom in grid
                    neighbors.Add(grid.Rows[(int)position.x, (int)(position.y - 1)]); // add bottom neighbors

                if (position.y != grid.Size.y - 1) // not extrem top in grid
                    neighbors.Add(grid.Rows[(int)position.x, (int)(position.y + 1)]); // add top neighbors

                return neighbors;
            }
        }

        public class Range
        {
            public enum RangeType
            {
                Default,
                Square,
                Cross,
                HorizontallyLine,
                VerticallyLine
            }

            public RangeType rangeType;
            public int size;
            public int startDistanceSize;

            public Range(int size, RangeType rangeType, int startDistanceSize = 1)
            {
                this.size = size;
                this.rangeType = rangeType;
                this.startDistanceSize = startDistanceSize;
            }
        }

        /*public class RandomGrid
        {
            public int maxCost;
            public List<int> ratioCost;
            public int ratioImpassible;

            public RandomGrid(List<int> ratioCost, int maxCost = 1, int ratioImpassible = 0)
            {
                this.ratioCost = ratioCost;
                this.maxCost = maxCost;
                this.ratioImpassible = ratioImpassible;
            }
        }*/

        public Row[,] Rows { get { return m_rows; } }
        public GridPosition Size { get { return m_size; } }
        public List<Row> LastAttackRange { get { return m_lastAttackRange; } }

        private Row[,] m_rows;
        private GridPosition m_size;
        private List<Row> m_lastAttackRange;

        /*public string GridToString()
        {
            string gridString = null;

            for (int x = 0; x < m_size.x; x++)
            {
                for (int y = 0; y < m_size.y; y++)
                {
                    gridString += m_rows[x, y].impassible ? 0 : 1;
                    if (x * y != (m_size.x - 1) * (m_size.y - 1))
                        gridString += ",";
                }
            }

            return gridString;
        }

        public static Grid StringToGrid(int width, int length, string grid)
        {
            Grid newGrid = new Grid();
            string[] impasibles = grid.Split(',');

            newGrid.m_size = new GridPosition(width, length);
            newGrid.m_rows = new Row[newGrid.m_size.x, newGrid.m_size.y];

            for (int x = 0; x < width; x ++)
            {
                for (int y = 0; y < length; y++)
                {
                    newGrid.m_rows[x, y] = new Row(new GridPosition(x, y), 1, impasibles[(x * width) + y] == "0");
                }
            }

            return newGrid;
        }*/

        public void CreateGrid(GridPosition size, int[,] gridString /*, RandomGrid randomGrid = null*/)
        {
            m_size = size;
            m_rows = new Row[m_size.x, m_size.y];

            for (int x = 0; x < m_size.x; x++)
            {
                for (int y = 0; y < m_size.y; y++)
                {
                   /* if (randomGrid != null)
                    {
                        int cost = 1;
                        for (int i = 0; i < randomGrid.ratioCost.Count; i++)
                        {
                            Random randomRatio = new Random(Guid.NewGuid().GetHashCode());
                            if (randomRatio.Next(0, 100) > randomGrid.ratioCost[i] && i < randomGrid.ratioCost.Count)
                                continue;
                            else
                                cost = i + 1;
                        }

                        Random randomImpasible = new Random(Guid.NewGuid().GetHashCode());
                        bool impassible = randomImpasible.Next(0, 100) < randomGrid.ratioImpassible;
                        m_rows[x, y] = new Row(new GridPosition(x, y), cost, impassible);
                    }
                    else*/
                        m_rows[x, y] = new Row(new GridPosition(x, y), 1, gridString[x, y] == -1, gridString[x, y]);
                }
            }
        }

        public Row GetRandomRow(List<Row> rowsFounded, bool isStatic = true)
        {
            Random randomXHash = new Random(Guid.NewGuid().GetHashCode());
            Random randomYHash = new Random(Guid.NewGuid().GetHashCode());
            int randomX = randomXHash.Next(0, Size.x);
            int randomY = randomYHash.Next(0, Size.y);

            if (rowsFounded != null)
            {
                if (rowsFounded.Find(x => x.position == new GridPosition(randomX, randomY)) == null)
                {
                    if (isStatic && m_rows[randomX, randomY].impassible)
                        return GetRandomRow(rowsFounded);

                    rowsFounded.Add(m_rows[randomX, randomY]);
                    return m_rows[randomX, randomY];
                }
                else
                {
                    return GetRandomRow(rowsFounded);
                }
            }
            else
                rowsFounded = new List<Row>();

            if (isStatic && m_rows[randomX, randomY].impassible)
                return GetRandomRow(rowsFounded);

            rowsFounded.Add(m_rows[randomX, randomY]);

            return m_rows[randomX, randomY];
        }

        public List<Row> GetRangeMovement(Range range, Row center)
        {
            return GetRange(range, center, new GridPosition[0], RangeType.Movement);
        }

        public List<Row> GetAOERange(Range range, Row center, Row rowCharacter = null)
        {
            //if (m_lastAttackRange != null && m_lastAttackRange.Contains(center))
            //{
                List<Row> rows = GetRange(range, center, new GridPosition[0], RangeType.AOE, rowCharacter);
                rows.RemoveAll(x => x.impassible);

                return rows;
            //}

            //return null;
        }

        public List<Row> GetAttackRange(Range range, Row center)
        {
            List<Row> inRange = GetRange(range, center, new GridPosition[0], RangeType.AttackRange, center);
            List<Row> ret = new List<Row>();

            inRange.RemoveAll(x => Math.Abs(x.position.x - center.position.x) + Math.Abs(x.position.y - center.position.y) < range.startDistanceSize);


            for (int i = 0; i < inRange.Count; i++)
            {
                if (!Bresenham.Occlusion(this, center, inRange[i]))
                {
                    ret.Add(inRange[i]);
                }
            }

            m_lastAttackRange = ret;
            return ret;
        }

        public void Clear()
        {
            if (m_rows != null)
            {
                System.Array.Clear(m_rows, 0, m_rows.Length);
                m_size = GridPosition.Zero;
                m_rows = null;
            }
        }

        private List<Row> GetRange(Range range, Row originRow, GridPosition[] occupied, RangeType rangeType, Row characterRow = null)
        {

            if (rangeType == RangeType.AOE && (range.rangeType == Range.RangeType.HorizontallyLine || range.rangeType == Range.RangeType.VerticallyLine))
            {
                List<Row> ret = new List<Row>();

                // For lines AOE
                if (characterRow != null)
                {

                    for (int i = 0; i < range.size; i++)
                    {
                        if (range.rangeType == Range.RangeType.HorizontallyLine)
                        {
                            int offset = (int)Math.Floor((double)(range.size / 2));
                            // right - left
                            if (originRow.position.x != characterRow.position.x)
                            {
                                int finalOffset = (int)(originRow.position.y - offset + i);
                                if (finalOffset >= 0 && finalOffset < m_size.y && m_rows[(int)originRow.position.x, finalOffset].impassible == false)
                                    ret.Add(m_rows[(int)originRow.position.x, finalOffset]);
                            }

                            // Bottom - Top
                            if (originRow.position.y != characterRow.position.y)
                            {
                                int finalOffset = (int)(originRow.position.x - offset + i);
                                if (finalOffset >= 0 && finalOffset < m_size.x && m_rows[finalOffset, (int)originRow.position.y].impassible == false)
                                    ret.Add(m_rows[finalOffset, (int)originRow.position.y]);
                            }
                        }
                        else if (range.rangeType == Range.RangeType.VerticallyLine)
                        {
                            // right - left
                            if (originRow.position.x != characterRow.position.x)
                            {
                                int finalOffset = 0;
                                if (originRow.position.x > characterRow.position.x) // right
                                    finalOffset = (int)(originRow.position.x + i);
                                else // left
                                    finalOffset = (int)(originRow.position.x - i);

                                if (finalOffset >= 0 && finalOffset < m_size.x && m_rows[finalOffset, (int)originRow.position.y].impassible == false)
                                    ret.Add(m_rows[finalOffset, (int)originRow.position.y]);
                            }

                            // top - bottom
                            if (originRow.position.y != characterRow.position.y)
                            {
                                int finalOffset = 0;
                                if (originRow.position.y > characterRow.position.y) // top
                                    finalOffset = (int)(originRow.position.y + i);
                                else // bottom
                                    finalOffset = (int)(originRow.position.y - i);

                                if (finalOffset >= 0 && finalOffset < m_size.y && m_rows[(int)originRow.position.x, finalOffset].impassible == false)
                                    ret.Add(m_rows[(int)originRow.position.x, finalOffset]);
                            }
                        }
                    }
                }

                return ret;
            }
            else
            {
                List<Row> closed = new List<Row>();
                List<Dijkstra.RowPath> open = new List<Dijkstra.RowPath>();

                Dijkstra.RowPath originPath = new Dijkstra.RowPath();
                if (rangeType == RangeType.AOE) originPath.AddStaticRow(originRow);
                else originPath.AddRow(originRow);

                open.Add(originPath);

                while (open.Count > 0)
                {
                    Dijkstra.RowPath current = open[0];
                    open.Remove(open[0]);

                    if (closed.Contains(current.lastRow))
                    {
                        continue;
                    }
                    if (current.costOfPath > range.size + 1)
                    {
                        continue;
                    }

                    closed.Add(current.lastRow);

                    foreach (Row t in current.lastRow.GetNeighbors(this))
                    {
                        if (occupied.Contains(t.position) || (rangeType == RangeType.Movement && (t.impassible || t.occuped)))
                            continue;

                        if (rangeType == RangeType.Movement && Math.Abs(current.lastRow.height - t.height) > 1)
                            continue;

                        Dijkstra.RowPath newRowPath = new Dijkstra.RowPath(current);

                        if (rangeType == RangeType.AttackRange) newRowPath.AddStaticRow(t);
                        else newRowPath.AddRow(t);

                        open.Add(newRowPath);
                    }
                }

                if (range.rangeType == Range.RangeType.Cross)
                {
                    closed.RemoveAll(x => x.position.x != originRow.position.x && x.position.y != originRow.position.y);
                }

                if (rangeType == RangeType.Movement && closed.Contains(originRow))
                {
                    closed.Remove(originRow);
                }

                if (rangeType == RangeType.Movement || range.rangeType == Range.RangeType.Cross)
                {
                    closed.Distinct();
                }

                return closed;
            }
        }
    }
}