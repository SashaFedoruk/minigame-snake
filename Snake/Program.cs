using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace nSnake
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream FS = new FileStream("map.dat",FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryFormatter BF = new BinaryFormatter();
            int[][] map = (int[][])BF.Deserialize(FS);
            FS.Close();
            

            Map m = new Map(map);
            ArrayList cSnake = new ArrayList();
            cSnake.Add(new Point(1, 1));
            cSnake.Add(new Point(2, 1));
            cSnake.Add(new Point(3, 1));
            cSnake.Add(new Point(4, 1));



            Player pl = Game.RegisterPlayer();
            Snake s = new Snake();
            Game g = new Game(m, s, 1, pl);
            //g.Clear();
            g.Start();
            Console.WriteLine();


        }
    }
}
