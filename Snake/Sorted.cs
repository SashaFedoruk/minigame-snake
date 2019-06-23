using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nSnake
{
    public class SortedScore : IComparer
    {

        int IComparer.Compare(Object x, Object y)
        {
            return ((new CaseInsensitiveComparer()).Compare(((Player)y).Score, ((Player)x).Score));
        }

    }
}
