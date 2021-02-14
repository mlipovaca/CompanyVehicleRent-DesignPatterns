using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3
{
    public class PriceList
    {
        public int VehicleId;
        public string Rent;
        public string ByHour;
        public string ByKilometers;

        public void SetVehicleId(int vehicleId)
        {
            VehicleId = vehicleId;
        }
        public int GetVehicleId()
        {
            return VehicleId;
        }

        public void SetRent(string rent)
        {
            Rent = rent;
        }
        public string GetRent()
        {
            return Rent;
        }

        public void SetByHour(string byHour)
        {
            ByHour = byHour;
        }
        public string GetByHour()
        {
            return ByHour;
        }

        public void SetByKilometers(string byKilometers)
        {
            ByKilometers = byKilometers;
        }
        public string GetByKilometers()
        {
            return ByKilometers;
        }
    }
}
