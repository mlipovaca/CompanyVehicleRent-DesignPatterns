using mlipovaca_zadaca_3.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3.Iterator
{
    public class CompaniesIterator : ICompositeIterator
    {
        private List<IComponentCompany> ChildList = new List<IComponentCompany>();
        private int Index;

        public CompaniesIterator(List<IComponentCompany> childList)
        {
            ChildList = childList;
        }

        public bool HasNext()
        {
            if (Index < ChildList.Count)
            {
                return true;
            }
            return false;
        }

        public Object Next()
        {
            if (this.HasNext())
            {
                return ChildList[Index++];
            }
            return null;
        }
    }
}
