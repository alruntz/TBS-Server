using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.GridSystem
{
    public struct GridPosition
    {
        public int x;
        public int y;

        public GridPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public GridPosition(string str)
        {
            Console.WriteLine("__START__");
            int x = 0;
            int y = 0;
            Console.WriteLine("__1__");
            string[] splitted = str.Split(',');
            Console.WriteLine("__2__");
            if (splitted != null && splitted.Length == 2)
            {
                Console.WriteLine("__3__");
                if (int.TryParse(splitted[0], out x))
                    this.x = x;
                else
                    this.x = -1;
                Console.WriteLine("__4__");
                if (int.TryParse(splitted[1], out y))
                    this.y = y;
                else
                    this.y = -1;
            }
            else
            {
                Console.WriteLine("__5__");
                this.x = -1;
                this.y = -1;
            }
            Console.WriteLine("__END__");
        }

        public static GridPosition Zero => new GridPosition(0, 0);

        public override string ToString()
        {
            return x + "," + y;
        }

        public override int GetHashCode()
        {
            return x * 0x00010000 + y;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj);
        }

        public bool Equals(GridPosition b)
        {
            return this == b;
        }

        public static bool operator == (GridPosition a, GridPosition b)
        {
            if (a.x == b.x && a.y == b.y)
                return true;

            return false;
        }

        public static bool operator != (GridPosition a, GridPosition b)
        {
            if (a.x != b.x || a.y != b.y)
                return true;

            return false;
        }
    }
}
