using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3.Composite
{
    public interface IComponentCompany
    {
        void ShowData(int choice, DateTime dateFrom, DateTime dateTo, int activityId, string[] args);
        void ShowAllAndContinueChilds(int choice, DateTime dateFrom, DateTime dateTo, int activityId, string[] args);
        void AddChild(IComponentCompany child);
        List<IComponentCompany> GetChildList();
        string GetMyName();
        int GetMyId();
        List<Location> GetMyLocations();
        IComponentCompany FindCompany(int companyId);
    }
}
