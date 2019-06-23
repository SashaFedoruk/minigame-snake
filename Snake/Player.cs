using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nSnake
{
     [Serializable]
    class Player
    {
        private String _name;
        private int _score;

        public int Score
        {
            get
            {
                return _score;
            }
        }
        public String Name
        {
            get
            {
                return _name;
            }
        }
        public void AddScore(int sc)
        {
            _score += sc;
        }
        public void SetScore(int sc)
        {
            _score = sc;
        }

        public Player(String name)
        {
            _name = name;
            _score = 0;
        }
    }
}
