using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

using FS;

namespace FS.GridSystem
{
    public class Bresenham
    {

        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public static double DistancePoints(GridPosition a, GridPosition b)
        {
            int xDist = a.x - b.x;
            int yDist = a.y - b.y;

            return Math.Sqrt(xDist * xDist + yDist * yDist);
        }

        public static bool Occlusion(Grid grid, Grid.Row start, Grid.Row end)
        {
            List<Grid.Row> inRange = BresenhamLine(grid, start, end);
            inRange = inRange.OrderBy(x => DistancePoints(start.position, x.position)).ToList();
            Grid.Row last = null;
            bool isDown = false;

            for (int i = 0; i < inRange.Count; i++)
            {
                if (inRange[i] != start)
                {
                    if (last != null && last.height > start.height && inRange[i].height < last.height)
                        isDown = true;

                    //if (isDown == true && inRange[i].height >= start.height && last.height <= inRange[i].height)
                    //    isDown = false;

                    if (isDown == true/* && inRange[i] == end*/)
                        return true;

                    if (inRange[i].impassible)
                    {
                        return true;
                    }
                    if (inRange[i].occuped)
                    {
                        if (end != inRange[i])
                            return true;
                    }
                }
                last = inRange[i];
            }

            return false;
        }

        public static List<Grid.Row> BresenhamLine(Grid grid, Grid.Row start, Grid.Row end)
        {
            int x0, y0, x1, y1;

            x0 = (int)start.position.x;
            x1 = (int)end.position.x;
            y0 = (int)start.position.y;
            y1 = (int)end.position.y;

            List<Grid.Row> result = new List<Grid.Row>();

            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            int deltax = x1 - x0;
            int deltay = Math.Abs(y1 - y0);
            int error = 0;
            int ystep;
            int y = y0;
            if (y0 < y1) ystep = 1; else ystep = -1;
            for (int x = x0; x <= x1; x++)
            {
                if (steep) result.Add(grid.Rows[y, x]);
                else result.Add(grid.Rows[x, y]);
                error += deltay;
                if (2 * error >= deltax)
                {
                    y += ystep;
                    error -= deltax;
                }
            }

            return result;
        }
    }
}
