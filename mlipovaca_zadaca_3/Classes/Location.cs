using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3
{
    public class Location
    {
        public int Id;
        public string LocationName;
        public string LocationAddress;
        public string Gps;

        public void SetId(int id)
        {
            Id = id;
        }
        public int GetId()
        {
            return Id;
        }

        public void SetLocationName(string locationName)
        {
            LocationName = locationName;
        }
        public string GetLocationName()
        {
            return LocationName;
        }

        public void SetLocationAddress(string locationAddress)
        {
            LocationAddress = locationAddress;
        }
        public string GetLocationAddress()
        {
            return LocationAddress;
        }

        public void SetGps(string gps)
        {
            Gps = gps;
        }
        public string GetGps()
        {
            return Gps;
        }
    }
}
