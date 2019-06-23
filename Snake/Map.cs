using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace nSnake
{
    class Map
    {
        protected int[][] _coordMap;



        public int[][] CoordMap
        {
            get
            {
                return _coordMap;
            }
        }
        public Map(int[][] cMap)
        {
            _coordMap = new int[cMap.Length][];
            for (int i = 0; i < cMap.Length; i++)
            {
                _coordMap[i] = new int[cMap[i].Length];
                for (int j = 0; j < cMap[i].Length; j++)
                {
                    _coordMap[i][j] = cMap[i][j];
                }
            }
        }

        public void PrintMap()
        {
            foreach (var row in _coordMap)
            {
                foreach (var col in row)
                {
                    Console.Write(col);
                }
                Console.WriteLine();
            }
        }


    }
}
