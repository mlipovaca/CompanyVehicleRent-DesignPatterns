using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3
{
    public class Vehicle
    {
        private int Identifier;
        public int Id;
        public string VehicleName;
        public int ChargingTime;
        public int Range;

        public Vehicle()
        {
            FileHelper.vehicleCounter++;
            this.Identifier = FileHelper.vehicleCounter;
        }

        public int GetUniqueIdentifier()
        {
            return Identifier;
        }

        public void SetId(int id)
        {
            Id = id;
        }
        public int GetId()
        {
            return Id;
        }

        public void SetVehicleName(string vehicleName)
        {
            VehicleName = vehicleName;
        }
        public string GetVehicleName()
        {
            return VehicleName;
        }

        public void SetChargingTime(int chargingTime)
        {
            ChargingTime = chargingTime;
        }
        public int GetChargingTime()
        {
            return ChargingTime;
        }

        public void SetRange(int range)
        {
            Range = range;
        }
        public int GetRange()
        {
            return Range;
        }
    }
}
