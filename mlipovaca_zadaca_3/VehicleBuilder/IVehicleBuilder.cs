using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3.VehicleBuilder
{
    interface IVehicleBuilder
    {
        Vehicle Build();
        IVehicleBuilder SetId(int id);
        IVehicleBuilder SetVehicleName(string vehicleName);
        IVehicleBuilder SetChargingTime(int chargingTime);
        IVehicleBuilder SetRange(int Range);
    }
}
