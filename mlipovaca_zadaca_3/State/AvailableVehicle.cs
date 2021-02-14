using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3.State
{
    public class AvailableVehicle : IVehicleStatusState
    {
        public void ChangeVehicleStatus(VehicleState context)
        {
            context.State = new RentedVehicle();
        }
    }
}
