using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3.Decorator
{
    public class DecimalDecorator : RowDecorator
    {
        private string Data;
        public DecimalDecorator(IRowTable row) : base(row)
        {
        }

        public override string MakeRow()
        {
            AddData();
            return base.MakeRow() + Data;
        }

        public void AddData()
        {
            string data = " {" + OutputHelper.Counter + ",2}";
            OutputHelper.Counter++;
            Data = data;
        }

    }
}
