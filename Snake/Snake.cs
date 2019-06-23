using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;
using System.Timers;

namespace nSnake
{
    class Snake
    {
        protected ArrayList _coordsSnake;
        protected bool _isLife;
        protected ConsoleKey _moveTo;
        protected Point _prevPos;

        Timer aTimer;
        public ArrayList CoordsSnake
        {
            get
            {
                return _coordsSnake;
            }
        }

        public Snake()
        {
            NewSnake();
        }
        public Snake(Snake s)
        {
            _isLife = s._isLife;
            _coordsSnake = s._coordsSnake;
            _moveTo = s._moveTo;
        }
        public Snake(ArrayList sn, bool isLife)
        {
            _coordsSnake = sn;
            _isLife = isLife;
        }
        public void NewSnake()
        {
            _coordsSnake = new ArrayList();
            _coordsSnake.Add(new Point(1, 3));
            _coordsSnake.Add(new Point(1, 2));
            _coordsSnake.Add(new Point(1, 1));

            _isLife = true;
            _moveTo = ConsoleKey.A;
        }

        public void NewCoordSnake(int x, int y)
        {
            _prevPos = (Point)_coordsSnake[_coordsSnake.Count - 1];
            ArrayList tmp = new ArrayList();
            Point p = (Point)_coordsSnake[0];
            p.X += x;
            p.Y += y;
            tmp.Add(p);
            for (int i = 0; i < _coordsSnake.Count; i++)
            {
                tmp.Add(_coordsSnake[i]);
            }
            for (int i = 0; i < _coordsSnake.Count; i++)
            {
                _coordsSnake[i] = tmp[i];
            }
            
        }

        public void Event(object sender, EventArgs e)
        {
            Console.SetCursorPosition(25, 5);
            Console.Write("+100");
            Console.SetCursorPosition(0, 0);
            aTimer = new Timer(1500);
            aTimer.Enabled = true;
            aTimer.Elapsed += new ElapsedEventHandler(ClearInfoEvent);
        }
        public void ClearInfoEvent(object sender, ElapsedEventArgs e)
        {
            Console.SetCursorPosition(25, 5);
            Console.Write("      ");
            Console.SetCursorPosition(0, 0);
            aTimer.Stop();
            aTimer.Enabled = false;
        }

    }
}
