using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3.Decorator
{
    public abstract class RowDecorator : IRowTable
    {
        protected IRowTable Row;
        protected int Index = 0;
        public RowDecorator(IRowTable row)
        {
            Row = row;
        }

        public virtual string MakeRow()
        {
            return Row.MakeRow();
        }
    }
}
