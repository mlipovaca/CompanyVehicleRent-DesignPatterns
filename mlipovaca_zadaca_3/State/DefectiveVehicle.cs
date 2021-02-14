using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3.State
{
    public class DefectiveVehicle : IVehicleStatusState
    {
        public void ChangeVehicleStatus(VehicleState context)
        {
            context.State = new AvailableVehicle();
        }
    }
}
