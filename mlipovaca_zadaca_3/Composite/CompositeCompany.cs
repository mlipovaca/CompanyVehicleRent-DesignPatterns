using mlipovaca_zadaca_3.Iterator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3.Composite
{
    public class CompositeCompany : IComponentCompany
    {
        private List<IComponentCompany> ChildList = new List<IComponentCompany>();
        private string MyName = "";
        private int MyId;

        public ICompositeIterator GetIterator()
        {
            return new CompaniesIterator(ChildList);
        }

        public void ShowData(int choice, DateTime dateFrom, DateTime dateTo, int activityId, string[] args)
        {
            for (ICompositeIterator iter = GetIterator(); iter.HasNext();)
            {
                IComponentCompany item = (IComponentCompany)iter.Next();
                item.ShowData(choice, dateFrom, dateTo, activityId, args);
            }
        }

        public void ShowAllAndContinueChilds(int choice, DateTime dateFrom, DateTime dateTo, int activityId, string[] args)
        {
            for (ICompositeIterator iter = GetIterator(); iter.HasNext();)
            {
                IComponentCompany item = (IComponentCompany)iter.Next();
                item.ShowAllAndContinueChilds(choice, dateFrom, dateTo, activityId, args);
            }
        }

        public IComponentCompany FindCompany(int companyId)
        {
            for (ICompositeIterator iter = GetIterator(); iter.HasNext();)
            {
                IComponentCompany item = (IComponentCompany)iter.Next();
                if (item.GetMyId() == companyId)
                    return item;

                var childSearch = item.FindCompany(companyId);
                if (childSearch != null)
                    return childSearch;
            }
            return null;
        }

        public void AddChild(IComponentCompany child)
        {
            ChildList.Add(child);
        }

        public List<IComponentCompany> GetChildList()
        {
            return ChildList;
        }

        public void SetMyName(string name)
        {
            MyName = name;
        }

        public string GetMyName()
        {
            return MyName;
        }
        public void SetMyId(int id)
        {
            MyId = id;
        }

        public int GetMyId()
        {
            return MyId;
        }

        public List<Location> GetMyLocations()
        {
            return null;
        }
    }
}
