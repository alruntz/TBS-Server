using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBSEngine
{
    public static class GridHelper
    {
        public static int[,] MapToGrid(TBS.Models.Map map)
        {
            int[,] grid = new int[map.Width, map.Length];
            for (int i = 0; i < map.Tiles.Count; i++)
            {
                grid[map.Tiles[i].XPosition, map.Tiles[i].YPosition] = map.Tiles[i].Impasible ? -1 : map.Tiles[i].Height;
            }

            return grid;
        }
    }
}
