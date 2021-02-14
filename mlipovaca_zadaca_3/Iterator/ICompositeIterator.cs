using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3.Iterator
{
    public interface ICompositeIterator
    {
        bool HasNext();
        Object Next();
    }
}
