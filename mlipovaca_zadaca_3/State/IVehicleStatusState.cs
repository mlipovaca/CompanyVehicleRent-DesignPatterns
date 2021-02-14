using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3.State
{
    public interface IVehicleStatusState
    {
        void ChangeVehicleStatus(VehicleState context);
    }
}
