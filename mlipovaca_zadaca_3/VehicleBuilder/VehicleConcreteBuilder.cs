using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3.VehicleBuilder
{
    class VehicleConcreteBuilder : IVehicleBuilder
    {
        private Vehicle vehicle;

        public VehicleConcreteBuilder()
        {
            vehicle = new Vehicle();
        }

        public Vehicle Build()
        {
            return vehicle;
        }

        public IVehicleBuilder SetId(int id)
        {
            vehicle.SetId(id);
            return this;
        }

        public IVehicleBuilder SetVehicleName(string vehicleName)
        {
            vehicle.SetVehicleName(vehicleName);
            return this;
        }

        public IVehicleBuilder SetChargingTime(int chargingTime)
        {
            vehicle.SetChargingTime(chargingTime);
            return this;
        }

        public IVehicleBuilder SetRange(int range)
        {
            vehicle.SetRange(range);
            return this;
        }
    }
}
