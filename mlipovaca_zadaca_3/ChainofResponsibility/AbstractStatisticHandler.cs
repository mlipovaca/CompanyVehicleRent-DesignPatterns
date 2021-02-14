using mlipovaca_zadaca_3.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3.ChainofResponsibility
{
    public abstract class AbstractStatisticHandler
    {
        public static int BillStat = 1;

        protected int Option;
        protected AbstractStatisticHandler Next;

        public AbstractStatisticHandler SetNext(AbstractStatisticHandler handler)
        {
            Next = handler;
            return this;
        }

        public void DoWork(int option, Person person)
        {
            if (Option == option)
            {
                Calculate(person);
            }
            else
            {
                Next.DoWork(option, person);
            }
        }

        public abstract void Calculate(Person person);

        protected static double GetPercentage(int part, int whole)
        {
            double ratio = (double)part / whole;
            double percentage = ratio * 100;
            double output = Math.Round(percentage, 2);
            return output;
        }
    }
}
