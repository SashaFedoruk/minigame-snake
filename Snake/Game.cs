using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace nSnake
{
    class Game : Snake
    {
        private Map _map;
        private int _level;
        private Point _dot;
        private int _score;
        private Random rnd;
        private Point _prevPosDot;
        private Player _player;
        private event EventHandler _newScore;

        private ArrayList _otherPlayers;


        public Game(Map map, Snake snake, int level, Player pl)
            : base(snake)
        {
            _map = map;
            _level = level;
            _score = 0;
            rnd = new Random();
            _dot = new Point();
            NewPosDot();
            _prevPosDot = _dot;
            _newScore += Event;
            _player = pl;

            FileStream FS;

            FS = new FileStream("scores.dat", FileMode.Open, FileAccess.ReadWrite);
            if (FS.CanWrite)
            {
                BinaryFormatter BF = new BinaryFormatter();
                _otherPlayers = (ArrayList)BF.Deserialize(FS);
                FS.Close();
            }
            else
            {
                _otherPlayers = new ArrayList();
            }

        }

        public void Start()
        {
            MainMenu();
            Print();
            PrintInfo();
            ConsoleKeyInfo cki = Console.ReadKey(true);
            int i = 0;
            while (true)
            {
                if (_isLife)
                {
                    if (Console.KeyAvailable)
                    {
                        cki = Console.ReadKey(true);
                        MoveSnake(cki.Key);
                    }
                    else if (_moveTo != ConsoleKey.A)
                    {
                        MoveSnake(ConsoleKey.A);
                    }
                    else
                    {
                        continue;
                    }
                    PrintSnakeAndDot();
                    Thread.Sleep(100);
                }
                else
                {
                    GameOver();
                }
            }
        }

        void GameOver()
        {
            _player.SetScore(_score);
            Console.SetCursorPosition(3, 9);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("GAME OVER!!!");
            Console.ResetColor();
            Console.SetCursorPosition(0, 30);
            Console.ReadKey();
            SaveScore();
            
            Player tmp;

            if (IndexOfPlayer() != -1 && ((Player)_otherPlayers[IndexOfPlayer()]).Score < _player.Score)
            {
                tmp = new Player(_player.Name);
                tmp.SetScore(_player.Score);
                _otherPlayers[IndexOfPlayer()] = tmp;
            }
            else if (IndexOfPlayer() == -1)
            {
                tmp = new Player(_player.Name);
                tmp.SetScore(_player.Score);
                _otherPlayers.Add(tmp);
            }
            NewSnake();

            SaveScore();
            _player = new Player(_player.Name);
            _score = 0;
            MainMenu();
            Print();
            PrintInfo();
        }
        void PrintSnakeAndDot()
        {
            Console.SetCursorPosition(_dot.X, _dot.Y);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("*");
            Console.ResetColor();

            Console.SetCursorPosition(_prevPos.X, _prevPos.Y);
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write(" ");
            Console.ResetColor();

            Point tmp = (Point)_coordsSnake[0];

            Console.SetCursorPosition(tmp.X, tmp.Y);
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("*");
            Console.ResetColor();



            for (int i = 1; i < _coordsSnake.Count; i++)
            {

                tmp = (Point)_coordsSnake[i];
                Console.SetCursorPosition(tmp.X, tmp.Y);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write(" ");
                Console.ResetColor();
            }

        }
        public void Print()
        {
            var map = _map.CoordMap;
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    int tmp = _coordsSnake.IndexOf(new Point(j, i));
                    if (tmp == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("*");
                    }
                    else if (tmp != -1)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write(" ");
                    }
                    else if (map[i][j] == 1)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write(" ");
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
        void PrintInfo()
        {
            Console.SetCursorPosition(25, 2);
            Console.Write("                                   ");
            Console.SetCursorPosition(30, 2);
            Console.Write("Score: {0}", _score);
            Console.SetCursorPosition(0, 0);
        }

        public void Clear()
        {
            _otherPlayers.Clear();
            SaveScore();
        }
        public void MoveSnake(ConsoleKey key)
        {
            if (key == ConsoleKey.A && _moveTo != ConsoleKey.A)
            {
                key = _moveTo;
            }
            Console.CursorVisible = false;


            var map = _map.CoordMap;
            Point p = (Point)_coordsSnake[0];
            bool tMove = false;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (map[p.Y - 1][p.X] == 0 &&
                        _coordsSnake.IndexOf(new Point(p.X, p.Y - 1)) == -1 && TrueMove(key))
                    {
                        _moveTo = ConsoleKey.UpArrow;
                        NewCoordSnake(0, -1);
                        tMove = true;
                    }
                    break;

                case ConsoleKey.DownArrow:
                    if (map[p.Y + 1][p.X] == 0 &&
                        _coordsSnake.IndexOf(new Point(p.X, p.Y + 1)) == -1 && TrueMove(key))
                    {
                        _moveTo = ConsoleKey.DownArrow;
                        NewCoordSnake(0, 1);
                        tMove = true;
                    }
                    break;

                case ConsoleKey.LeftArrow:
                    if (map[p.Y][p.X - 1] == 0 &&
                        _coordsSnake.IndexOf(new Point(p.X - 1, p.Y)) == -1 && TrueMove(key))
                    {
                        _moveTo = ConsoleKey.LeftArrow;
                        NewCoordSnake(-1, 0);
                        tMove = true;
                    }
                    break;

                case ConsoleKey.RightArrow:
                    if (map[p.Y][p.X + 1] == 0 &&
                        _coordsSnake.IndexOf(new Point(p.X + 1, p.Y)) == -1 && TrueMove(key))
                    {
                        _moveTo = ConsoleKey.RightArrow;
                        NewCoordSnake(1, 0);
                        tMove = true;
                    }
                    break;

                case ConsoleKey.Escape:
                    SaveScore();
                    tMove = false;
                    break;

                default:
                    break;
            }
            if (!tMove && TrueMove(key))
            {
                _isLife = false;
                SaveScore();
            }
            NewLengthSnake();
            StatusDot();
        }

        public int IndexOfPlayer()
        {
            for (int i = 0; i < _otherPlayers.Count; i++)
            {
                if (((Player)_otherPlayers[i]).Name == _player.Name)
                {
                    return i;
                }
            }
            return -1;
        }
        public void SaveScore()
        {
            int idx = IndexOfPlayer();

            _player.SetScore(_score);
            if (idx != -1)
            {
                Player tmp = (Player)_otherPlayers[idx];
                if (tmp.Score < _player.Score)
                {
                    tmp.SetScore(_player.Score);
                    _otherPlayers[idx] = tmp;
                }
            }
            else
            {
                _otherPlayers.Add(_player);
            }
            _otherPlayers.Sort(new SortedScore());
            FileStream FS = new FileStream("scores.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryFormatter BF = new BinaryFormatter();
            BF.Serialize(FS, _otherPlayers);
            FS.Close();
        }
        public static Player RegisterPlayer()
        {
            String name = "";
            String patt = @"^[A-Za-z]{2,15}$";
            while (name == "")
            {
                Console.SetCursorPosition(40, 10);
                Console.Write("Please enter your name: ");
                Console.SetCursorPosition(45, 11);
                Console.Write("                                               ");
                Console.SetCursorPosition(45, 11);
                String tmp = Console.ReadLine();
                if (Regex.IsMatch(tmp, patt))
                {
                    name = tmp;
                }
                else
                {
                    Console.SetCursorPosition(45, 15);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Invalid Input!");
                    Console.ResetColor();
                    Thread.Sleep(500);
                    Console.SetCursorPosition(45, 15);
                    Console.Write("                      ");

                }
            }
            Console.SetCursorPosition(40, 10);
            Console.Write("                                           ");
            Console.SetCursorPosition(45, 11);
            Console.Write("                                           ");
            Console.SetCursorPosition(0, 0);
            return new Player(name);
        }

        public bool TrueMove(ConsoleKey key)
        {
            if (_moveTo == ConsoleKey.UpArrow)
            {
                if (key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow || key == ConsoleKey.UpArrow)
                {
                    return true;
                }
            }
            else if (_moveTo == ConsoleKey.DownArrow)
            {
                if (key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow || key == ConsoleKey.DownArrow)
                {
                    return true;
                }
            }
            else if (_moveTo == ConsoleKey.LeftArrow)
            {
                if (key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow || key == ConsoleKey.LeftArrow)
                {
                    return true;
                }
            }
            else if (_moveTo == ConsoleKey.RightArrow)
            {
                if (key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow || key == ConsoleKey.RightArrow)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
            return false;
        }

        public void PrintMainMenu()
        {
            Console.SetCursorPosition(40, 10);
            Console.Write("----------------------------");
            Console.SetCursorPosition(40, 11);
            Console.Write("|           MENU           |");
            Console.SetCursorPosition(40, 12);
            Console.Write("|                          |");
            Console.SetCursorPosition(40, 15);
            Console.Write("|                          |");
            Console.SetCursorPosition(40, 16);
            Console.Write("----------------------------");
        }
        public void MainMenu()
        {
            Console.Clear();
            PrintMainMenu();
            ArrayList menu = new ArrayList();
            menu.Add("   Start game   ");
            menu.Add("   Print Score  ");
            int idxAct = 0;
            ConsoleKeyInfo cki;
            while (idxAct != -1)
            {

                for (int i = 0; i < menu.Count; i++)
                {
                    Console.SetCursorPosition(40, 13 + i);
                    Console.Write("|    ");
                    if (idxAct == i)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.Write(menu[i]);
                    Console.ResetColor();
                    Console.Write("      |");
                }
                cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.DownArrow && idxAct < menu.Count - 1)
                {
                    idxAct++;
                }
                else if (cki.Key == ConsoleKey.UpArrow && idxAct > 0)
                {
                    idxAct--;
                }
                else if (cki.Key == ConsoleKey.Enter)
                {
                    if (idxAct == 0)
                    {
                        idxAct = -1;
                        Console.Clear();
                        Console.SetCursorPosition(0, 0);
                        break;
                    }
                    else
                    {
                        PrintScore();
                        Console.Clear();
                        PrintMainMenu();
                    }
                }
            }

        }
        public void PrintScore()
        {
            Console.Clear();
            Console.WriteLine("---------------------------------------");
            int i = 1;
            foreach (Player pl in _otherPlayers)
            {
                Console.WriteLine("| {0} | {1} | {2} |", (i++).ToString().PadLeft(2), pl.Name.PadLeft(18), pl.Score.ToString().PadLeft(10));
            }
            Console.WriteLine("---------------------------------------");
            Console.ReadKey();
        }
        public void StatusDot()
        {
            Point p = (Point)_coordsSnake[0];
            if (_dot == p)
            {
                _score += 100;

                PrintInfo();
                NewPosDot();
                _newScore(this, new EventArgs());
            }
        }
        public void NewPosDot()
        {
            bool tPosDot = false;
            _prevPosDot = _dot;
            while (!tPosDot)
            {
                _dot.X = rnd.Next(1, 19);
                _dot.Y = rnd.Next(1, 19);
                if (_coordsSnake.IndexOf(_dot) == -1)
                {
                    tPosDot = true;
                }
            }
            Console.SetCursorPosition(25, 5);
            Console.Write("      ");
            Console.SetCursorPosition(0, 0);
        }
        public void NewLengthSnake()
        {
            Point p = (Point)_coordsSnake[_coordsSnake.Count - 1];
            if (p == _prevPosDot)
            {
                _coordsSnake.Add(_prevPosDot);
                _prevPosDot = new Point();
            }
        }
    }
}
