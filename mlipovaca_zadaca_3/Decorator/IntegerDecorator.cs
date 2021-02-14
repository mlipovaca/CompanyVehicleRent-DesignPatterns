using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3.Decorator
{
    public class IntegerDecorator : RowDecorator
    {
        private string Data;
        public IntegerDecorator(IRowTable row) : base(row)
        {
        }

        public override string MakeRow()
        {
            AddData();
            return base.MakeRow() + Data;
        }

        public void AddData()
        {
            string data = " {" + OutputHelper.Counter + ",-5}";
            OutputHelper.Counter++;
            Data = data;
        }
    }
}
