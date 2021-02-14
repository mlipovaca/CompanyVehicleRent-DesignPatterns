using mlipovaca_zadaca_3.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3
{
    public class VehiclesSingleton
    {
        private static volatile VehiclesSingleton vehicles = new VehiclesSingleton();

        public CompositeCompany Company;

        private VehiclesSingleton() { }

        public static VehiclesSingleton GetVehiclesInstance()
        {
            return vehicles;
        }

        public List<Vehicle> ListVehicles;

        public List<Location> ListLocations;

        public List<LocationCapacity> ListLocationCapacities;

        public List<Person> ListPersons;

        public List<PriceList> ListPriceList;

        public List<Activity> ListActivity;

        public List<Company> ListCompanies;

        public void SetCompositeCompany()
        {
            if (Company == null)
            {
                Company = new CompositeCompany();
                Company.SetMyId(-1);
            }
        }

        public CompositeCompany GetCompositeCompany()
        {
            return Company;
        }

        public Location FindLocation(int locationId)
        {
            foreach (var location in ListLocations)
            {
                if (location.GetId() == locationId)
                {
                    return location;
                }
            }
            return null;
        }
    }
}
