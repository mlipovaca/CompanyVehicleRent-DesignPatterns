using mlipovaca_zadaca_3.ChainofResponsibility;
using mlipovaca_zadaca_3.Composite;
using mlipovaca_zadaca_3.Decorator;
using mlipovaca_zadaca_3.State;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3
{
    static class OutputHelper
    {
        private static string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        private static string dateTimeTableFormat = "dd.MM.yyyy";
        public static List<Activity> tempListActivity = new List<Activity>();
        public static VehicleState vehState;
        public static Dictionary<int, Tuple<int, int, int>> defectiveVehs = new Dictionary<int, Tuple<int, int, int>>();
        public static Dictionary<int, Tuple<DateTime, int, int, int, int>> rentVehs = new Dictionary<int, Tuple<DateTime, int, int, int, int>>();
        public static Dictionary<int, Tuple<int, int, DateTime, DateTime, float>> rentVehsTime = new Dictionary<int, Tuple<int, int, DateTime, DateTime, float>>();
        public static Dictionary<int, Tuple<DateTime, string, int, string, int, string, string>> rentBills = new Dictionary<int, Tuple<DateTime, string, int, string, int, string, string>>();
        public static Dictionary<int, Tuple<DateTime, string, int, string, int, string, string>> rentAllBills = new Dictionary<int, Tuple<DateTime, string, int, string, int, string, string>>();
        public static int countDefectiveVehicle = 0;
        public static int countDefectiveVehicleIndex = 0;
        public static int counterRentVeh = 0;
        public static int counterRentVehTime = 0;
        public static int counterRentBills = 0;
        public static double debtMax = 0;
        public static double personDebt = 0;
        public static int counterVeh = 1;
        public static int Counter;

        public static void MainMenu(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n>>> MAIN MENU <<<");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("1) Pregled raspoloživih vozila odabrane vrste na odabranoj lokaciji");
                Console.WriteLine("2) Najam odabrane vrste vozila na odabranoj lokaciji");
                Console.WriteLine("3) Pregled raspoloživih mjesta odabrane vrste vozila za odabranu lokaciju");
                Console.WriteLine("4) Vraćanje vozila na odabranu lokaciju uz unos ukupnog broj kilometara te ispis računa");
                Console.WriteLine("4) Vraćanje neispravnog vozila na odabranu lokaciju uz unos ukupnog broj kilometara te ispis računa");
                Console.WriteLine("5) Prijelaz iz interaktivnog u skupni način");
                Console.WriteLine("6) Ispis podataka o strukturi i stanju tvrtke/org jedinice");
                Console.WriteLine("7) Ispis podataka o najmu i zaradi tvrtke/org jedinice");
                Console.WriteLine("8) Ispis podataka o računima tvrtke/org jedinice");
                Console.WriteLine("9) Ispis financijskog stanja korisnika koji su imali najam vozila");
                Console.WriteLine("10) Ispis podataka o računima korisnika");
                Console.WriteLine("11) Plaćanje dugovanja korisnika");
                Console.WriteLine("0) Kraj programa");

                Console.Write("\nOdabir: ");
                string choose = Console.ReadLine();

                if (int.TryParse(choose, out int choice))
                {
                    if (choice == 1 || choice == 2 || choice == 3 || choice == 4 || choice == 0)
                    {
                        ChooseDatetime(choice, args); 
                    }
                    else if (choice == 5)
                    {
                        OutputDataForActivityChangedFile();
                    }
                    else if (choice == 6)
                    {
                        ChooseStructureStatusCompany(choice, args);
                    }
                    else if (choice == 7)
                    {
                        ChooseStructureRentProfit(choice, args);
                    }
                    else if (choice == 8)
                    {
                        ChooseStructureBills(choice, args);
                    }
                    else if (choice == 9)
                    {
                        ChooseStatusBills(choice, args);
                    }
                    else if (choice == 10)
                    {
                        Activity customActivity = new Activity();
                        customActivity.SetId(10);
                        ChoosePerson(args, customActivity);
                    }
                    else if (choice == 11)
                    {
                        Activity customActivity = new Activity();
                        customActivity.SetId(11);
                        ChoosePerson(args, customActivity);
                    }
                    else
                    {
                        Console.WriteLine("\nOdabrana aktivnost ne postoji!");
                    }
                }
                else
                {
                    Console.WriteLine("Neispravan unos!");
                }
            }

            Console.WriteLine("\nExit program...");
            Console.ReadLine();
            Environment.Exit(0);
        }

        public static void MainMenuWithS(string[] args)
        {
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

            Console.WriteLine();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-d")
                {
                    debtMax = double.Parse(args[i + 1]);
                }
            }

            foreach (var activity in vehicle.ListActivity)
            {
                OutputDataForActivity(args, activity);
            }

            if (vehicle.ListActivity.Any(x => x.GetId() == 0))
            {
                MainMenu(args);
            }

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-i")
                {
                    FileHelper.steamWriter.Close();
                }
            }

            Console.ReadLine();
            Environment.Exit(0);
        }

        public static void OutputDataForActivityChangedFile()
        {
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

            Console.WriteLine();

            string[] args1 = new[] { "DZ_3_aktivnosti.txt", "-s" };

            FileHelper.LoadFileActivityChange(args1);
            foreach (var activity in vehicle.ListActivity)
            {
                OutputDataForActivity(args1, activity);
            }

            Console.ReadLine();
            Environment.Exit(0);
        }

        private static void OutputDataForActivity(string[] args, Activity activity)
        {
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();
            string date = activity.GetDatetime() != null ? activity.GetDatetime() : activity.GetCustomDatetime().ToString();

            if (activity.Id == 1)
            {
                var person = vehicle.ListPersons.Where(x => x.Id == activity.GetPersonId()).ToList()[0];
                var location = vehicle.ListLocations.Where(x => x.Id == activity.GetLocationId()).ToList()[0];
                var vehicleType = vehicle.ListVehicles.Where(x => x.Id == activity.GetVehicleId()).ToList()[0];
                var price = vehicle.ListPriceList.Where(x => x.VehicleId == activity.GetVehicleId()).ToList()[0];

                if (args.Contains("-s"))
                {
                    Console.WriteLine(activity.Id + "; " + date + "; " + activity.GetPersonId() + "; " + activity.GetLocationId() + "; " + activity.GetVehicleId());
                }
                Console.WriteLine("U " + date + " korisnik " + person.GetFirstLastName() + " traži na lokaciji " + location.GetLocationName() +
                        " broj raspoloživih " + vehicleType.GetVehicleName() + ".");

                vehState = new VehicleState(new AvailableVehicle());
                vehState.OutputAvailable("Slobodno");
            }
            else if (activity.Id == 2)
            {
                var person = vehicle.ListPersons.Where(x => x.Id == activity.GetPersonId()).ToList()[0];
                var location = vehicle.ListLocations.Where(x => x.Id == activity.GetLocationId()).ToList()[0];
                var vehicleType = vehicle.ListVehicles.Where(x => x.Id == activity.GetVehicleId()).ToList()[0];
                var price = vehicle.ListPriceList.Where(x => x.VehicleId == activity.GetVehicleId()).ToList()[0];

                if (args.Contains("-s"))
                {
                    Console.WriteLine(activity.Id + "; " + date + "; " + activity.GetPersonId() + "; " + activity.GetLocationId() + "; " + activity.GetVehicleId());
                }
                Console.WriteLine("U " + activity.GetDatetime() + " korisnik " + person.GetFirstLastName() + " traži na lokaciji " + location.GetLocationName() +
                    " najam " + vehicleType.GetVehicleName() + ".");

                var rentVeh = rentVehs.Where(x => x.Value.Item2 == activity.GetLocationId()).Where(x => x.Value.Item3 == activity.GetVehicleId()).FirstOrDefault();

                if (args.Contains("-s"))
                {
                    if (rentVeh.Value != null)
                    {
                        rentVehs.Add(counterRentVeh, new Tuple<DateTime, int, int, int, int>(DateTime.Parse(activity.GetDatetime().TrimStart('„').TrimEnd('“').TrimStart('"').TrimEnd('"')), activity.GetLocationId(), activity.GetVehicleId(), activity.GetPersonId(), counterVeh));
                        counterRentVeh++;
                        counterVeh++;
                    }
                    else
                    {
                        counterVeh = 1;
                        rentVehs.Add(counterRentVeh, new Tuple<DateTime, int, int, int, int>(DateTime.Parse(activity.GetDatetime().TrimStart('„').TrimEnd('“').TrimStart('"').TrimEnd('"')), activity.GetLocationId(), activity.GetVehicleId(), activity.GetPersonId(), counterVeh));
                        counterRentVeh++;
                        counterVeh++;
                    }

                    vehState = new VehicleState(new AvailableVehicle());
                    vehState.Request();
                    vehState.OutputAvailable("Iznajmljeno");
                }
                else
                {
                    if (rentVeh.Value != null)
                    {
                        rentVehs.Add(counterRentVeh, new Tuple<DateTime, int, int, int, int>(DateTime.Parse(activity.GetDatetime().TrimStart('„').TrimEnd('“').TrimStart('"').TrimEnd('"')), activity.GetLocationId(), activity.GetVehicleId(), activity.GetPersonId(), counterVeh));
                        counterRentVeh++;
                        counterVeh++;
                    }
                    else
                    {
                        counterVeh = 1;
                        rentVehs.Add(counterRentVeh, new Tuple<DateTime, int, int, int, int>(DateTime.Parse(activity.GetDatetime().TrimStart('„').TrimEnd('“').TrimStart('"').TrimEnd('"')), activity.GetLocationId(), activity.GetVehicleId(), activity.GetPersonId(), counterVeh));
                        counterRentVeh++;
                        counterVeh++;
                    }

                    vehState = new VehicleState(new AvailableVehicle());
                    vehState.Request();
                    vehState.OutputAvailable("Iznajmljeno");
                }
            }
            else if (activity.Id == 3)
            {
                var person = vehicle.ListPersons.Where(x => x.Id == activity.GetPersonId()).ToList()[0];
                var location = vehicle.ListLocations.Where(x => x.Id == activity.GetLocationId()).ToList()[0];
                var vehicleType = vehicle.ListVehicles.Where(x => x.Id == activity.GetVehicleId()).ToList()[0];
                var price = vehicle.ListPriceList.Where(x => x.VehicleId == activity.GetVehicleId()).ToList()[0];

                if (args.Contains("-s"))
                {
                    Console.WriteLine(activity.Id + "; " + activity.GetDatetime() + "; " + activity.GetPersonId() + "; " + activity.GetLocationId() + "; " + activity.GetVehicleId());
                }
                Console.WriteLine("U " + activity.GetDatetime() + " korisnik " + person.GetFirstLastName() + " traži na lokaciji " + location.GetLocationName() +
                        " broj raspoloživih mjesta za " + vehicleType.GetVehicleName() + ".");

                vehState = new VehicleState(new AvailableVehicle());
                vehState.OutputAvailable("Slobodno");
            }
            else if (activity.Id == 4 && (activity.GetDescriptionProblem() == null || activity.GetDescriptionProblem() == ""))
            {
                var person = vehicle.ListPersons.Where(x => x.Id == activity.GetPersonId()).ToList()[0];
                var location = vehicle.ListLocations.Where(x => x.Id == activity.GetLocationId()).ToList()[0];
                var vehicleType = vehicle.ListVehicles.Where(x => x.Id == activity.GetVehicleId()).ToList()[0];
                var price = vehicle.ListPriceList.Where(x => x.VehicleId == activity.GetVehicleId()).ToList()[0];

                if (args.Contains("-s"))
                {
                    Console.WriteLine(activity.Id + "; " + activity.GetDatetime() + "; " + activity.GetPersonId() + "; " + activity.GetLocationId() + "; " + activity.GetVehicleId() + "; " + activity.GetNumberKilometers());
                }

                if (args.Contains("-s"))
                {
                    var dateRent = vehicle.ListActivity.Where(x => x.PersonId == activity.GetPersonId()).Where(x => x.VehicleId == activity.GetVehicleId()).Where(x => x.Id == 2).FirstOrDefault();

                    if (dateRent != null)
                    {
                        DateTime virtualDatetime = DateTime.Parse(dateRent.GetDatetime().TrimStart('"').TrimEnd('"').TrimStart('„').TrimEnd('“'));
                        DateTime activityDatetime = DateTime.Parse(date.TrimStart('"').TrimEnd('"').TrimStart('„').TrimEnd('“'));
                        float rentHour = (float)(activityDatetime - virtualDatetime).TotalHours;

                        if (rentHour >= 0)
                        {
                            
                            Console.WriteLine("U " + activity.GetDatetime() + " korisnik " + person.GetFirstLastName() + " na lokaciji " + location.GetLocationName() +
                                " vraća unajmljeni " + vehicleType.GetVehicleName() + " koji ima ukupno " + activity.GetNumberKilometers() + " km. Stavke računa su: " +
                                " 1 najam " + vehicleType.GetVehicleName() + " - " + float.Parse(price.GetRent()) + " kn, najam je bio " + ((int)rentHour) + " sata - " +
                                ((int)rentHour) + " * " + float.Parse(price.GetByHour()) + " kn = " + ((int)rentHour) * float.Parse(price.GetByHour()) + " kn, prethodno stanje bilo je" +
                                " 0 km znači da je prošao " + activity.GetNumberKilometers() + " km - " + activity.GetNumberKilometers() + " * " + float.Parse(price.GetByHour()) + " kn "
                                + " = " + activity.GetNumberKilometers() * float.Parse(price.GetByHour()) + " kn. Račun" + " ukupno iznosi " + float.Parse(price.GetRent()) + " kn + " +
                                ((int)rentHour) * float.Parse(price.GetByHour()) + " kn + " + activity.GetNumberKilometers() * float.Parse(price.GetByHour()) + " kn = " + (float.Parse(price.GetRent()) +
                                (((int)rentHour) * float.Parse(price.GetByHour())) + (activity.GetNumberKilometers()) * float.Parse(price.GetByHour())) + " kn.");
                        }

                        float companyUnitProfit = (float.Parse(price.GetRent()) + (((int)rentHour) * float.Parse(price.GetByHour())) + (activity.GetNumberKilometers()) * float.Parse(price.GetByHour()));
                        var vehRented = rentVehs.Where(x => x.Value.Item3 == activity.GetVehicleId()).FirstOrDefault();
                        var veh = rentVehsTime.Where(x => x.Value.Item1 == vehRented.Value.Item2).Where(x => x.Value.Item2 == activity.GetVehicleId()).FirstOrDefault();
                        var locationNameRent = vehicle.ListLocations.Where(x => x.Id == vehRented.Value.Item2).ToList()[0];
                        
                        rentBills.Add(counterRentBills, new Tuple<DateTime, string, int, string, int, string, string>(activityDatetime, person.GetFirstLastName(), vehRented.Value.Item2, locationNameRent.GetLocationName(),
                                activity.GetLocationId(), location.GetLocationName(), "Račun" + " ukupno iznosi " + float.Parse(price.GetRent()) + " kn + " +
                                ((int)rentHour) * float.Parse(price.GetByHour()) + " kn + " + activity.GetNumberKilometers() * float.Parse(price.GetByHour()) + " kn = " + (float.Parse(price.GetRent()) +
                                (((int)rentHour) * float.Parse(price.GetByHour())) + (activity.GetNumberKilometers()) * float.Parse(price.GetByHour())) + " kn."));
                        rentAllBills.Add(counterRentBills, new Tuple<DateTime, string, int, string, int, string, string>(activityDatetime, person.GetFirstLastName(), vehRented.Value.Item2, locationNameRent.GetLocationName(),
                                activity.GetLocationId(), location.GetLocationName(), (float.Parse(price.GetRent()) + (((int)rentHour) * float.Parse(price.GetByHour())) + (activity.GetNumberKilometers()) * float.Parse(price.GetByHour())) + " kn"));
                        counterRentBills++;

                        if (veh.Value == null)
                        {
                            rentVehsTime.Add(counterRentVehTime, new Tuple<int, int, DateTime, DateTime, float>(vehRented.Value.Item2, activity.GetVehicleId(), virtualDatetime, activityDatetime, companyUnitProfit));
                            counterRentVehTime++;
                            vehState.Request();
                            vehState.OutputAvailable("Na punjenju");
                        }
                        else
                        {
                            var par = rentVehsTime.Where(x => x.Value.Item1 == vehRented.Value.Item2).Where(x => x.Value.Item2 == activity.GetVehicleId()).FirstOrDefault();
                            int locationId = par.Value.Item1;
                            int vehicleId = par.Value.Item2;
                            DateTime date1 = new DateTime(virtualDatetime.Ticks);
                            DateTime date2 = new DateTime(activityDatetime.Ticks);
                            float profit = par.Value.Item5 + companyUnitProfit;
                            rentVehsTime.Remove(par.Key);
                            rentVehsTime.Add(counterRentVehTime, new Tuple<int, int, DateTime, DateTime, float>(locationId, vehicleId, date1, date2, profit));
                            counterRentVehTime++;
                            vehState.Request();
                            vehState.OutputAvailable("Na punjenju");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Ovo vozilo " + vehicleType.GetVehicleName() + " nije iznajmljeno od strane korisnika " + person.GetFirstLastName() + "!");
                    }
                }
                else
                {
                    var dateRent = tempListActivity.Where(x => x.PersonId == activity.GetPersonId()).Where(x => x.VehicleId == activity.GetVehicleId()).Where(x => x.Id == 2).FirstOrDefault();
                    DateTime virtualDatetime;
                    if (dateRent != null)
                    {
                        virtualDatetime = DateTime.Parse(dateRent.GetDatetime().TrimStart('"').TrimEnd('"').TrimStart('„').TrimEnd('“'));
                    }
                    else
                    {
                        virtualDatetime = new DateTime(0);
                    }
                    DateTime activityDatetime = DateTime.Parse(date.TrimStart('"').TrimEnd('"').TrimStart('„').TrimEnd('“'));
                    float rentHour = (float)(activityDatetime - virtualDatetime).TotalHours;

                    if (rentHour >= 0)
                    {
                        Console.WriteLine("U " + activity.GetDatetime() + " korisnik " + person.GetFirstLastName() + " na lokaciji " + location.GetLocationName() +
                            " vraća unajmljeni " + vehicleType.GetVehicleName() + " koji ima ukupno " + activity.GetNumberKilometers() + " km. Stavke računa su: " +
                            " 1 najam " + vehicleType.GetVehicleName() + " - " + float.Parse(price.GetRent()) + " kn, najam je bio " + ((int)rentHour) + " sata - " +
                            ((int)rentHour) + " * " + float.Parse(price.GetByHour()) + " kn = " + ((int)rentHour) * float.Parse(price.GetByHour()) + " kn, prethodno stanje bilo je" +
                            " 0 km znači da je prošao " + activity.GetNumberKilometers() + " km - " + activity.GetNumberKilometers() + " * " + float.Parse(price.GetByHour()) + " kn "
                            + " = " + activity.GetNumberKilometers() * float.Parse(price.GetByHour()) + " kn. Račun" + " ukupno iznosi " + float.Parse(price.GetRent()) + " kn + " +
                            ((int)rentHour) * float.Parse(price.GetByHour()) + " kn + " + activity.GetNumberKilometers() * float.Parse(price.GetByHour()) + " kn = " + (float.Parse(price.GetRent()) +
                            (((int)rentHour) * float.Parse(price.GetByHour())) + (activity.GetNumberKilometers()) * float.Parse(price.GetByHour())) + " kn.");
                    }

                    float companyUnitProfit = (float.Parse(price.GetRent()) + (((int)rentHour) * float.Parse(price.GetByHour())) + (activity.GetNumberKilometers()) * float.Parse(price.GetByHour()));
                    var vehRented = rentVehs.Where(x => x.Value.Item3 == activity.GetVehicleId()).FirstOrDefault();
                    var veh = rentVehsTime.Where(x => x.Value.Item1 == vehRented.Value.Item2).Where(x => x.Value.Item2 == activity.GetVehicleId()).FirstOrDefault();
                    var locationNameRent = vehicle.ListLocations.Where(x => x.Id == vehRented.Value.Item2).ToList()[0];

                    rentBills.Add(counterRentBills, new Tuple<DateTime, string, int, string, int, string, string>(activityDatetime, person.GetFirstLastName(), vehRented.Value.Item2, locationNameRent.GetLocationName(),
                            activity.GetLocationId(), location.GetLocationName(), "Račun" + " ukupno iznosi " + float.Parse(price.GetRent()) + " kn + " +
                            ((int)rentHour) * float.Parse(price.GetByHour()) + " kn + " + activity.GetNumberKilometers() * float.Parse(price.GetByHour()) + " kn = " + (float.Parse(price.GetRent()) +
                            (((int)rentHour) * float.Parse(price.GetByHour())) + (activity.GetNumberKilometers()) * float.Parse(price.GetByHour())) + " kn."));
                    rentAllBills.Add(counterRentBills, new Tuple<DateTime, string, int, string, int, string, string>(activityDatetime, person.GetFirstLastName(), vehRented.Value.Item2, locationNameRent.GetLocationName(),
                            activity.GetLocationId(), location.GetLocationName(), (float.Parse(price.GetRent()) + (((int)rentHour) * float.Parse(price.GetByHour())) + (activity.GetNumberKilometers()) * float.Parse(price.GetByHour())) + " kn"));
                    counterRentBills++;

                    if (veh.Value == null)
                    {
                        rentVehsTime.Add(counterRentVehTime, new Tuple<int, int, DateTime, DateTime, float>(vehRented.Value.Item2, activity.GetVehicleId(), virtualDatetime, activityDatetime, companyUnitProfit));
                        counterRentVehTime++;
                        vehState.Request();
                        vehState.OutputAvailable("Na punjenju");
                    }
                    else
                    {
                        var par = rentVehsTime.Where(x => x.Value.Item1 == vehRented.Value.Item2).Where(x => x.Value.Item2 == activity.GetVehicleId()).FirstOrDefault();
                        int locationId = par.Value.Item1;
                        int vehicleId = par.Value.Item2;
                        DateTime date1 = new DateTime(virtualDatetime.Ticks);
                        DateTime date2 = new DateTime(activityDatetime.Ticks);
                        float profit = par.Value.Item5 + companyUnitProfit;
                        rentVehsTime.Remove(par.Key);
                        rentVehsTime.Add(counterRentVehTime, new Tuple<int, int, DateTime, DateTime, float>(locationId, vehicleId, date1, date2, profit));
                        counterRentVehTime++;
                        vehState.Request();
                        vehState.OutputAvailable("Na punjenju");
                    }
                }
            }
            else if (activity.Id == 4 && (activity.GetDescriptionProblem() != null || activity.GetDescriptionProblem() != ""))
            {
                var person = vehicle.ListPersons.Where(x => x.Id == activity.GetPersonId()).ToList()[0];
                var location = vehicle.ListLocations.Where(x => x.Id == activity.GetLocationId()).ToList()[0];
                var vehicleType = vehicle.ListVehicles.Where(x => x.Id == activity.GetVehicleId()).ToList()[0];
                var price = vehicle.ListPriceList.Where(x => x.VehicleId == activity.GetVehicleId()).ToList()[0];

                if (args.Contains("-s"))
                {
                    Console.WriteLine(activity.Id + "; " + activity.GetDatetime() + "; " + activity.GetPersonId() + "; " + activity.GetLocationId() + "; " + activity.GetVehicleId() + "; " + activity.GetNumberKilometers() + "; " + activity.GetDescriptionProblem());
                }

                if (args.Contains("-s"))
                {
                    var dateRent = vehicle.ListActivity.Where(x => x.PersonId == activity.GetPersonId()).Where(x => x.VehicleId == activity.GetVehicleId()).Where(x => x.Id == 2).FirstOrDefault();
                    if (dateRent != null)
                    {
                        DateTime virtualDatetime = DateTime.Parse(dateRent.GetDatetime().TrimStart('"').TrimEnd('"').TrimStart('„').TrimEnd('“'));
                        DateTime activityDatetime = DateTime.Parse(date.TrimStart('"').TrimEnd('"').TrimStart('„').TrimEnd('“'));
                        float rentHour = (float)(activityDatetime - virtualDatetime).TotalHours;

                        if (rentHour >= 0)
                        {
                            countDefectiveVehicle++;

                            Console.WriteLine("Broj vraćanja vozila u neispravnom stanju: " + countDefectiveVehicle);

                            Console.WriteLine("U " + activity.GetDatetime() + " korisnik " + person.GetFirstLastName() + " na lokaciji " + location.GetLocationName() +
                                    " vraća unajmljeni " + vehicleType.GetVehicleName() + " koji ima ukupno " + activity.GetNumberKilometers() + " km te prijavljuje da vozilo ima problem '" +
                                    activity.GetDescriptionProblem() + "'. Stavke računa su:  1 najam " + vehicleType.GetVehicleName() + " - " + float.Parse(price.GetRent()) + " kn, najam je bio " +
                                    ((int)rentHour) + " sata - " + ((int)rentHour) + " * " + float.Parse(price.GetByHour()) + " kn = " + ((int)rentHour) * float.Parse(price.GetByHour()) +
                                    " kn, prethodno stanje bilo je 0 km znači da je prošao " + activity.GetNumberKilometers() + " km - " + activity.GetNumberKilometers() + " * " + float.Parse(price.GetByHour()) + " kn "
                                    + " = " + activity.GetNumberKilometers() * float.Parse(price.GetByHour()) + " kn. Račun" + " ukupno iznosi " + float.Parse(price.GetRent()) + " kn + " + ((int)rentHour) * float.Parse(price.GetByHour())
                                    + " kn + " + activity.GetNumberKilometers() * float.Parse(price.GetByHour()) + " kn = " + (float.Parse(price.GetRent()) + (((int)rentHour) * float.Parse(price.GetByHour())) + (activity.GetNumberKilometers())
                                    * float.Parse(price.GetByHour())) + " kn.");

                            float companyUnitProfit = (float.Parse(price.GetRent()) + (((int)rentHour) * float.Parse(price.GetByHour())) + (activity.GetNumberKilometers()) * float.Parse(price.GetByHour()));
                            var defVeh = defectiveVehs.Where(x => x.Value.Item1 == activity.GetLocationId()).Where(x => x.Value.Item2 == activity.GetVehicleId()).FirstOrDefault();
                            var vehRented = rentVehs.Where(x => x.Value.Item3 == activity.GetVehicleId()).Where(x => x.Value.Item4 == activity.GetPersonId()).FirstOrDefault();
                            var locationNameRent = vehicle.ListLocations.Where(x => x.Id == vehRented.Value.Item2).ToList()[0]; ;

                            rentBills.Add(counterRentBills, new Tuple<DateTime, string, int, string, int, string, string>(activityDatetime, person.GetFirstLastName(), vehRented.Value.Item2, locationNameRent.GetLocationName(),
                                activity.GetLocationId(), location.GetLocationName(), "Račun" + " ukupno iznosi " + float.Parse(price.GetRent()) + " kn + " +
                                ((int)rentHour) * float.Parse(price.GetByHour()) + " kn + " + activity.GetNumberKilometers() * float.Parse(price.GetByHour()) + " kn = " + (float.Parse(price.GetRent()) +
                                (((int)rentHour) * float.Parse(price.GetByHour())) + (activity.GetNumberKilometers()) * float.Parse(price.GetByHour())) + " kn."));
                            rentAllBills.Add(counterRentBills, new Tuple<DateTime, string, int, string, int, string, string>(activityDatetime, person.GetFirstLastName(), vehRented.Value.Item2, locationNameRent.GetLocationName(),
                            activity.GetLocationId(), location.GetLocationName(), (float.Parse(price.GetRent()) + (((int)rentHour) * float.Parse(price.GetByHour())) + (activity.GetNumberKilometers()) * float.Parse(price.GetByHour())) + " kn"));
                            counterRentBills++;

                            if (defVeh.Value == null)
                            {
                                defectiveVehs.Add(countDefectiveVehicleIndex, new Tuple<int, int, int>(activity.GetLocationId(), activity.GetVehicleId(), 1));
                                countDefectiveVehicleIndex++;
                                vehState.Request();
                                vehState.Request();
                                vehState.OutputAvailable("Neispravno vozilo");
                            }
                            else
                            {
                                var par = defectiveVehs.Where(x => x.Value.Item1 == activity.GetLocationId()).Where(x => x.Value.Item2 == activity.GetVehicleId()).FirstOrDefault();
                                int locationId = par.Value.Item1;
                                int vehicleId = par.Value.Item2;
                                int counter = par.Value.Item3 + 1;
                                defectiveVehs.Remove(par.Key);
                                defectiveVehs.Add(countDefectiveVehicleIndex, new Tuple<int, int, int>(locationId, vehicleId, counter));
                                vehState.Request();
                                vehState.Request();
                                vehState.OutputAvailable("Neispravno vozilo");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Ovo vozilo " + vehicleType.GetVehicleName() + " nije iznajmljeno od strane korisnika " + person.GetFirstLastName() + "!");
                    }
                }
                else
                {
                    var dateRent = tempListActivity.Where(x => x.PersonId == activity.GetPersonId()).Where(x => x.VehicleId == activity.GetVehicleId()).Where(x => x.Id == 2).FirstOrDefault();
                    DateTime virtualDatetime;
                    if (dateRent != null)
                    {
                        virtualDatetime = DateTime.Parse(dateRent.GetDatetime().TrimStart('"').TrimEnd('"').TrimStart('„').TrimEnd('“'));
                    }
                    else
                    {
                        virtualDatetime = new DateTime(0);
                    }
                    DateTime activityDatetime = DateTime.Parse(date.TrimStart('"').TrimEnd('"').TrimStart('„').TrimEnd('“'));
                    float rentHour = (float)(activityDatetime - virtualDatetime).TotalHours;

                    if (rentHour >= 0)
                    {
                        countDefectiveVehicle++;

                        Console.WriteLine("Broj vraćanja vozila u neispravnom stanju: " + countDefectiveVehicle);

                        Console.WriteLine("U " + activity.GetDatetime() + " korisnik " + person.GetFirstLastName() + " na lokaciji " + location.GetLocationName() +
                                " vraća unajmljeni " + vehicleType.GetVehicleName() + " koji ima ukupno " + activity.GetNumberKilometers() + " km te prijavljuje da vozilo ima problem '" +
                                activity.GetDescriptionProblem() + "'. Stavke računa su:  1 najam " + vehicleType.GetVehicleName() + " - " + float.Parse(price.GetRent()) + " kn, najam je bio " +
                                ((int)rentHour) + " sata - " + ((int)rentHour) + " * " + float.Parse(price.GetByHour()) + " kn = " + ((int)rentHour) * float.Parse(price.GetByHour()) +
                                " kn, prethodno stanje bilo je 0 km znači da je prošao " + activity.GetNumberKilometers() + " km - " + activity.GetNumberKilometers() + " * " + float.Parse(price.GetByHour()) + " kn "
                                + " = " + activity.GetNumberKilometers() * float.Parse(price.GetByHour()) + " kn. Račun" + " ukupno iznosi " + float.Parse(price.GetRent()) + " kn + " + ((int)rentHour) * float.Parse(price.GetByHour())
                                + " kn + " + activity.GetNumberKilometers() * float.Parse(price.GetByHour()) + " kn = " + (float.Parse(price.GetRent()) + (((int)rentHour) * float.Parse(price.GetByHour())) + (activity.GetNumberKilometers())
                                * float.Parse(price.GetByHour())) + " kn.");

                        float companyUnitProfit = (float.Parse(price.GetRent()) + (((int)rentHour) * float.Parse(price.GetByHour())) + (activity.GetNumberKilometers()) * float.Parse(price.GetByHour()));
                        var defVeh = defectiveVehs.Where(x => x.Value.Item1 == activity.GetLocationId()).Where(x => x.Value.Item3 == activity.GetVehicleId()).FirstOrDefault();
                        var vehRented = rentVehs.Where(x => x.Value.Item3 == activity.GetVehicleId()).Where(x => x.Value.Item4 == activity.GetPersonId()).FirstOrDefault();
                        var locationNameRent = vehicle.ListLocations.Where(x => x.Id == vehRented.Value.Item2).ToList()[0]; ;

                        rentBills.Add(counterRentBills, new Tuple<DateTime, string, int, string, int, string, string>(activityDatetime, person.GetFirstLastName(), vehRented.Value.Item2, locationNameRent.GetLocationName(),
                            activity.GetLocationId(), location.GetLocationName(), "Račun" + " ukupno iznosi " + float.Parse(price.GetRent()) + " kn + " +
                            ((int)rentHour) * float.Parse(price.GetByHour()) + " kn + " + activity.GetNumberKilometers() * float.Parse(price.GetByHour()) + " kn = " + (float.Parse(price.GetRent()) +
                            (((int)rentHour) * float.Parse(price.GetByHour())) + (activity.GetNumberKilometers()) * float.Parse(price.GetByHour())) + " kn."));
                        rentAllBills.Add(counterRentBills, new Tuple<DateTime, string, int, string, int, string, string>(activityDatetime, person.GetFirstLastName(), vehRented.Value.Item2, locationNameRent.GetLocationName(),
                            activity.GetLocationId(), location.GetLocationName(), (float.Parse(price.GetRent()) + (((int)rentHour) * float.Parse(price.GetByHour())) + (activity.GetNumberKilometers()) * float.Parse(price.GetByHour())) + " kn"));
                        counterRentBills++;

                        if (defVeh.Value == null)
                        {
                            defectiveVehs.Add(countDefectiveVehicleIndex, new Tuple<int, int, int>(activity.GetLocationId(), activity.GetVehicleId(), 1));
                            countDefectiveVehicleIndex++;
                            vehState.Request();
                            vehState.Request();
                            vehState.OutputAvailable("Neispravno vozilo");
                        }
                        else
                        {
                            var par = defectiveVehs.Where(x => x.Value.Item1 == activity.GetLocationId()).Where(x => x.Value.Item2 == activity.GetVehicleId()).FirstOrDefault();
                            int locationId = par.Value.Item1;
                            int vehicleId = par.Value.Item2;
                            int counter = par.Value.Item3 + 1;
                            defectiveVehs.Remove(par.Key);
                            defectiveVehs.Add(countDefectiveVehicleIndex, new Tuple<int, int, int>(locationId, vehicleId, counter));
                            vehState.Request();
                            vehState.Request();
                            vehState.OutputAvailable("Neispravno vozilo");
                        }

                        var veh = rentVehsTime.Where(x => x.Value.Item1 == activity.GetLocationId()).Where(x => x.Value.Item2 == activity.GetVehicleId()).FirstOrDefault();
                        if (veh.Value == null)
                        {
                            rentVehsTime.Add(counterRentVehTime, new Tuple<int, int, DateTime, DateTime, float>(activity.GetLocationId(), activity.GetVehicleId(), virtualDatetime, activityDatetime, companyUnitProfit));
                            counterRentVehTime++;
                        }
                        else
                        {
                            var par = rentVehsTime.Where(x => x.Value.Item1 == activity.GetLocationId()).Where(x => x.Value.Item2 == activity.GetVehicleId()).FirstOrDefault();
                            int locationId = par.Value.Item1;
                            int vehicleId = par.Value.Item2;
                            DateTime date1 = new DateTime(virtualDatetime.Ticks);
                            DateTime date2 = new DateTime(activityDatetime.Ticks);
                            float profit = par.Value.Item5 + companyUnitProfit;
                            rentVehsTime.Remove(par.Key);
                            rentVehsTime.Add(counterRentVehTime, new Tuple<int, int, DateTime, DateTime, float>(locationId, vehicleId, date1, date2, profit));
                        }
                    }
                }
            }
            else if (activity.Id == 6)
            {
                string[] trimActivity = activity.CustomActivity.Split(' ');

                if (args.Contains("-s"))
                {
                    Console.WriteLine(activity.Id + "; " + activity.GetCustomActivity());
                }

                if (trimActivity.Contains("struktura") && trimActivity.Contains("stanje"))
                {
                    if (trimActivity.Length == 2)
                    {
                        DisplayHeaderOfStructureAndStatusData(210);
                        vehicle.GetCompositeCompany().ShowAllAndContinueChilds(2, DateTime.Now, DateTime.Now, 6, args);
                    }
                    else
                    {
                        var company = vehicle.GetCompositeCompany().FindCompany(int.Parse(trimActivity[2]));
                        if (company != null)
                        {
                            DisplayHeaderOfStructureAndStatusData(210);
                            company.ShowData(2, DateTime.Now, DateTime.Now, 6, args);
                        }
                        else
                        {
                            Console.WriteLine("ERROR: Nije moguće prikazati tablicu Strukture i stanja za kompaniju pod ID: " + trimActivity[2]);
                        }
                    }
                }
                else
                {
                    if (trimActivity.Length == 1)
                    {
                        DisplayHeaderOfStructureData(90);
                        vehicle.GetCompositeCompany().ShowAllAndContinueChilds(1, DateTime.Now, DateTime.Now, 6, args);
                    }
                    else
                    {
                        var company = vehicle.GetCompositeCompany().FindCompany(int.Parse(trimActivity[1]));
                        if (company != null)
                        {
                            DisplayHeaderOfStructureData(90);
                            company.ShowData(1, DateTime.Now, DateTime.Now, 6, args);
                        }
                        else
                        {
                            Console.WriteLine("ERROR: Nije moguće prikazati tablicu Strukture i stanja za kompaniju pod ID: " + trimActivity[1]);
                        }
                    }
                }
            }
            else if (activity.Id == 7)
            {
                string[] trimActivity = activity.CustomActivity.Split(' ');

                if (args.Contains("-s"))
                {
                    Console.WriteLine(activity.Id + "; " + activity.GetCustomActivity());
                }

                if (trimActivity.Contains("struktura") && trimActivity.Contains("najam") && trimActivity.Contains("zarada"))
                {
                    if (trimActivity.Length == 5)
                    {
                        DisplayHeaderOfStructureRentAndProfitData(210);
                        vehicle.GetCompositeCompany().ShowAllAndContinueChilds(3, DateTime.Parse(trimActivity[3]), DateTime.Parse(trimActivity[4]), 7, args);
                    }
                    else if (trimActivity.Length == 6)
                    {
                        var company = vehicle.GetCompositeCompany().FindCompany(int.Parse(trimActivity[5]));
                        if (company != null)
                        {
                            DisplayHeaderOfStructureRentAndProfitData(210);
                            company.ShowData(3, DateTime.Parse(trimActivity[3]), DateTime.Parse(trimActivity[4]), 7, args);
                        }
                        else
                        {
                            Console.WriteLine("ERROR: Nije moguće prikazati tablicu Strukture, najma i zarade za kompaniju pod ID: " + trimActivity[5]);
                        }
                    }
                }
                else if (trimActivity.Contains("struktura") && trimActivity.Contains("najam") && !trimActivity.Contains("zarada"))
                {
                    if (trimActivity.Length == 4)
                    {
                        DisplayHeaderOfStructureAndRentData(210);
                        vehicle.GetCompositeCompany().ShowAllAndContinueChilds(2, DateTime.Parse(trimActivity[2]), DateTime.Parse(trimActivity[3]), 7, args);
                    }
                    else if (trimActivity.Length == 5)
                    {
                        var company = vehicle.GetCompositeCompany().FindCompany(int.Parse(trimActivity[4]));
                        if (company != null)
                        {
                            DisplayHeaderOfStructureAndRentData(210);
                            company.ShowData(2, DateTime.Parse(trimActivity[2]), DateTime.Parse(trimActivity[3]), 7, args);
                        }
                        else
                        {
                            Console.WriteLine("ERROR: Nije moguće prikazati tablicu Strukture, najma i zarade za kompaniju pod ID: " + trimActivity[4]);
                        }
                    }
                }
                else
                {
                    if (trimActivity.Length == 3)
                    {
                        DisplayHeaderOfStructureData(90);
                        vehicle.GetCompositeCompany().ShowAllAndContinueChilds(1, DateTime.Now, DateTime.Now, 7, args);
                    }
                    else
                    {
                        var company = vehicle.GetCompositeCompany().FindCompany(int.Parse(trimActivity[3]));
                        if (company != null)
                        {
                            DisplayHeaderOfStructureData(90);
                            company.ShowData(1, DateTime.Now, DateTime.Now, 7, args);
                        }
                        else
                        {
                            Console.WriteLine("ERROR: Nije moguće prikazati tablicu Strukture i stanja za kompaniju pod ID: " + trimActivity[3]);
                        }
                    }
                }
            }
            else if (activity.Id == 8)
            {
                string[] trimActivity = activity.CustomActivity.Split(' ');

                if (args.Contains("-s"))
                {
                    Console.WriteLine(activity.Id + "; " + activity.GetCustomActivity());
                }

                if (trimActivity.Contains("struktura") && trimActivity.Contains("računi"))
                {
                    if (trimActivity.Length == 4)
                    {
                        DisplayHeaderOfStructureAndBillsData(210);
                        vehicle.GetCompositeCompany().ShowAllAndContinueChilds(2, DateTime.Parse(trimActivity[2]), DateTime.Parse(trimActivity[3]), 8, args);
                    }
                    else
                    {
                        var company = vehicle.GetCompositeCompany().FindCompany(int.Parse(trimActivity[4]));
                        if (company != null)
                        {
                            DisplayHeaderOfStructureAndBillsData(210);
                            company.ShowData(2, DateTime.Parse(trimActivity[2]), DateTime.Parse(trimActivity[3]), 8, args);
                        }
                        else
                        {
                            Console.WriteLine("ERROR: Nije moguće prikazati tablicu Strukture i stanja za kompaniju pod ID: " + trimActivity[4]);
                        }
                    }
                }
                else
                {
                    if (trimActivity.Length == 3)
                    {
                        DisplayHeaderOfStructureData(90);
                        vehicle.GetCompositeCompany().ShowAllAndContinueChilds(1, DateTime.Now, DateTime.Now, 8, args);
                    }
                    else
                    {
                        var company = vehicle.GetCompositeCompany().FindCompany(int.Parse(trimActivity[3]));
                        if (company != null)
                        {
                            DisplayHeaderOfStructureData(90);
                            company.ShowData(1, DateTime.Now, DateTime.Now, 8, args);
                        }
                        else
                        {
                            Console.WriteLine("ERROR: Nije moguće prikazati tablicu Strukture i stanja za kompaniju pod ID: " + trimActivity[3]);
                        }
                    }
                }
            }
            else if (activity.Id == 9)
            {
                Person person = new Person();

                if (args.Contains("-s"))
                {
                    Console.WriteLine(activity.Id);
                }

                DisplayHeaderOfStatusAndBillsData(120);
                person.ShowData(9);
            }
            else if (activity.Id == 10)
            {
                Person person = new Person();

                if (args.Contains("-s"))
                {
                    Console.WriteLine(activity.Id + "; " + activity.GetCustomActivity());
                }

                DisplayHeaderOfStatusPersonsBillsData(180);

                if (args.Contains("-s"))
                {
                    string[] trimActivity = activity.CustomActivity.Split(' ');

                    BillStatHandler.OutputStatisticTable(DateTime.Parse(trimActivity[1]), DateTime.Parse(trimActivity[2]), int.Parse(trimActivity[0]));
                }
            }
            else if (activity.Id == 11)
            {
                int billId;
                double billPrice;
                double payableAmount = 0;

                if (args.Contains("-s"))
                {
                    Console.WriteLine(activity.Id + "; " + activity.GetCustomActivity());
                }

                if (args.Contains("-s"))
                {
                    string[] trimActivity = activity.CustomActivity.Split(' ');
                    var person = vehicle.ListPersons.Where(x => x.Id == int.Parse(trimActivity[0])).ToList()[0];
                    var billPersonList = rentBills.Where(x => x.Value.Item2 == person.FirstLastName).ToList();


                    if (person.Contract != 0)
                    {
                        foreach (var bills in billPersonList)
                        {
                            string[] splitBill = bills.Value.Item7.Split(' ');
                            personDebt += double.Parse(splitBill[12]);
                        }
                    }
                    else
                    {
                        personDebt = 0;
                    }

                    if (personDebt != 0)
                    {
                        Console.Write("Korisnik " + person.GetFirstLastName() + " je platio/la " + double.Parse(trimActivity[1]) + " kn za svoje dugovanje.");
                        Console.Write(" Podmireni su ");

                        var billList = rentBills.Where(x => x.Value.Item2 == person.GetFirstLastName()).ToList();

                        for (int i = 0; i < billPersonList.Count; i++)
                        {
                            billId = billPersonList[i].Key + 1;
                            string[] splitBill = billPersonList[i].Value.Item7.Split(' ');
                            billPrice = double.Parse(splitBill[12]);
                            if (double.Parse(trimActivity[1]) > billPrice)
                            {
                                if (i == 0)
                                {
                                    Console.Write("račun " + billId + " s iznosom od " + billPrice + " kn");
                                    for (int j = 0; j < billList.Count; j++)
                                    {
                                        if (billList[j].Key == billPersonList[i].Key)
                                            rentBills.Remove(j);
                                    }
                                }
                                else
                                {
                                    Console.Write(" i račun " + billId + " s iznosom od " + billPrice + " kn");
                                    for (int j = 0; j < billList.Count; j++)
                                    {
                                        if (billList[j].Key == billPersonList[i].Key)
                                            rentBills.Remove(j);
                                    }
                                }
                                payableAmount += billPrice;
                            }
                            else
                            {
                                Console.Write(". Račun " + billId + " nije podmiren jer ima iznos " + billPrice + " kn");
                            }

                            if (i + 1 == billPersonList.Count)
                            {
                                double billPrice1 = double.Parse(splitBill[3]);
                                Console.Write(". Nakon podmirenja računa 1 ostalo je " + (double.Parse(trimActivity[1]) - billPrice1) + " kn za ostale račune,");
                                for (int j = 0; j < billPersonList.Count; j++)
                                {
                                    int billId1 = billPersonList[j].Key + 1;
                                    string[] splitBill1 = billPersonList[j].Value.Item7.Split(' ');
                                    billPrice1 = double.Parse(splitBill1[12]);
                                    if (double.Parse(trimActivity[1]) <= billPrice1)
                                    {
                                        int counterPrice = 0;
                                        if (counterPrice == 0)
                                        {
                                            Console.Write(" a to nije dovoljno za račun " + billId1);
                                            counterPrice++;
                                        }
                                        else
                                            Console.Write(" i račun " + billId1);
                                    }
                                }
                            }
                        }

                        Console.WriteLine(". Korisniku je vraćeno " + (double.Parse(trimActivity[1]) - payableAmount) + " kn.");
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Nema nikakvih dugova za platiti");
                    }
                }
                else
                {
                    var person = vehicle.ListPersons.Where(x => x.Id == activity.GetPersonId()).ToList()[0];
                    var billPersonList = rentBills.Where(x => x.Value.Item2 == person.FirstLastName).ToList();

                    if (person.Contract != 0)
                    {
                        foreach (var bills in billPersonList)
                        {
                            string[] splitBill = bills.Value.Item7.Split(' ');
                            personDebt += double.Parse(splitBill[12]);
                        }
                    }
                    else
                    {
                        personDebt = 0;
                    }

                    if (personDebt != 0)
                    {
                        Console.Write("Korisnik " + person.GetFirstLastName() + " je platio/la " + double.Parse(activity.GetCustomActivity()) + " kn za svoje dugovanje.");
                        Console.Write(" Podmireni su ");

                        var billList = rentBills.Where(x => x.Value.Item2 == person.GetFirstLastName()).ToList();

                        for (int i = 0; i < billPersonList.Count; i++)
                        {
                            billId = billPersonList[i].Key + 1;
                            string[] splitBill = billPersonList[i].Value.Item7.Split(' ');
                            billPrice = double.Parse(splitBill[12]);
                            if (double.Parse(activity.GetCustomActivity()) > billPrice)
                            {
                                if (i == 0)
                                {
                                    Console.Write("račun " + billId + " s iznosom od " + billPrice + " kn");
                                    for (int j = 0; j < billList.Count; j++)
                                    {
                                        if (billList[j].Key == billPersonList[i].Key)
                                            rentBills.Remove(j);
                                    }
                                }
                                else
                                {
                                    Console.Write(" i račun " + billId + " s iznosom od " + billPrice + " kn");
                                    for (int j = 0; j < billList.Count; j++)
                                    {
                                        if (billList[j].Key == billPersonList[i].Key)
                                            rentBills.Remove(j);
                                    }
                                }
                                payableAmount += billPrice;
                            }
                            else
                            {
                                Console.Write(". Račun " + billId + " nije podmiren jer ima iznos " + billPrice + " kn");
                            }

                            if (i + 1 == billPersonList.Count)
                            {
                                double billPrice1 = double.Parse(splitBill[3]);
                                Console.Write(". Nakon podmirenja računa 1 ostalo je " + (double.Parse(activity.GetCustomActivity()) - billPrice1) + " kn za ostale račune,");
                                for (int j = 0; j < billPersonList.Count; j++)
                                {
                                    int billId1 = billPersonList[j].Key + 1;
                                    string[] splitBill1 = billPersonList[j].Value.Item7.Split(' ');
                                    billPrice1 = double.Parse(splitBill1[12]);
                                    if (double.Parse(activity.GetCustomActivity()) <= billPrice1)
                                    {
                                        int counterPrice = 0;
                                        if (counterPrice == 0)
                                        {
                                            Console.Write(" a to nije dovoljno za račun " + billId1);
                                            counterPrice++;
                                        }
                                        else
                                            Console.Write(" i račun " + billId1);
                                    }
                                }
                            }
                        }

                        Console.WriteLine(". Korisniku je vraćeno " + (double.Parse(activity.GetCustomActivity()) - payableAmount) + " kn.");
                    }
                    else
                    {
                        Console.WriteLine("Nemate nikakvih dugova za platiti!");
                    }
                }
            }
            else if (activity.Id == 0)
            {
                if (args.Contains("-s"))
                {
                    Console.WriteLine(activity.Id + "; " + activity.GetDatetime());
                }
                Console.WriteLine("\nU " + activity.GetDatetime() + " program završava s radom.");
            }

            Console.Write(Environment.NewLine);
        }

        public static void ChooseDatetime(int activityId, string[] args)
        {
            Activity customActivity = new Activity();
            customActivity.SetId(activityId);
            //Odabir datuma i vremena
            Console.Write("Unesite datum i vrijeme (\"yyyy-MM-dd HH:MM:SS\"): ");
            string insertDateStringWithQuotes = Console.ReadLine();
            string insertDateString = insertDateStringWithQuotes.TrimStart('"').TrimEnd('"').TrimStart('„').TrimEnd('“');

            DateTime? insertDate = DateTime.TryParseExact(insertDateString, dateTimeFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime datetime)
                ? datetime
                : (DateTime?)null;

            bool dateCheck = FileHelper.CheckValueVirtualTimeDatetime(args, insertDateString);

            if (datetime != DateTime.MinValue)
            {
                if (dateCheck)
                {
                    //Ako je upisano ispravno vrijeme
                    customActivity.SetDatetime(insertDateStringWithQuotes);
                    if (customActivity.GetId() != 0)
                    {
                        ChoosePerson(args, customActivity);
                    }
                    else
                    {
                        OutputDataForActivity(args, customActivity);
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Console.WriteLine("\nUneseno vrijeme je manje (u prošlosti) nego virtualno vrijeme!");
                }
            }
            else
            {
                Console.WriteLine("\nNiste unijeli vrijeme u ispravnom formatu");
            }
        }

        public static void ChoosePerson(string[] args, Activity customActivity)
        {
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

            Console.WriteLine("\nOdaberite osobu: \n");
            for (int i = 0; i < vehicle.ListPersons.Count; i++)
            {
                Console.WriteLine("(" + (i + 1) + ") " + vehicle.ListPersons[i].GetFirstLastName());
            }

            if (customActivity.GetId() == 10)
            {
                Console.Write("\nMoj odabir (prazno za sve osobe): ");
            }
            else
            {
                Console.Write("\nMoj odabir: ");
            }

            string userInsert = Console.ReadLine();
            if (int.TryParse(userInsert, out int choice))
            {
                if (choice > vehicle.ListPersons.Count || choice <= 0)
                {
                    Console.WriteLine("\nTa osoba ne postoji !");
                }
                else
                {
                    customActivity.SetPersonId(choice);
                    if (customActivity.GetId() == 11)
                    {
                        ChooseAmount(args, customActivity);
                    }
                    else if (customActivity.GetId() == 10)
                    {
                        ChooseTableDatetime(choice, customActivity.GetId(), args);
                    }
                    else
                    {
                        ChooseLocation(args, customActivity);
                    }
                }
            }
            else
            {
                Console.WriteLine("\nNe ispravan unos!");
            }
        }

        public static void ChooseLocation(string[] args, Activity customActivity)
        {
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

            Console.WriteLine("\nOdaberite lokaciju: \n");
            for (int i = 0; i < vehicle.ListLocations.Count; i++)
            {
                Console.WriteLine("(" + (i + 1) + ") " + vehicle.ListLocations[i].GetLocationName());
            }
            Console.Write("\nMoj odabir: ");
            string locationInsert = Console.ReadLine();
            if (int.TryParse(locationInsert, out int choice))
            {
                if (choice > vehicle.ListLocations.Count || choice <= 0)
                {
                    Console.WriteLine("\nTa lokacija ne postoji !");
                }
                else
                {
                    customActivity.SetLocationId(choice);
                    ChooseVehicleType(args, customActivity);
                }
            }
            else
            {
                Console.WriteLine("\nNe ispravan unos!");
            }
        }

        public static void ChooseAmount(string[] args, Activity customActivity)
        {
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

            Console.Write("Odaberite iznos plaćanja: ");
            string amount = Console.ReadLine();

            if (double.TryParse(amount, out double choice))
            {
                if (customActivity.GetId() == 11)
                {
                    customActivity.SetCustomActivitiy(amount);
                    Console.WriteLine("\n");
                    OutputDataForActivity(args, customActivity);
                }
            }
            else
            {
                Console.WriteLine("\nNe ispravan unos!");
            }
        }

        public static void ChooseVehicleType(string[] args, Activity customActivity)
        {
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-d")
                {
                    debtMax = double.Parse(args[i + 1]);
                }
            }

            Console.WriteLine("\nOdaberite vozilo: \n");

            List<LocationCapacity> listAvailableVehicles = new List<LocationCapacity>();
            listAvailableVehicles = vehicle.ListLocationCapacities.Where(x => x.GetLocationId() == customActivity.GetLocationId()).Where(x => x.GetAvailableVehicles() > 0).ToList();

            for (int i = 0; i < listAvailableVehicles.Count; i++)
            {
                Console.WriteLine("(" + (listAvailableVehicles[i].GetVehicleId()) + ") " + vehicle.ListVehicles[listAvailableVehicles[i].GetVehicleId() - 1].GetVehicleName());
            }
            Console.Write("\nMoj odabir: ");
            string vehicleInsert = Console.ReadLine();
            if (int.TryParse(vehicleInsert, out int choice))
            {
                var found = listAvailableVehicles.Find(x => x.GetVehicleId() == choice);

                if (found == null)
                {
                    Console.WriteLine("\nTo vozilo ne postoji !");
                }
                else
                {
                    var getAvailableVeh = listAvailableVehicles.Where(x => x.GetVehicleId() == choice).Where(x => x.GetLocationId() == customActivity.GetLocationId()).ToList()[0];
                    customActivity.SetVehicleId(choice);

                    bool vehFound = false;

                    foreach (var vehRent in tempListActivity)
                    {
                        if (vehRent.GetPersonId() == customActivity.GetPersonId() && vehRent.GetVehicleId() == choice)
                        {
                            vehFound = true;
                        }
                    }

                    if (customActivity.GetId() == 1)
                    {
                        Console.WriteLine("\nBroj raspoloživih vozila: " + getAvailableVeh.GetAvailableVehicles() + "\n");
                        OutputDataForActivity(args, customActivity);
                    }
                    else if (customActivity.GetId() == 2)
                    {
                        var personId = vehicle.ListPersons.Where(x => x.Id == customActivity.PersonId).ToList()[0];
                        var billPersonList = rentBills.Where(x => x.Value.Item2 == personId.FirstLastName).ToList();

                        if (personId.Contract != 0)
                        {
                            foreach (var bills in billPersonList)
                            {
                                string[] splitBill = bills.Value.Item7.Split(' ');
                                personDebt += double.Parse(splitBill[12]);
                            }
                        }
                        else
                        {
                            personDebt = 0;
                        }

                        if (!vehFound && !defectiveVehs.Keys.Contains(choice) && personDebt < debtMax)
                        {
                            tempListActivity.Add(customActivity);
                            Console.WriteLine();
                            OutputDataForActivity(args, customActivity);
                        }
                        else if (defectiveVehs.Keys.Contains(choice))
                        {
                            Console.WriteLine("\nTrenutno nije moguće iznajmiti to vozilo - neispravno vozilo!");
                        }
                        else if (personDebt >= debtMax)
                        {
                            Console.WriteLine("\nNemožete iznajmiti više vozila --- prekoračili ste dugovanje!");
                        }
                        else
                        {
                            Console.WriteLine("\nVeć ste unajmili istu vrstu vozila!");
                        }
                    }
                    else if (customActivity.GetId() == 3)
                    {
                        Console.WriteLine("\nBroj raspoloživih mjesta vozila: " + getAvailableVeh.GetSeat() + "\n");
                        OutputDataForActivity(args, customActivity);
                    }
                    else if (customActivity.GetId() == 4)
                    {
                        if (vehFound)
                        {
                            ChooseDescriptionProblem(customActivity);
                            OutputDataForActivity(args, customActivity);
                            var activityId = tempListActivity.Where(x => x.GetVehicleId() == customActivity.GetVehicleId()).Where(x => x.GetPersonId() == customActivity.GetPersonId()).ToList()[0];
                            tempListActivity.Remove(activityId);
                        }
                        else
                        {
                            Console.WriteLine("\nNiste iznajmili uneseno vozilo!");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Ne ispravan unos!");
            }
        }

        public static void ChooseKilometers(string[] args, Activity customActivity)
        {
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

            var range = vehicle.ListVehicles.Where(x => x.GetId() == customActivity.GetVehicleId()).ToList()[0];

            Console.Write("Unesite broj kilometara: ");
            string insertKilometers = Console.ReadLine();

            if(int.TryParse(insertKilometers, out int kilometers))
            {
                if (kilometers <= 0)
                {
                    Console.WriteLine("\nBroj kilometara mora biti pozitivna cijela vrijednost, veća od 0!");
                }
                else if (kilometers > range.GetRange())
                {
                    Console.WriteLine("\nUnijeli ste broj kilometara veći od mogućeg dometa!");
                }
                else
                {
                    customActivity.SetNumberKilometers(kilometers);
                    OutputDataForActivity(args, customActivity);
                }
            }
            else
            {
                Console.WriteLine("\nNiste unijeli ispravnu brojčanu vrijednost");
            }
        }

        public static void ChooseDescriptionProblem(Activity customActivity)
        {
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

            Console.Write("Unesite opis problema vozila (ostavite prazno ukoliko nema problem): ");
            string insertDescription = Console.ReadLine();

            customActivity.SetDescriptionProblem(insertDescription);
            Console.WriteLine();
        }

        public static void ChooseStructureStatusCompany(int activityId, string[] args)
        {
            Console.WriteLine("\n1) Struktura tvrtke");
            Console.WriteLine("2) Struktura i stanje tvrtke");

            Console.Write("\nUnesite: ");
            string insertChoice = Console.ReadLine();

            if (int.TryParse(insertChoice, out int choice))
            {
                if (choice == 1 || choice == 2)
                {
                    ChooseUnit(choice, activityId, args);
                }
                else
                {
                    Console.WriteLine("\nUnijeli ste krivi odabir!");
                }
            }
        }

        public static void ChooseStructureRentProfit(int activityId, string[] args)
        {
            Console.WriteLine("\n1) Struktura tvrtke");
            Console.WriteLine("2) Struktura i najam tvrtke");
            Console.WriteLine("3) Struktura, najam i zarada tvrtke");

            Console.Write("\nUnesite: ");
            string insertChoice = Console.ReadLine();
            
            if (int.TryParse(insertChoice, out int choice))
            {
                if (choice == 1 || choice == 2 || choice == 3)
                {
                    ChooseTableDatetime(choice, activityId, args);
                }
                else
                {
                    Console.WriteLine("\nUnijeli ste krivi odabir!");
                }
            }
        }

        public static void ChooseStructureBills(int activityId, string[] args)
        {
            Console.WriteLine("\n1) Struktura tvrtke");
            Console.WriteLine("2) Struktura i računi tvrtke");

            Console.Write("\nUnesite: ");
            string insertChoice = Console.ReadLine();

            if (int.TryParse(insertChoice, out int choice))
            {
                if (choice == 1 || choice == 2)
                {
                    ChooseTableDatetime(choice, activityId, args);
                }
                else
                {
                    Console.WriteLine("\nUnijeli ste krivi odabir!");
                }
            }
        }

        public static void ChooseStatusBills(int activityId, string[] args)
        {
            DisplayStatusAndBillsData(activityId, args);
        }

        public static void DisplayHeaderOfStructureData(int length)
        {
            Console.WriteLine("\n");
            Counter = 0;
            IRowTable rowTable =
                new TextDecorator(
                    new TextDecorator(
                        new TextDecorator(
                            new ConcreteRow())));
            string format = rowTable.MakeRow();
            string output = String.Format(format, "LOKACIJE", "NADREĐENA JEDINICA", "NAZIV JEDINICE");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(output);
            Console.WriteLine(new String('_', length));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void DisplayHeaderOfStructureAndStatusData(int length)
        {
            Console.WriteLine("\n");
            Counter = 0;
            IRowTable rowTable =
                new TextDecorator(
                    new TextDecorator(
                        new TextDecorator(
                            new TextDecorator(
                                new TextDecorator(
                                    new TextDecorator(
                                        new TextDecorator(
                                            new ConcreteRow())))))));
            string format = rowTable.MakeRow();
            string output = String.Format(format, "BROJ NEISPRAVNIH VOZILA", "BROJ RASPOLOŽIVIH VOZILA", "BROJ MJESTA", "VRSTA VOZILA", "LOKACIJE", "NADREĐENA JEDINICA", "NAZIV JEDINICE");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(output);
            Console.WriteLine(new String('_', length));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void DisplayHeaderOfStructureAndRentData(int length)
        {
            Console.WriteLine("\n");
            Counter = 0;
            IRowTable rowTable =
                new TextDecorator(
                    new TextDecorator(
                        new TextDecorator(
                            new TextDecorator(
                                new TextDecorator(
                                    new TextDecorator(
                                            new ConcreteRow()))))));
            string format = rowTable.MakeRow();
            string output = String.Format(format, "TRAJANJE NAJMA (OD - DO)", "BROJ NAJMA", "VRSTA VOZILA", "LOKACIJE", "NADREĐENA JEDINICA", "NAZIV JEDINICE");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(output);
            Console.WriteLine(new String('_', length));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void DisplayHeaderOfStructureRentAndProfitData(int length)
        {
            Console.WriteLine("\n");
            Counter = 0;
            IRowTable rowTable =
                new TextDecorator(
                    new TextDecorator(
                        new TextDecorator(
                            new TextDecorator(
                                new TextDecorator(
                                    new TextDecorator(
                                        new TextDecorator(
                                            new ConcreteRow())))))));
            string format = rowTable.MakeRow();
            string output = String.Format(format, "ZARADA", "TRAJANJE NAJMA (OD - DO)", "BROJ NAJMA", "VRSTA VOZILA", "LOKACIJE", "NADREĐENA JEDINICA", "NAZIV JEDINICE");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(output);
            Console.WriteLine(new String('_', length));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void DisplayHeaderOfStructureAndBillsData(int length)
        {
            Console.WriteLine("\n");
            Counter = 0;
            IRowTable rowTable =
                new TextDecorator(
                    new TextDecorator(
                        new TextDecorator(
                            new TextDecorator(
                                new TextDecorator(
                                            new ConcreteRow())))));
            string format = rowTable.MakeRow();
            string output = String.Format(format, "RAČUNI", "VRSTA VOZILA", "LOKACIJE", "NADREĐENA JEDINICA", "NAZIV JEDINICE");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(output);
            Console.WriteLine(new String('_', length));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void DisplayHeaderOfStatusAndBillsData(int length)
        {
            Console.WriteLine("\n");
            Counter = 0;
            IRowTable rowTable =
                new TextDecorator(
                    new TextDecorator(
                        new TextDecorator(
                            new TextDecorator(
                                new ConcreteRow()))));
            string format = rowTable.MakeRow();
            string output = String.Format(format, "DATUM I VRIJEME", "FINANCIJSKO STANJE", "IME I PREZIME", "ID KORISNIKA");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(output);
            Console.WriteLine(new String('_', length));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void DisplayHeaderOfStatusPersonsBillsData(int length)
        {
            Console.WriteLine("\n");
            Counter = 0;
            IRowTable rowTable =
                new TextDecorator(
                    new TextDecorator(
                        new TextDecorator(
                            new TextDecorator(
                                new TextDecorator(
                                    new TextDecorator(
                                        new ConcreteRow()))))));
            string format = rowTable.MakeRow();
            string output = String.Format(format, "LOKACIJA NAJMA", "VRSTA VOZILA", "STATUS", "DATUM I VRIJEME", "IZNOS", "BROJ RAČUNA");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(output);
            Console.WriteLine(new String('_', length));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ChooseUnit(int choice, int activityId, string[] args)
        {
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

            Console.WriteLine("\nOdaberite organizacijsku jedinicu: \n");

            for (int i = 0; i < vehicle.ListCompanies.Count; i++)
            {
                Console.WriteLine("(" + (i + 1) + ") " + vehicle.ListCompanies[i].GetMyName());
            }
            Console.Write("\nOdaberite (prazno za cijelu tvrtku): ");
            string insertUnit = Console.ReadLine();

            if (int.TryParse(insertUnit, out int choiceUnit))
            {
                if (choiceUnit > vehicle.ListCompanies.Count || choiceUnit <= 0)
                {
                    Console.WriteLine("\nTa tvrtka ne postoji !");
                }
                else
                {
                    if (activityId == 6)
                    {
                        if (choice == 1)
                            DisplayStructureData(insertUnit, choice, activityId, args);
                        else if (choice == 2)
                            DisplayStructureAndStatusData(insertUnit, choice, DateTime.Now, DateTime.Now, activityId, args);
                    }
                }
            }
            else if (insertUnit == "")
            {
                if (activityId == 6)
                {
                    if (choice == 1)
                        DisplayStructureData(insertUnit, choice, activityId, args);
                    else if (choice == 2)
                        DisplayStructureAndStatusData(insertUnit, choice, DateTime.Now, DateTime.Now, activityId, args);
                }
            }
            else
            {
                Console.WriteLine("Ne ispravan unos!");
            }
        }

        public static void ChooseTableDatetime(int choice, int activityId, string[] args)
        {
            //Odabir datuma i vremena
            Console.Write("Unesite datum od (\"dd.MM.yyyy\"): ");
            string insertDateFromString = Console.ReadLine();
            Console.Write("Unesite datum do (\"dd.MM.yyyy\"): ");
            string insertDateToString = Console.ReadLine();

            DateTime? insertDateFrom = DateTime.TryParseExact(insertDateFromString, dateTimeTableFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime datetime)
                ? datetime
                : (DateTime?)null;

            DateTime? insertDateTo = DateTime.TryParseExact(insertDateToString, dateTimeTableFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime datetime1)
                ? datetime
                : (DateTime?)null;

            if (datetime != DateTime.MinValue && datetime1 != DateTime.MinValue)
            {
                if (activityId == 10)
                {
                    DisplayStatusPersonsBillsData(choice, datetime, datetime1, activityId, args);
                }
                else
                {
                    ChooseUnitWithDate(choice, datetime, datetime1, activityId, args);
                }
            }
            else
            {
                Console.WriteLine("\nNiste unijeli vrijeme u ispravnom formatu");
            }
        }

        public static void ChooseUnitWithDate(int choice, DateTime dateFrom, DateTime dateTo, int activityId, string[] args)
        {
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

            Console.WriteLine("\nOdaberite organizacijsku jedinicu: \n");

            for (int i = 0; i < vehicle.ListCompanies.Count; i++)
            {
                Console.WriteLine("(" + (i + 1) + ") " + vehicle.ListCompanies[i].GetMyName());
            }
            Console.Write("\nOdaberite (prazno za cijelu tvrtku): ");
            string insertUnit = Console.ReadLine();

            if (int.TryParse(insertUnit, out int choiceUnit))
            {
                if (choiceUnit > vehicle.ListCompanies.Count || choiceUnit <= 0)
                {
                    Console.WriteLine("\nTa tvrtka ne postoji !");
                }
                else
                {
                    if (activityId == 7)
                    {
                        if (choice == 1)
                            DisplayStructureData(insertUnit, choice, activityId, args);
                        else if (choice == 2)
                            DisplayStructureAndRentData(insertUnit, dateFrom, dateTo, choice, activityId, args);
                        else if (choice == 3)
                            DisplayStructureRentAndProfitData(insertUnit, dateFrom, dateTo, choice, activityId, args);
                    }
                    else if (activityId == 8)
                    {
                        if (choice == 1)
                            DisplayStructureData(insertUnit, choice, activityId, args);
                        else if (choice == 2)
                            DisplayStructureAndBillsData(insertUnit, dateFrom, dateTo, choice, activityId, args);
                    }
                }
            }
            else if (insertUnit == "")
            {
                if (activityId == 7)
                {
                    if (choice == 1)
                        DisplayStructureData(insertUnit, choice, activityId, args);
                    else if (choice == 2)
                        DisplayStructureAndRentData(insertUnit, dateFrom, dateTo, choice, activityId, args);
                    else if (choice == 3)
                        DisplayStructureRentAndProfitData(insertUnit, dateFrom, dateTo, choice, activityId, args);
                }
                else if (activityId == 8)
                {
                    if (choice == 1)
                        DisplayStructureData(insertUnit, choice, activityId, args);
                    else if (choice == 2)
                        DisplayStructureAndBillsData(insertUnit, dateFrom, dateTo, choice, activityId, args);
                }
            }
            else
            {
                Console.WriteLine("Ne ispravan unos!");
            }
        }

        public static void DisplayStructureData(string insertUnit, int choice, int activityId, string[] args)
        {
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

            DisplayHeaderOfStructureData(90);

            if (insertUnit.Trim() == "")
            {
                vehicle.GetCompositeCompany().ShowAllAndContinueChilds(choice, DateTime.Now, DateTime.Now, activityId, args);
            }
            else
            {
                var company = vehicle.GetCompositeCompany().FindCompany(int.Parse(insertUnit));
                company.ShowData(choice, DateTime.Now, DateTime.Now, activityId, args);
            }
        }

        public static void DisplayStructureAndStatusData(string insertUnit, int choice, DateTime dateFrom, DateTime dateTo, int activityId, string[] args)
        {
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

            DisplayHeaderOfStructureAndStatusData(210);

            if (insertUnit.Trim() == "")
            {
                vehicle.GetCompositeCompany().ShowAllAndContinueChilds(choice, dateFrom, dateTo, activityId, args);
            }
            else
            {
                var company = vehicle.GetCompositeCompany().FindCompany(int.Parse(insertUnit));
                company.ShowData(choice, dateFrom, dateTo, activityId, args);
            }
        }

        public static void DisplayStructureAndRentData(string insertUnit, DateTime dateFrom, DateTime dateTo, int choice, int activityId, string[] args)
        {
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

            DisplayHeaderOfStructureAndRentData(210);

            if (insertUnit.Trim() == "")
            {
                vehicle.GetCompositeCompany().ShowAllAndContinueChilds(choice, dateFrom, dateTo, activityId, args);
            }
            else
            {
                var company = vehicle.GetCompositeCompany().FindCompany(int.Parse(insertUnit));
                company.ShowData(choice, dateFrom, dateTo, activityId, args);
            }
        }

        public static void DisplayStructureRentAndProfitData(string insertUnit, DateTime dateFrom, DateTime dateTo, int choice, int activityId, string[] args)
        {
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

            DisplayHeaderOfStructureRentAndProfitData(210);

            if (insertUnit.Trim() == "")
            {
                vehicle.GetCompositeCompany().ShowAllAndContinueChilds(choice, dateFrom, dateTo, activityId, args);
            }
            else
            {
                var company = vehicle.GetCompositeCompany().FindCompany(int.Parse(insertUnit));
                company.ShowData(choice, dateFrom, dateTo, activityId, args);
            }
        }

        public static void DisplayStructureAndBillsData(string insertUnit, DateTime dateFrom, DateTime dateTo, int choice, int activityId, string[] args)
        {
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

            DisplayHeaderOfStructureAndBillsData(210);

            if (insertUnit.Trim() == "")
            {
                vehicle.GetCompositeCompany().ShowAllAndContinueChilds(choice, dateFrom, dateTo, activityId, args);
            }
            else
            {
                var company = vehicle.GetCompositeCompany().FindCompany(int.Parse(insertUnit));
                company.ShowData(choice, dateFrom, dateTo, activityId, args);
            }
        }

        public static void DisplayStatusAndBillsData(int activityId, string[] args)
        {
            Person person = new Person();

            DisplayHeaderOfStatusAndBillsData(120);

            person.ShowData(activityId);
        }

        public static void DisplayStatusPersonsBillsData(int choice, DateTime dateFrom, DateTime dateTo, int activityId, string[] args)
        {
            Person person = new Person();

            DisplayHeaderOfStatusPersonsBillsData(180);

            BillStatHandler.OutputStatisticTable(dateFrom, dateTo, choice);
        }

        public static string FillEmptySpace(int informationLength, int totalLength)
        {
            string result = "";

            for (int i = informationLength; i < totalLength - 1; i++)
            {
                result += " ";
            }

            return result;
        }
    }
}
