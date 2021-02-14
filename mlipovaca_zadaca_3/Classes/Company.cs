using mlipovaca_zadaca_3.Composite;
using mlipovaca_zadaca_3.Decorator;
using mlipovaca_zadaca_3.Iterator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3
{
    public class Company : IComponentCompany
    {
        public int Id;
        public string CompanyName;
        public Company SuperiorUnit;
        public List<Location> Locations = new List<Location>();

        private List<IComponentCompany> mChildList = new List<IComponentCompany>();
        public void AddChild(IComponentCompany child)
        {
            mChildList.Add(child);
        }

        public List<IComponentCompany> GetChildList()
        {
            return mChildList;
        }

        public void SetMyName(string name)
        {
            CompanyName = name;
        }

        public string GetMyName()
        {
            return CompanyName;
        }

        public void SetMyId(int id)
        {
            Id = id;
        }

        public int GetMyId()
        {
            return Id;
        }

        public void SetSuperiorUnit(Company superiorUnit)
        {
            SuperiorUnit = superiorUnit;
        }

        public Company GetSuperiorUnit()
        {
            return SuperiorUnit;
        }
        public void ShowData(int choice, DateTime dateFrom, DateTime dateTo, int activityId, string[] args)
        {
            if (activityId == 6)
            {
                if (choice == 1)
                {
                    string outputLocation = "";
                    string superiorUnit = "";
                    for (int i = 0; i < Locations.Count; i++)
                    {
                        if (i != 0)
                        {
                            for (int j = 0; j < 64; j++)
                            {
                                outputLocation += " ";
                            }
                        }
                        if (i == Locations.Count - 1)
                            outputLocation += Locations[i].GetLocationName();
                        else
                        {
                            outputLocation += Locations[i].GetLocationName() + Environment.NewLine;

                        }
                    }
                    if (outputLocation == "")
                    {
                        outputLocation = "Nema lokacije";
                    }

                    if (this.GetSuperiorUnit() == null)
                    {
                        superiorUnit = "Nema nadređenu jedinicu";
                    }
                    else
                    {
                        superiorUnit = this.GetSuperiorUnit().CompanyName;
                    }

                    OutputHelper.Counter = 0;
                    IRowTable rowTable =
                        new TextDecorator(
                            new TextDecorator(
                                new TextDecorator(
                                    new ConcreteRow())));
                    string format = rowTable.MakeRow();
                    string output = String.Format(format, outputLocation, superiorUnit, this.CompanyName);
                    Console.WriteLine(output);
                    Console.WriteLine(new String('_', 90));
                }
                else if (choice == 2)
                {
                    VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

                    string totalLocationInformation = "";
                    string vehicleInformation = "";
                    int counterLocation = 0;
                    string superiorUnit = "";

                    for (int i = 0; i < Locations.Count; i++)
                    {
                        string locationInformation = "";

                        var allVehiclesForLocation = vehicle.ListLocationCapacities.Where(x => x.LocationId == Locations[i].Id).ToList();

                        int counter = 0;
                        int defVehCounterActivity = 0;
                        string totalVehicleInformation = "";

                        foreach (var vehName in allVehiclesForLocation)
                        {
                            vehicleInformation = "";

                            vehicleInformation += vehicle.ListVehicles.Where(x => x.Id == vehName.GetVehicleId()).FirstOrDefault().GetVehicleName();
                            vehicleInformation += OutputHelper.FillEmptySpace(vehicleInformation.Length, 30);
                            vehicleInformation += vehName.GetSeat();
                            vehicleInformation += OutputHelper.FillEmptySpace(vehicleInformation.Length, 35);
                            vehicleInformation += vehName.GetAvailableVehicles();
                            vehicleInformation += OutputHelper.FillEmptySpace(vehicleInformation.Length, 40);
                            if (args.Contains("-s"))
                            {
                                var getDefectiveVeh = vehicle.ListActivity.Where(x => x.VehicleId == vehName.GetVehicleId()).Where(x => x.LocationId == Locations[i].Id).ToList();

                                foreach (var defVeh in getDefectiveVeh)
                                {
                                    if (defVeh.GetDescriptionProblem() != null)
                                    {
                                        defVehCounterActivity++;
                                    }
                                }
                                vehicleInformation += defVehCounterActivity;
                                defVehCounterActivity = 0;
                            }
                            else
                            {
                                var defVehs = OutputHelper.defectiveVehs.Where(x => x.Value.Item1 == vehName.GetLocationId()).Where(x => x.Value.Item2 == vehName.GetVehicleId()).FirstOrDefault();
                                if (defVehs.Value != null)
                                {
                                    vehicleInformation += defVehs.Value.Item3;
                                }
                                else
                                {
                                    vehicleInformation += 0;
                                }
                            }

                            vehicleInformation += OutputHelper.FillEmptySpace(vehicleInformation.Length, 45);

                            if (counter != 0)
                            {
                                totalVehicleInformation += Environment.NewLine + OutputHelper.FillEmptySpace(0, 96) + vehicleInformation;
                            }
                            else
                            {
                                totalVehicleInformation += vehicleInformation;
                            }
                            counter++;
                        }

                        locationInformation += Locations[i].GetLocationName();

                        if (counterLocation != 0)
                        {
                            totalLocationInformation += Environment.NewLine + OutputHelper.FillEmptySpace(0, 65) + locationInformation;
                            totalLocationInformation += OutputHelper.FillEmptySpace(locationInformation.Length, 32) + totalVehicleInformation;
                        }
                        else
                        {
                            totalLocationInformation += locationInformation;
                            totalLocationInformation += OutputHelper.FillEmptySpace(locationInformation.Length, 32) + totalVehicleInformation;
                        }

                        counterLocation++;

                    }

                    if (this.GetSuperiorUnit() == null)
                    {
                        superiorUnit = "Nema nadređenu jedinicu";
                    }
                    else
                    {
                        superiorUnit = this.GetSuperiorUnit().CompanyName;
                    }

                    OutputHelper.Counter = 0;
                    IRowTable rowTable =
                        new LocationCapacitiesDecorator(
                            new TextDecorator(
                                  new TextDecorator(
                                         new ConcreteRow())));
                    string format = rowTable.MakeRow();
                    string output = String.Format(format, totalLocationInformation, superiorUnit, this.CompanyName);
                    Console.WriteLine(output);
                    Console.WriteLine(new String('_', 210));
                }
            }
            else if (activityId == 7)
            {
                if (choice == 1)
                {
                    string outputLocation = "";
                    string superiorUnit = "";
                    for (int i = 0; i < Locations.Count; i++)
                    {
                        if (i != 0)
                        {
                            for (int j = 0; j < 64; j++)
                            {
                                outputLocation += " ";
                            }
                        }
                        if (i == Locations.Count - 1)
                            outputLocation += Locations[i].GetLocationName();
                        else
                        {
                            outputLocation += Locations[i].GetLocationName() + Environment.NewLine;

                        }
                    }
                    if (outputLocation == "")
                    {
                        outputLocation = "Nema lokacije";
                    }

                    if (this.GetSuperiorUnit() == null)
                    {
                        superiorUnit = "Nema nadređenu jedinicu";
                    }
                    else
                    {
                        superiorUnit = this.GetSuperiorUnit().CompanyName;
                    }

                    OutputHelper.Counter = 0;
                    IRowTable rowTable =
                        new TextDecorator(
                            new TextDecorator(
                                new TextDecorator(
                                    new ConcreteRow())));
                    string format = rowTable.MakeRow();
                    string output = String.Format(format, outputLocation, superiorUnit, this.CompanyName);
                    Console.WriteLine(output);
                    Console.WriteLine(new String('_', 90));
                }
                else if (choice == 2)
                {
                    VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

                    string totalLocationInformation = "";
                    string vehicleInformation = "";
                    int counterLocation = 0;
                    string superiorUnit = "";

                    for (int i = 0; i < Locations.Count; i++)
                    {
                        string locationInformation = "";

                        var allVehiclesForLocation = vehicle.ListLocationCapacities.Where(x => x.LocationId == Locations[i].Id).ToList();

                        int counter = 0;
                        string totalVehicleInformation = "";

                        foreach (var vehName in allVehiclesForLocation)
                        {
                            vehicleInformation = "";

                            vehicleInformation += vehicle.ListVehicles.Where(x => x.Id == vehName.GetVehicleId()).FirstOrDefault().GetVehicleName();
                            vehicleInformation += OutputHelper.FillEmptySpace(vehicleInformation.Length, 30);
                            var rentVeh = OutputHelper.rentVehs.Where(x => x.Value.Item2 == vehName.GetLocationId()).Where(x => x.Value.Item3 == vehName.GetVehicleId()).FirstOrDefault();
                            if (rentVeh.Value != null)
                            {
                                vehicleInformation += rentVeh.Value.Item4;
                            }
                            else
                            {
                                vehicleInformation += 0;
                            }
                            vehicleInformation += OutputHelper.FillEmptySpace(vehicleInformation.Length, 35);
                            var rentVehTime = OutputHelper.rentVehsTime.Where(x => x.Value.Item1 == vehName.GetLocationId()).Where(x => x.Value.Item2 == vehName.GetVehicleId())
                                .Where(x => x.Value.Item3 >= dateFrom).Where(x => x.Value.Item4 <= dateTo).FirstOrDefault();
                            if (rentVehTime.Value != null)
                            {
                                vehicleInformation += rentVehTime.Value.Item3 + "     ";
                                vehicleInformation += rentVehTime.Value.Item4;
                            }
                            else
                            {
                                vehicleInformation += 0;
                            }

                            if (counter != 0)
                            {
                                totalVehicleInformation += Environment.NewLine + OutputHelper.FillEmptySpace(0, 96) + vehicleInformation;
                            }
                            else
                            {
                                totalVehicleInformation += vehicleInformation;
                            }
                            counter++;
                        }

                        locationInformation += Locations[i].GetLocationName();

                        if (counterLocation != 0)
                        {
                            totalLocationInformation += Environment.NewLine + OutputHelper.FillEmptySpace(0, 65) + locationInformation;
                            totalLocationInformation += OutputHelper.FillEmptySpace(locationInformation.Length, 32) + totalVehicleInformation;
                        }
                        else
                        {
                            totalLocationInformation += locationInformation;
                            totalLocationInformation += OutputHelper.FillEmptySpace(locationInformation.Length, 32) + totalVehicleInformation;
                        }

                        counterLocation++;

                    }

                    if (this.GetSuperiorUnit() == null)
                    {
                        superiorUnit = "Nema nadređenu jedinicu";
                    }
                    else
                    {
                        superiorUnit = this.GetSuperiorUnit().CompanyName;
                    }

                    OutputHelper.Counter = 0;
                    IRowTable rowTable =
                        new LocationCapacitiesDecorator(
                            new TextDecorator(
                                  new TextDecorator(
                                         new ConcreteRow())));
                    string format = rowTable.MakeRow();
                    string output = String.Format(format, totalLocationInformation, superiorUnit, this.CompanyName);
                    Console.WriteLine(output);
                    Console.WriteLine(new String('_', 210));
                }
                else if (choice == 3)
                {
                    VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

                    string totalLocationInformation = "";
                    string vehicleInformation = "";
                    int counterLocation = 0;
                    string superiorUnit = "";

                    for (int i = 0; i < Locations.Count; i++)
                    {
                        string locationInformation = "";

                        var allVehiclesForLocation = vehicle.ListLocationCapacities.Where(x => x.LocationId == Locations[i].Id).ToList();

                        int counter = 0;
                        string totalVehicleInformation = "";

                        foreach (var vehName in allVehiclesForLocation)
                        {
                            vehicleInformation = "";

                            vehicleInformation += vehicle.ListVehicles.Where(x => x.Id == vehName.GetVehicleId()).FirstOrDefault().GetVehicleName();
                            vehicleInformation += OutputHelper.FillEmptySpace(vehicleInformation.Length, 30);
                            var rentVeh = OutputHelper.rentVehs.Where(x => x.Value.Item2 == vehName.GetLocationId()).Where(x => x.Value.Item3 == vehName.GetVehicleId()).LastOrDefault();
                            if (rentVeh.Value != null)
                            {
                                vehicleInformation += rentVeh.Value.Item5;
                            }
                            else
                            {
                                vehicleInformation += 0;
                            }
                            vehicleInformation += OutputHelper.FillEmptySpace(vehicleInformation.Length, 35);
                            var rentVehTime = OutputHelper.rentVehsTime.Where(x => x.Value.Item1 == vehName.GetLocationId()).Where(x => x.Value.Item2 == vehName.GetVehicleId())
                                .Where(x => x.Value.Item3 >= dateFrom).Where(x => x.Value.Item4 <= dateTo).LastOrDefault();
                            if (rentVehTime.Value != null)
                            {
                                vehicleInformation += rentVehTime.Value.Item3 + "     ";
                                vehicleInformation += rentVehTime.Value.Item4 + "     ";
                            }
                            else
                            {
                                vehicleInformation += 0;
                            }

                            vehicleInformation += OutputHelper.FillEmptySpace(vehicleInformation.Length, 40);
                            if (rentVehTime.Value != null)
                            {
                                vehicleInformation += rentVehTime.Value.Item5 + "kn";
                            }
                            else
                            {
                                vehicleInformation += 0;
                            }

                            if (counter != 0)
                            {
                                totalVehicleInformation += Environment.NewLine + OutputHelper.FillEmptySpace(0, 96) + vehicleInformation;
                            }
                            else
                            {
                                totalVehicleInformation += vehicleInformation;
                            }
                            counter++;
                        }

                        locationInformation += Locations[i].GetLocationName();

                        if (counterLocation != 0)
                        {
                            totalLocationInformation += Environment.NewLine + OutputHelper.FillEmptySpace(0, 65) + locationInformation;
                            totalLocationInformation += OutputHelper.FillEmptySpace(locationInformation.Length, 32) + totalVehicleInformation;
                        }
                        else
                        {
                            totalLocationInformation += locationInformation;
                            totalLocationInformation += OutputHelper.FillEmptySpace(locationInformation.Length, 32) + totalVehicleInformation;
                        }

                        counterLocation++;

                    }

                    if (this.GetSuperiorUnit() == null)
                    {
                        superiorUnit = "Nema nadređenu jedinicu";
                    }
                    else
                    {
                        superiorUnit = this.GetSuperiorUnit().CompanyName;
                    }

                    OutputHelper.Counter = 0;
                    IRowTable rowTable =
                        new LocationCapacitiesDecorator(
                            new TextDecorator(
                                  new TextDecorator(
                                         new ConcreteRow())));
                    string format = rowTable.MakeRow();
                    string output = String.Format(format, totalLocationInformation, superiorUnit, this.CompanyName);
                    Console.WriteLine(output);
                    Console.WriteLine(new String('_', 210));
                }
            }
            else if (activityId == 8)
            {
                if (choice == 1)
                {
                    string outputLocation = "";
                    string superiorUnit = "";
                    for (int i = 0; i < Locations.Count; i++)
                    {
                        if (i != 0)
                        {
                            for (int j = 0; j < 64; j++)
                            {
                                outputLocation += " ";
                            }
                        }
                        if (i == Locations.Count - 1)
                            outputLocation += Locations[i].GetLocationName();
                        else
                        {
                            outputLocation += Locations[i].GetLocationName() + Environment.NewLine;

                        }
                    }
                    if (outputLocation == "")
                    {
                        outputLocation = "Nema lokacije";
                    }

                    if (this.GetSuperiorUnit() == null)
                    {
                        superiorUnit = "Nema nadređenu jedinicu";
                    }
                    else
                    {
                        superiorUnit = this.GetSuperiorUnit().CompanyName;
                    }

                    OutputHelper.Counter = 0;
                    IRowTable rowTable =
                        new TextDecorator(
                            new TextDecorator(
                                new TextDecorator(
                                    new ConcreteRow())));
                    string format = rowTable.MakeRow();
                    string output = String.Format(format, outputLocation, superiorUnit, this.CompanyName);
                    Console.WriteLine(output);
                    Console.WriteLine(new String('_', 90));
                }
                else if (choice == 2)
                {
                    VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

                    string totalLocationInformation = "";
                    string vehicleInformation = "";
                    int counterLocation = 0;
                    string superiorUnit = "";

                    for (int i = 0; i < Locations.Count; i++)
                    {
                        string locationInformation = "";

                        var allVehiclesForLocation = vehicle.ListLocationCapacities.Where(x => x.LocationId == Locations[i].Id).ToList();

                        int counter = 0;
                        string totalVehicleInformation = "";

                        foreach (var vehName in allVehiclesForLocation)
                        {
                            vehicleInformation = "";
                            int counterBills = 0;

                            vehicleInformation += vehicle.ListVehicles.Where(x => x.Id == vehName.GetVehicleId()).FirstOrDefault().GetVehicleName();
                            vehicleInformation += OutputHelper.FillEmptySpace(vehicleInformation.Length, 30);

                            var rentVehTime = OutputHelper.rentVehsTime.Where(x => x.Value.Item1 == vehName.GetLocationId()).Where(x => x.Value.Item2 == vehName.GetVehicleId())
                                .Where(x => x.Value.Item3 >= dateFrom).Where(x => x.Value.Item4 <= dateTo).FirstOrDefault();
                            var rentBills = OutputHelper.rentBills.Where(x => x.Value.Item3 == vehName.GetLocationId()).ToList();

                            if (rentVehTime.Value != null)
                            {
                                foreach (var bills in rentBills)
                                {
                                    if (counterBills == 0)
                                    {
                                        vehicleInformation += (bills.Key + 1) + "     ";
                                        vehicleInformation += bills.Value.Item1 + "     ";
                                        vehicleInformation += bills.Value.Item2 + "     ";
                                        vehicleInformation += bills.Value.Item3 + "     ";
                                        vehicleInformation += bills.Value.Item4 + "     ";
                                        vehicleInformation += bills.Value.Item5 + "     ";
                                        vehicleInformation += bills.Value.Item6 + "     ";
                                        vehicleInformation += OutputHelper.FillEmptySpace(0, 131) + bills.Value.Item7 + "     ";
                                        counterBills++;
                                    }
                                    else
                                    {
                                        vehicleInformation += Environment.NewLine + OutputHelper.FillEmptySpace(0, 125);
                                        vehicleInformation += (bills.Key + 1) + "     ";
                                        vehicleInformation += bills.Value.Item1 + "     ";
                                        vehicleInformation += bills.Value.Item2 + "     ";
                                        vehicleInformation += bills.Value.Item3 + "     ";
                                        vehicleInformation += bills.Value.Item4 + "     ";
                                        vehicleInformation += bills.Value.Item5 + "     ";
                                        vehicleInformation += bills.Value.Item6 + "     ";
                                        vehicleInformation += OutputHelper.FillEmptySpace(0, 131) + bills.Value.Item7 + "     ";
                                    }
                                }
                            }
                            else
                            {
                                vehicleInformation += 0;
                            }

                            if (counter != 0)
                            {
                                totalVehicleInformation += Environment.NewLine + OutputHelper.FillEmptySpace(0, 96) + vehicleInformation;
                            }
                            else
                            {
                                totalVehicleInformation += vehicleInformation;
                            }

                            counter++;
                        }

                        locationInformation += Locations[i].GetLocationName();

                        if (counterLocation != 0)
                        {
                            totalLocationInformation += Environment.NewLine + OutputHelper.FillEmptySpace(0, 65) + locationInformation;
                            totalLocationInformation += OutputHelper.FillEmptySpace(locationInformation.Length, 32) + totalVehicleInformation;
                        }
                        else
                        {
                            totalLocationInformation += locationInformation;
                            totalLocationInformation += OutputHelper.FillEmptySpace(locationInformation.Length, 32) + totalVehicleInformation;
                        }

                        counterLocation++;

                    }

                    if (this.GetSuperiorUnit() == null)
                    {
                        superiorUnit = "Nema nadređenu jedinicu";
                    }
                    else
                    {
                        superiorUnit = this.GetSuperiorUnit().CompanyName;
                    }

                    OutputHelper.Counter = 0;
                    IRowTable rowTable =
                        new LocationCapacitiesDecorator(
                            new TextDecorator(
                                    new TextDecorator(
                                            new ConcreteRow())));
                    string format = rowTable.MakeRow();
                    string output = String.Format(format, totalLocationInformation, superiorUnit, this.CompanyName);
                    Console.WriteLine(output);
                    Console.WriteLine(new String('_', 210));
                }
            }
        }

        public void ShowAllAndContinueChilds(int choice, DateTime dateFrom, DateTime dateTo, int activityId, string[] args)
        {
            ShowData(choice, dateFrom, dateTo, activityId, args);
            for (ICompositeIterator iter = GetIterator(); iter.HasNext();)
            {
                IComponentCompany item = (IComponentCompany)iter.Next();
                item.ShowAllAndContinueChilds(choice, dateFrom, dateTo, activityId, args);
            }
        }
        public ICompositeIterator GetIterator()
        {
            return new CompaniesIterator(mChildList);
        }

        public IComponentCompany FindCompany(int companyId)
        {
            for (ICompositeIterator iter = GetIterator(); iter.HasNext();)
            {
                IComponentCompany item = (IComponentCompany)iter.Next();
                if (item.GetMyId() == companyId)
                    return item;

                var childSearch = item.FindCompany(companyId);
                if (childSearch != null)
                    return childSearch;
            }
            return null;
        }

        public void AddLocations(Location location)
        {
            Locations.Add(location);
        }

        public List<Location> GetLocations()
        {
            return Locations;
        }

        public List<Location> GetMyLocations()
        {
            return GetLocations();
        }
    }
}
