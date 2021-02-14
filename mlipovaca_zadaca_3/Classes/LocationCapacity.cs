using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3
{
    public class LocationCapacity
    {
        public int LocationId;
        public int VehicleId;
        public int Seat;
        public int AvailableVehicles;

        public void SetLocationId(int locationId)
        {
            LocationId = locationId;
        }
        public int GetLocationId()
        {
            return LocationId;
        }

        public void SetVehicleId(int vehicleId)
        {
            VehicleId = vehicleId;
        }
        public int GetVehicleId()
        {
            return VehicleId;
        }
        public void SetSeat(int seat)
        {
            Seat = seat;
        }
        public int GetSeat()
        {
            return Seat;
        }

        public void SetAvailableVehicles(int availableVehicles)
        {
            AvailableVehicles = availableVehicles;
        }
        public int GetAvailableVehicles()
        {
            return AvailableVehicles;
        }
    }
}
