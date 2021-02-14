using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3.VehicleBuilder
{
    class VehicleBuildDirector
    {

        private IVehicleBuilder builder;

        public VehicleBuildDirector(IVehicleBuilder builder)
        {
            this.builder = builder;
        }

        public Vehicle Construct(
            int id, string vehicleName, int chargingTime, int range)
        {
            return builder.SetId(id)
                          .SetVehicleName(vehicleName)
                          .SetChargingTime(chargingTime)
                          .SetRange(range)
                          .Build();
        }
    }
}
