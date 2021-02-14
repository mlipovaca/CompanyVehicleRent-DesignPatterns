using mlipovaca_zadaca_3.VehicleBuilder;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3
{
    class FileHelper
    {
        public static int vehicleCounter = 0;
        public static string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public static string dateTimeFormatActivity = "dd.MM.yyyy";
        private static char NEW_ATTRIBUTE = ';';
        private static char NEW_LINE = '\n';
        private static string ARGUMENT_REGEX = @"^(([a-zA-Z\d\s-_#\\:.!]{1,250})(\.txt))$";
        public static List<int> tempVehIds = new List<int>();
        public static System.IO.FileStream fileStream;
        public static System.IO.StreamWriter steamWriter;

        public static void CheckInputParameters(string[] args)
        {
            if (CheckInputParametersFunc(args, ARGUMENT_REGEX))
            {
                Console.WriteLine("\n--- Input Parameters su prošli validaciju ---");
                Console.WriteLine("--- ZAPOČNI PROGRAM ---");
                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            }
            else
            {
                Console.WriteLine("\n--- Input Parameters nisu u točnom obliku ---");
                Console.WriteLine("--- IZLAZAK IZ PROGRAMA ---");
                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        public static void CheckInputParametersWithS(string[] args)
        {
            if (CheckInputParametersFuncWithS(args, ARGUMENT_REGEX))
            {
                Console.WriteLine("\n--- Input Parameters su prošli validaciju ---");
                Console.WriteLine("--- ZAPOČNI PROGRAM ---");
                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            }
            else
            {
                Console.WriteLine("\n--- Input Parameters nisu u točnom obliku ---");
                Console.WriteLine("--- IZLAZAK IZ PROGRAMA ---");
                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        public static void CheckInputParametersWithQuotes(string[] args)
        {
            if (CheckInputParametersFuncWithQuotes(args, ARGUMENT_REGEX))
            {
                Console.WriteLine("\n--- Input Parameters su prošli validaciju ---");
                Console.WriteLine("--- ZAPOČNI PROGRAM ---");
                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            }
            else
            {
                Console.WriteLine("\n--- Input Parameters nisu u točnom obliku ---");
                Console.WriteLine("--- IZLAZAK IZ PROGRAMA ---");
                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        public static void CheckInputParametersWithQuotesAndS(string[] args)
        {
            if (CheckInputParametersFuncWithQuotesAndS(args, ARGUMENT_REGEX))
            {
                Console.WriteLine("\n--- Input Parameters su prošli validaciju ---");
                Console.WriteLine("--- ZAPOČNI PROGRAM ---");
                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            }
            else
            {
                Console.WriteLine("\n--- Input Parameters nisu u točnom obliku ---");
                Console.WriteLine("--- IZLAZAK IZ PROGRAMA ---");
                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        public static void CheckConfigurationFile(string[] args)
        {
            string path = CheckPathFileConfiguration(args);
            string configurationFile = "";
            bool containsAcitivity = false;
            bool containsOutput = false;

            try
            {
                configurationFile = System.IO.File.ReadAllText(path);
            }
            catch
            {
                Console.WriteLine("--- Datoteka Konfiguracija nije učitana, provjerite da li se datoteka nalazi na putanji");
                Console.WriteLine("--- IZLAZAK IZ PROGRAMA ---");
                Console.ReadLine();
                Environment.Exit(0);
            }

            string[] seriesLinesVehicle1 = rowsFromFile(configurationFile, NEW_LINE);
            for (int i = 0; i < seriesLinesVehicle1.Length; i++)
            {
                string[] dataConfiguration = seriesLinesVehicle1[i].Split('=');

                if (seriesLinesVehicle1[i].Contains("="))
                {
                    if (dataConfiguration[0] == "aktivnosti")
                    {
                        containsAcitivity = true;
                    }
                    else if (dataConfiguration[0] == "izlaz")
                    {
                        containsOutput = true;
                    }
                }
                else
                {
                    Console.WriteLine("--- Unijeli ste krivi naziv konfiguracijske datoteke za pokretanje programa ---");
                    Console.WriteLine("--- IZLAZAK IZ PROGRAMA ---");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }

            if (containsAcitivity != true && containsOutput != true)
            {
                string[] seriesLinesVehicle = rowsFromFile(configurationFile, NEW_LINE);
                args = new string[22];

                for (int i = 0; i < seriesLinesVehicle.Length; i++)
                {
                    string[] lines = seriesLinesVehicle[i].Split('=');
                    if (lines[0] == "vozila")
                    {
                        args[0] = "-v";
                        args[1] = lines[1].Trim();
                    }
                    else if (lines[0] == "lokacije")
                    {
                        args[2] = "-l";
                        args[3] = lines[1].Trim();
                    }
                    else if (lines[0] == "cjenik")
                    {
                        args[4] = "-c";
                        args[5] = lines[1].Trim();
                    }
                    else if (lines[0] == "kapaciteti")
                    {
                        args[6] = "-k";
                        args[7] = lines[1].Trim();
                    }
                    else if (lines[0] == "osobe")
                    {
                        args[8] = "-o";
                        args[9] = lines[1].Trim();
                    }
                    else if (lines[0] == "vrijeme")
                    {
                        args[10] = "-t";
                        args[11] = lines[1].Trim();
                    }
                    else if (lines[0] == "struktura")
                    {
                        args[12] = "-os";
                        args[13] = lines[1].Trim();
                    }
                    else if (lines[0] == "tekst")
                    {
                        args[14] = "-dt";
                        args[15] = lines[1].Trim();
                    }
                    else if (lines[0] == "cijeli")
                    {
                        args[16] = "-dc";
                        args[17] = lines[1].Trim();
                    }
                    else if (lines[0] == "decimala")
                    {
                        args[18] = "-dd";
                        args[19] = lines[1].Trim();
                    }
                    else if (lines[0] == "dugovanje")
                    {
                        args[20] = "-d";
                        args[21] = lines[1].Trim();
                    }
                }

                CheckInputParameters(args);
                LoadFileVehicles(args);
                LoadFileLocations(args);
                LoadFileLocationCapacity(args);
                LoadFilePersons(args);
                LoadFilePriceList(args);
                LoadFileCompanies(args);
                OutputHelper.MainMenu(args);
            }
            else if (containsAcitivity != true && containsOutput == true)
            {
                string[] seriesLinesVehicle = rowsFromFile(configurationFile, NEW_LINE);
                args = new string[24];

                for (int i = 0; i < seriesLinesVehicle.Length; i++)
                {
                    string[] lines = seriesLinesVehicle[i].Split('=');
                    if (lines[0] == "vozila")
                    {
                        args[0] = "-v";
                        args[1] = lines[1].Trim();
                    }
                    else if (lines[0] == "lokacije")
                    {
                        args[2] = "-l";
                        args[3] = lines[1].Trim();
                    }
                    else if (lines[0] == "cjenik")
                    {
                        args[4] = "-c";
                        args[5] = lines[1].Trim();
                    }
                    else if (lines[0] == "kapaciteti")
                    {
                        args[6] = "-k";
                        args[7] = lines[1].Trim();
                    }
                    else if (lines[0] == "osobe")
                    {
                        args[8] = "-o";
                        args[9] = lines[1].Trim();
                    }
                    else if (lines[0] == "vrijeme")
                    {
                        args[10] = "-t";
                        args[11] = lines[1].Trim();
                    }
                    else if (lines[0] == "struktura")
                    {
                        args[12] = "-os";
                        args[13] = lines[1].Trim();
                    }
                    else if (lines[0] == "tekst")
                    {
                        args[14] = "-dt";
                        args[15] = lines[1].Trim();
                    }
                    else if (lines[0] == "cijeli")
                    {
                        args[16] = "-dc";
                        args[17] = lines[1].Trim();
                    }
                    else if (lines[0] == "decimala")
                    {
                        args[18] = "-dd";
                        args[19] = lines[1].Trim();
                    }
                    else if (lines[0] == "dugovanje")
                    {
                        args[20] = "-d";
                        args[21] = lines[1].Trim();
                    }
                    else if (lines[0] == "izlaz")
                    {
                        args[22] = "-i";
                        args[23] = lines[1].Trim();
                    }
                }

                CheckInputParameters(args);
                LoadFileVehicles(args);
                LoadFileLocations(args);
                LoadFileLocationCapacity(args);
                LoadFilePersons(args);
                LoadFilePriceList(args);
                LoadFileCompanies(args);
                OutputHelper.MainMenu(args);
            }
            else if (containsAcitivity == true && containsOutput == true)
            {
                string[] seriesLinesVehicle = rowsFromFile(configurationFile, NEW_LINE);
                args = new string[26];

                for (int i = 0; i < seriesLinesVehicle.Length; i++)
                {
                    string[] lines = seriesLinesVehicle[i].Split('=');
                    if (lines[0] == "vozila")
                    {
                        args[0] = "-v";
                        args[1] = lines[1].Trim();
                    }
                    else if (lines[0] == "lokacije")
                    {
                        args[2] = "-l";
                        args[3] = lines[1].Trim();
                    }
                    else if (lines[0] == "cjenik")
                    {
                        args[4] = "-c";
                        args[5] = lines[1].Trim();
                    }
                    else if (lines[0] == "kapaciteti")
                    {
                        args[6] = "-k";
                        args[7] = lines[1].Trim();
                    }
                    else if (lines[0] == "osobe")
                    {
                        args[8] = "-o";
                        args[9] = lines[1].Trim();
                    }
                    else if (lines[0] == "vrijeme")
                    {
                        args[10] = "-t";
                        args[11] = lines[1].Trim();
                    }
                    else if (lines[0] == "struktura")
                    {
                        args[12] = "-os";
                        args[13] = lines[1].Trim();
                    }
                    else if (lines[0] == "aktivnosti")
                    {
                        args[14] = "-s";
                        args[15] = lines[1].Trim();
                    }
                    else if (lines[0] == "tekst")
                    {
                        args[16] = "-dt";
                        args[17] = lines[1].Trim();
                    }
                    else if (lines[0] == "cijeli")
                    {
                        args[18] = "-dc";
                        args[19] = lines[1].Trim();
                    }
                    else if (lines[0] == "decimala")
                    {
                        args[20] = "-dd";
                        args[21] = lines[1].Trim();
                    }
                    else if (lines[0] == "dugovanje")
                    {
                        args[22] = "-d";
                        args[23] = lines[1].Trim();
                    }
                    else if (lines[0] == "izlaz")
                    {
                        args[24] = "-i";
                        args[25] = lines[1].Trim();
                    }
                }

                fileStream = new System.IO.FileStream(args[25], System.IO.FileMode.Create);
                steamWriter = new System.IO.StreamWriter(fileStream);
                Console.WriteLine("------- Trenutno se nalazite za ispis u datoteku: " + args[25] + "-------");
                Console.SetOut(steamWriter);

                CheckInputParametersWithS(args);
                LoadFileVehicles(args);
                LoadFileLocations(args);
                LoadFileLocationCapacity(args);
                LoadFilePersons(args);
                LoadFilePriceList(args);
                LoadFileCompanies(args);
                LoadFileActivity(args);
                OutputHelper.MainMenuWithS(args);
            }
            else {
                string[] seriesLinesVehicle = rowsFromFile(configurationFile, NEW_LINE);
                args = new string[24];

                for (int i = 0; i < seriesLinesVehicle.Length; i++)
                {
                    string[] lines = seriesLinesVehicle[i].Split('=');
                    if (lines[0] == "vozila")
                    {
                        args[0] = "-v";
                        args[1] = lines[1].Trim();
                    }
                    else if (lines[0] == "lokacije")
                    {
                        args[2] = "-l";
                        args[3] = lines[1].Trim();
                    }
                    else if (lines[0] == "cjenik")
                    {
                        args[4] = "-c";
                        args[5] = lines[1].Trim();
                    }
                    else if (lines[0] == "kapaciteti")
                    {
                        args[6] = "-k";
                        args[7] = lines[1].Trim();
                    }
                    else if (lines[0] == "osobe")
                    {
                        args[8] = "-o";
                        args[9] = lines[1].Trim();
                    }
                    else if (lines[0] == "vrijeme")
                    {
                        args[10] = "-t";
                        args[11] = lines[1].Trim();
                    }
                    else if (lines[0] == "struktura")
                    {
                        args[12] = "-os";
                        args[13] = lines[1].Trim();
                    }
                    else if (lines[0] == "aktivnosti")
                    {
                        args[14] = "-s";
                        args[15] = lines[1].Trim();
                    }
                    else if (lines[0] == "tekst")
                    {
                        args[16] = "-dt";
                        args[17] = lines[1].Trim();
                    }
                    else if (lines[0] == "cijeli")
                    {
                        args[18] = "-dc";
                        args[19] = lines[1].Trim();
                    }
                    else if (lines[0] == "decimala")
                    {
                        args[20] = "-dd";
                        args[21] = lines[1].Trim();
                    }
                    else if (lines[0] == "dugovanje")
                    {
                        args[22] = "-d";
                        args[23] = lines[1].Trim();
                    }
                }

                CheckInputParametersWithS(args);
                LoadFileVehicles(args);
                LoadFileLocations(args);
                LoadFileLocationCapacity(args);
                LoadFilePersons(args);
                LoadFilePriceList(args);
                LoadFileCompanies(args);
                LoadFileActivity(args);
                OutputHelper.MainMenuWithS(args);
            }
        }

        public static void LoadFileVehicles(string[] args)
        {
            string path = CheckPathFileVehicles(args);
            VehiclesSingleton Vehicle = VehiclesSingleton.GetVehiclesInstance();
            
            Console.WriteLine("\n--- Čitanje datoteke Vozila ---");
            string vehicleFile = "";
            try
            {
                vehicleFile = System.IO.File.ReadAllText(path);
            }
            catch
            {
                Console.WriteLine("--- Datoteka Vozila nije učitana, provjerite da li se datoteka nalazi na putanji " + path + "");
                Console.WriteLine("--- IZLAZAK IZ PROGRAMA ---");
                Console.ReadLine();
                Environment.Exit(0);
            }

            Console.WriteLine("--- Loadanje podataka iz datoteke Vozila! ---");
            string[] seriesLinesVehicle = rowsFromFile(vehicleFile, NEW_LINE);
            Vehicle.ListVehicles = GetCorrectVehicles(args, seriesLinesVehicle, NEW_ATTRIBUTE);
            if (Vehicle.ListVehicles.Count > 0)
            {
                Console.WriteLine("--- Učitavanje podataka datoteke Vozila je završeno. (Ispravnih zapisa: " + Vehicle.ListVehicles.Count + ") ---");
            }
            else
            {
                Console.WriteLine("--- Datoteka Vozila ne sadrži niti jedan ispravan zapis ---");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        public static void LoadFileLocations(string[] args)
        {
            string path = CheckPathFileLocations(args);
            VehiclesSingleton Vehicle = VehiclesSingleton.GetVehiclesInstance();

            Console.WriteLine("\n--- Čitanje datoteke Lokacija ---");
            string locationFile = "";
            try
            {
                locationFile = System.IO.File.ReadAllText(path);
            }
            catch
            {
                Console.WriteLine("--- Datoteka Lokacija nije učitana, provjerite da li se datoteka nalazi na putanji " + path + "");
                Console.WriteLine("--- IZLAZAK IZ PROGRAMA ---");
                Console.ReadLine();
                Environment.Exit(0);
            }

            Console.WriteLine("--- Loadanje podataka iz datoteke Lokacija! ---");
            string[] seriesLinesLocation = rowsFromFile(locationFile, NEW_LINE);
            Vehicle.ListLocations = GetCorrectLocations(args, seriesLinesLocation, NEW_ATTRIBUTE);
            if (Vehicle.ListLocations.Count > 0)
            {
                Console.WriteLine("--- Učitavanje podataka datoteke Lokacija je završeno. (Ispravnih zapisa: " + Vehicle.ListLocations.Count + ") ---");
            }
            else
            {
                Console.WriteLine("--- Datoteka Lokacija ne sadrži niti jedan ispravan zapis ---");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        public static void LoadFileLocationCapacity(string[] args)
        {
            string path = CheckPathFileLocationCapacity(args);
            VehiclesSingleton Vehicle = VehiclesSingleton.GetVehiclesInstance();

            Console.WriteLine("\n--- Čitanje datoteke Lokacije kapaciteti ---");
            string locationCapacityFile = "";
            try
            {
                locationCapacityFile = System.IO.File.ReadAllText(path);
            }
            catch
            {
                Console.WriteLine("--- Datoteka Lokacija kapaciteti nije učitana, provjerite da li se datoteka nalazi na putanji " + path + "");
                Console.WriteLine("--- IZLAZAK IZ PROGRAMA ---");
                Console.ReadLine();
                Environment.Exit(0);
            }

            Console.WriteLine("--- Loadanje podataka iz datoteke Lokacija kapaciteti! ---");
            string[] seriesLinesLocationCapacity = rowsFromFile(locationCapacityFile, NEW_LINE);
            Vehicle.ListLocationCapacities = GetCorrectLocationCapacity(args, seriesLinesLocationCapacity, NEW_ATTRIBUTE);
            if (Vehicle.ListLocationCapacities.Count > 0)
            {
                Console.WriteLine("--- Učitavanje podataka datoteke Lokacija kapaciteta je završeno. (Ispravnih zapisa: " + Vehicle.ListLocationCapacities.Count + ") ---");
            }
            else
            {
                Console.WriteLine("--- Datoteka Lokacija kapaciteti ne sadrži niti jedan ispravan zapis ---");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        public static void LoadFilePersons(string[] args)
        {
            string path = CheckPathFilePersons(args);
            VehiclesSingleton Vehicle = VehiclesSingleton.GetVehiclesInstance();

            Console.WriteLine("\n--- Čitanje datoteke Osobe ---");
            string personFile = "";
            try
            {
                personFile = System.IO.File.ReadAllText(path);
            }
            catch
            {
                Console.WriteLine("--- Datoteka Osobe nije učitana, provjerite da li se datoteka nalazi na putanji " + path + "");
                Console.WriteLine("--- IZLAZAK IZ PROGRAMA ---");
                Console.ReadLine();
                Environment.Exit(0);
            }

            Console.WriteLine("--- Loadanje podataka iz datoteke Osobe! ---");
            string[] seriesLinesPersons = rowsFromFile(personFile, NEW_LINE);
            Vehicle.ListPersons = GetCorrectPersons(args, seriesLinesPersons, NEW_ATTRIBUTE);
            if (Vehicle.ListPersons.Count > 0)
            {
                Console.WriteLine("--- Učitavanje podataka datoteke Osobe je završeno. (Ispravnih zapisa: " + Vehicle.ListPersons.Count + ") ---");
            }
            else
            {
                Console.WriteLine("--- Datoteka Osobe ne sadrži niti jedan ispravan zapis ---");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        public static void LoadFilePriceList(string[] args)
        {
            string path = CheckPathFilePriceList(args);
            VehiclesSingleton Vehicle = VehiclesSingleton.GetVehiclesInstance();

            Console.WriteLine("\n--- Čitanje datoteke Cjenik ---");
            string priceListFile = "";
            try
            {
                priceListFile = System.IO.File.ReadAllText(path);
            }
            catch
            {
                Console.WriteLine("--- Datoteka Cjenik nije učitana, provjerite da li se datoteka nalazi na putanji " + path + "");
                Console.WriteLine("--- IZLAZAK IZ PROGRAMA ---");
                Console.ReadLine();
                Environment.Exit(0);
            }

            Console.WriteLine("--- Loadanje podataka iz datoteke Cjenik! ---");
            string[] seriesLinesPriceList = rowsFromFile(priceListFile, NEW_LINE);
            Vehicle.ListPriceList = GetCorrectPriceList(args, seriesLinesPriceList, NEW_ATTRIBUTE);
            if (Vehicle.ListPriceList.Count > 0)
            {
                Console.WriteLine("--- Učitavanje podataka datoteke Cjenik je završeno. (Ispravnih zapisa: " + Vehicle.ListPriceList.Count + ") ---");
            }
            else
            {
                Console.WriteLine("--- Datoteka Cjenik ne sadrži niti jedan ispravan zapis ---");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        public static void LoadFileActivity(string[] args)
        {
            string path = CheckPathFileActivity(args);
            VehiclesSingleton Vehicle = VehiclesSingleton.GetVehiclesInstance();

            Console.WriteLine("\n--- Čitanje datoteke Aktivnosti ---");
            string activityFile = "";
            try
            {
                activityFile = System.IO.File.ReadAllText(path);
            }
            catch
            {
                Console.WriteLine("--- Datoteka Aktivnosti nije učitana, provjerite da li se datoteka nalazi na putanji " + path + "");
                Console.WriteLine("--- IZLAZAK IZ PROGRAMA ---");
                Console.ReadLine();
                Environment.Exit(0);
            }

            Console.WriteLine("--- Loadanje podataka iz datoteke Aktivnosti! ---");
            string[] seriesLinesActivity = rowsFromFile(activityFile, NEW_LINE);
            Vehicle.ListActivity = GetCorrectActivity(args, seriesLinesActivity, NEW_ATTRIBUTE);
            if (Vehicle.ListActivity.Count > 0)
            {
                Console.WriteLine("--- Učitavanje podataka datoteke Aktivnosti je završeno. (Ispravnih zapisa: " + Vehicle.ListActivity.Count + ") ---");
            }
            else
            {
                Console.WriteLine("--- Datoteka Aktivnosti ne sadrži niti jedan ispravan zapis ---");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        public static void LoadFileCompanies(string[] args)
        {
            string path = CheckPathFileCompany(args);
            VehiclesSingleton Vehicle = VehiclesSingleton.GetVehiclesInstance();

            Console.WriteLine("\n--- Čitanje datoteke Tvrtke ---");
            string companyFile = "";
            try
            {
                companyFile = System.IO.File.ReadAllText(path);
            }
            catch
            {
                Console.WriteLine("--- Datoteka Tvrtke nije učitana, provjerite da li se datoteka nalazi na putanji " + path + "");
                Console.WriteLine("--- IZLAZAK IZ PROGRAMA ---");
                Console.ReadLine();
                Environment.Exit(0);
            }

            Vehicle.SetCompositeCompany();

            Console.WriteLine("--- Loadanje podataka iz datoteke Tvrtke! ---");
            string[] seriesLinesCompany = rowsFromFile(companyFile, NEW_LINE);
            Vehicle.ListCompanies = GetCorrectCompany(args, seriesLinesCompany, NEW_ATTRIBUTE);
            if (Vehicle.ListCompanies.Count > 0)
            {
                Console.WriteLine("--- Učitavanje podataka datoteke Tvrtke je završeno. (Ispravnih zapisa: " + Vehicle.ListCompanies.Count + ") ---");
            }
            else
            {
                Console.WriteLine("--- Datoteka Tvrtke ne sadrži niti jedan ispravan zapis ---");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        public static void LoadFileActivityChange(string[] args)
        {
            VehiclesSingleton Vehicle = VehiclesSingleton.GetVehiclesInstance();

            Console.WriteLine("\n--- Čitanje datoteke Aktivnosti ---");
            string activityFile = "";

            try
            {
                activityFile = System.IO.File.ReadAllText(args[0]);
            }
            catch
            {
                Console.WriteLine("--- Datoteka Aktivnosti nije učitana, provjerite da li se datoteka nalazi na putanji " + args[0] + "");
                Console.WriteLine("--- IZLAZAK IZ PROGRAMA ---");
                Console.ReadLine();
                Environment.Exit(0);
            }

            Console.WriteLine("--- Loadanje podataka iz datoteke Aktivnosti! ---");
            string[] seriesLinesActivity = rowsFromFile(activityFile, NEW_LINE);
            Vehicle.ListActivity = GetCorrectActivity(args, seriesLinesActivity, NEW_ATTRIBUTE);
            if (Vehicle.ListActivity.Count > 0)
            {
                Console.WriteLine("--- Učitavanje podataka datoteke Aktivnosti je završeno. (Ispravnih zapisa: " + Vehicle.ListActivity.Count + ") ---");
            }
            else
            {
                Console.WriteLine("--- Datoteka Aktivnosti ne sadrži niti jedan ispravan zapis ---");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Metoda služi za provjeru ulaznih parametara
        /// </summary>
        /// <param name="args">Input parameters</param>
        /// <param name="regex">Regex input</param>
        /// <returns>Vraća true ako input parameters prolaze check ili false ukoliko ne prolazi</returns>
        public static bool CheckInputParametersFunc(string[] args, string regex)
        {
            if (args.Length == 14)
            {
                if (args.Length != 14 || !ContainsEveryFlag(args))
                {
                    return false;
                }
                else
                {
                    bool foundError = false;
                    if (!ParametersCorrectOrder(args))
                    {
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i <= 13; i = i)
                        {
                            if (args[i].Trim() == "-t")
                            {
                                if (!DateTime.TryParse(string.Format(args[i + 1]).TrimStart('„').TrimStart('"').TrimEnd('“').TrimEnd('"'), out DateTime datum))
                                {
                                    foundError = true;
                                }
                                i += 2;
                            }
                            else
                            {
                                Match check = Regex.Match(args[i + 1], regex);
                                if (!check.Success)
                                {
                                    foundError = true;
                                }
                                i += 2;
                            }
                        }
                        return !foundError;
                    }
                }
            }
            else if (args.Length == 24)
            {
                if (args.Length != 24 || !ContainsEveryFlag(args))
                {
                    return false;
                }
                else
                {
                    bool foundError = false;
                    if (!ParametersCorrectOrder(args))
                    {
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i <= 12; i = i)
                        {
                            if (args[i].Trim() == "-t")
                            {
                                if (!DateTime.TryParse(string.Format(args[i + 1]).TrimStart('„').TrimStart('"').TrimEnd('“').TrimEnd('"'), out DateTime datum))
                                {
                                    foundError = true;
                                }
                                i += 2;
                            }
                            else
                            {
                                Match check = Regex.Match(args[i + 1], regex);
                                if (!check.Success)
                                {
                                    foundError = true;
                                }
                                i += 2;
                            }
                        }
                        return !foundError;
                    }
                }
            }
            else if (args.Length == 22)
            {
                if (args.Length != 22 || !ContainsEveryFlag(args))
                {
                    return false;
                }
                else
                {
                    bool foundError = false;
                    if (!ParametersCorrectOrder(args))
                    {
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i <= 12; i = i)
                        {
                            if (args[i].Trim() == "-t")
                            {
                                if (!DateTime.TryParse(string.Format(args[i + 1]).TrimStart('„').TrimStart('"').TrimEnd('“').TrimEnd('"'), out DateTime datum))
                                {
                                    foundError = true;
                                }
                                i += 2;
                            }
                            else
                            {
                                Match check = Regex.Match(args[i + 1], regex);
                                if (!check.Success)
                                {
                                    foundError = true;
                                }
                                i += 2;
                            }
                        }
                        return !foundError;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public static bool CheckInputParametersFuncWithS(string[] args, string regex)
        {
            if (args.Length == 16)
            {
                if (args.Length != 16 || !ContainsEveryFlagWithS(args))
                {
                    return false;
                }
                else
                {
                    bool foundError = false;
                    if (!ParametersCorrectOrderWithS(args))
                    {
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i <= 15; i = i)
                        {
                            if (args[i].Trim() == "-t")
                            {
                                if (!DateTime.TryParse(string.Format(args[i + 1]).TrimStart('„').TrimStart('"').TrimEnd('“').TrimEnd('"'), out DateTime datum))
                                {
                                    foundError = true;
                                }
                                i += 2;
                            }
                            else
                            {
                                Match check = Regex.Match(args[i + 1], regex);
                                if (!check.Success)
                                {
                                    foundError = true;
                                }
                                i += 2;
                            }
                        }
                        return !foundError;
                    }
                }
            }
            else if (args.Length == 26)
            {
                if (args.Length != 26 || !ContainsEveryFlag(args))
                {
                    return false;
                }
                else
                {
                    bool foundError = false;
                    if (!ParametersCorrectOrder(args))
                    {
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i <= 14; i = i)
                        {
                            if (args[i].Trim() == "-t")
                            {
                                if (!DateTime.TryParse(string.Format(args[i + 1]).TrimStart('„').TrimStart('"').TrimEnd('“').TrimEnd('"'), out DateTime datum))
                                {
                                    foundError = true;
                                }
                                i += 2;
                            }
                            else
                            {
                                Match check = Regex.Match(args[i + 1], regex);
                                if (!check.Success)
                                {
                                    foundError = true;
                                }
                                i += 2;
                            }
                        }
                        return !foundError;
                    }
                }
            }
            else if (args.Length == 24)
            {
                if (args.Length != 24 || !ContainsEveryFlag(args))
                {
                    return false;
                }
                else
                {
                    bool foundError = false;
                    if (!ParametersCorrectOrder(args))
                    {
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i <= 14; i = i)
                        {
                            if (args[i].Trim() == "-t")
                            {
                                if (!DateTime.TryParse(string.Format(args[i + 1]).TrimStart('„').TrimStart('"').TrimEnd('“').TrimEnd('"'), out DateTime datum))
                                {
                                    foundError = true;
                                }
                                i += 2;
                            }
                            else
                            {
                                Match check = Regex.Match(args[i + 1], regex);
                                if (!check.Success)
                                {
                                    foundError = true;
                                }
                                i += 2;
                            }
                        }
                        return !foundError;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public static bool CheckInputParametersFuncWithQuotes(string[] args, string regex)
        {
            if (args.Length != 13 || !ContainsEveryFlag(args))
            {
                return false;
            }
            else
            {
                bool foundError = false;
                if (!ParametersCorrectOrderWithQuotes(args))
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i <= 12; i = i)
                    {
                        if (args[i].Trim() == "-t")
                        {
                            if (!DateTime.TryParse(string.Format(args[i + 1]).TrimStart('„').TrimStart('"').TrimEnd('“').TrimEnd('"'), out DateTime datum))
                            {
                                foundError = true;
                            }
                            i += 3;
                        }
                        else
                        {
                            Match check = Regex.Match(args[i + 1], regex);
                            if (!check.Success)
                            {
                                foundError = true;
                            }
                            i += 2;
                        }
                    }
                    return !foundError;
                }
            }
        }

        public static bool CheckInputParametersFuncWithQuotesAndS(string[] args, string regex)
        {
            if (args.Length != 15 || !ContainsEveryFlag(args))
            {
                return false;
            }
            else
            {
                bool foundError = false;
                if (!ParametersCorrectOrderWithQuotesAndS(args))
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i <= 14; i = i)
                    {
                        if (args[i].Trim() == "-t")
                        {
                            if (!DateTime.TryParse(string.Format(args[i + 1]).TrimStart('„').TrimStart('"').TrimEnd('“').TrimEnd('"'), out DateTime datum))
                            {
                                foundError = true;
                            }
                            i += 3;
                        }
                        else
                        {
                            Match check = Regex.Match(args[i + 1], regex);
                            if (!check.Success)
                            {
                                foundError = true;
                            }
                            i += 2;
                        }
                    }
                    return !foundError;
                }
            }
        }

        private static bool ContainsEveryFlag(string[] args)
        {
            List<string> listArguments = new List<string>();
            foreach (string argument in args)
            {
                listArguments.Add(argument.Trim());
            }

            if (!listArguments.Contains("-v"))
                return false;
            else if (!listArguments.Contains("-l"))
                return false;
            else if (!listArguments.Contains("-c"))
                return false;
            else if (!listArguments.Contains("-k"))
                return false;
            else if (!listArguments.Contains("-o"))
                return false;
            else if (!listArguments.Contains("-t"))
                return false;
            else if (!listArguments.Contains("-os"))
                return false;
            else
            {
                return true;
            }
        }

        private static bool ContainsEveryFlagWithS(string[] args)
        {
            List<string> listArguments = new List<string>();
            foreach (string argument in args)
            {
                listArguments.Add(argument.Trim());
            }

            if (!listArguments.Contains("-v"))
                return false;
            else if (!listArguments.Contains("-l"))
                return false;
            else if (!listArguments.Contains("-c"))
                return false;
            else if (!listArguments.Contains("-k"))
                return false;
            else if (!listArguments.Contains("-o"))
                return false;
            else if (!listArguments.Contains("-t"))
                return false;
            else if (!listArguments.Contains("-s"))
                return false;
            else if (!listArguments.Contains("-os"))
                return false;
            else
            {
                return true;
            }
        }

        private static bool ParametersCorrectOrder(string[] args)
        {
            for (int i = 0; i <= 11; i=i)
            {
                if (ContainsFlag(args[i].Trim()))
                {
                    if (args[i].Trim() == "-t")
                    {
                        i += 2;
                        if (i > 11)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        i += 2;
                        if (i > 11)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private static bool ParametersCorrectOrderWithS(string[] args)
        {
            for (int i = 0; i <= 13; i = i)
            {
                if (ContainsFlag(args[i].Trim()))
                {
                    if (args[i].Trim() == "-t")
                    {
                        i += 2;
                        if (i > 13)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        i += 2;
                        if (i > 13)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private static bool ParametersCorrectOrderWithQuotes(string[] args)
        {
            for (int i = 0; i <= 12; i = i)
            {
                if (ContainsFlag(args[i].Trim()))
                {
                    if (args[i].Trim() == "-t")
                    {
                        i += 3;
                        if (i > 12)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        i += 2;
                        if (i > 12)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private static bool ParametersCorrectOrderWithQuotesAndS(string[] args)
        {
            for (int i = 0; i <= 14; i = i)
            {
                if (ContainsFlag(args[i].Trim()))
                {
                    if (args[i].Trim() == "-t")
                    {
                        i += 3;
                        if (i > 14)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        i += 2;
                        if (i > 14)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private static bool ContainsFlag(string data)
        {
            if (data == "-v")
                return true;
            else if (data == "-l")
                return true;
            else if (data == "-c")
                return true;
            else if (data == "-k")
                return true;
            else if (data == "-o")
                return true;
            else if (data == "-t")
                return true;
            else if (data == "-s")
                return true;
            else if (data == "-os")
                return true;
            else
                return false;
        }

        /// <summary>
        /// Metoda služi za split-anje teksta datoteke u retke
        /// </summary>
        /// <param name="input">String input</param>
        /// <param name="split">Char split</param>
        /// <returns>Vraća tekst u retcima</returns>
        public static string[] rowsFromFile(string input, char split)
        {
            string[] list = input.Split(split);
                for (int i = 0; i < list.Length; i++)
                {
                    list[i].Trim();
                }
            return list;
        }

        private static string CheckPathFileVehicles(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Trim() == "-v")
                {
                    return args[i + 1].Trim();
                }
            }
            return "";
        }

        private static string CheckPathFileConfiguration(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                return args[i].Trim();
            }
            return "";
        }

        private static string CheckPathFileLocations(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Trim() == "-l")
                {
                    return args[i + 1].Trim();
                }
            }
            return "";
        }

        private static string CheckPathFileLocationCapacity(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Trim() == "-k")
                {
                    return args[i + 1].Trim();
                }
            }
            return "";
        }

        private static string CheckPathFilePersons(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Trim() == "-o")
                {
                    return args[i + 1].Trim();
                }
            }
            return "";
        }

        private static string CheckPathFilePriceList(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Trim() == "-c")
                {
                    return args[i + 1].Trim();
                }
            }
            return "";
        }

        private static string CheckPathFileActivity(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Trim() == "-s")
                {
                    return args[i + 1].Trim();
                }
            }
            return "";
        }

        private static string CheckPathFileCompany(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Trim() == "-os")
                {
                    return args[i + 1].Trim();
                }
            }
            return "";
        }

        public static string CheckValueVirtualTime(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Trim() == "-t")
                {
                    if (args[i + 1].Contains('„') || args[i + 2].Contains('“'))
                    {
                        return args[i + 1] + " " + args[i + 2];
                    }
                    else
                    {
                        return args[i + 1];
                    }
                }
            }
            return "";
        }

        public static bool CheckValueVirtualTimeDatetime(string[] args, string date)
        {
            date = date.TrimStart('"').TrimEnd('"').TrimStart('„').TrimEnd('“');
            DateTime? dateTime = DateTime.TryParseExact(date, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime datetime)
                ? datetime
                : (DateTime?)null;

            string virtualTime = CheckValueVirtualTime(args).TrimStart('"').TrimEnd('"').TrimStart('„').TrimEnd('“');

            DateTime? dateTime1 = DateTime.TryParseExact(virtualTime, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out DateTime datetime1)
                        ? datetime1
                        : (DateTime?)null;

            if (datetime >= datetime1)
            {
                return true;
            }
            else
            {
                return false;
            }
            return false;
        }

        public static bool CheckValueVirtualTimeDatetimeWithS(string[] args, string date)
        {
            date = date.TrimStart('"').TrimEnd('"').TrimStart('„').TrimEnd('“');
            date = date.Substring(2);
            DateTime? dateTime = DateTime.TryParseExact(date, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime datetime)
                ? datetime
                : (DateTime?)null;

            string virtualTime = CheckValueVirtualTime(args).TrimStart('"').TrimEnd('"').TrimStart('„').TrimEnd('“');

            DateTime? dateTime1 = DateTime.TryParseExact(virtualTime, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out DateTime datetime1)
                        ? datetime1
                        : (DateTime?)null;

            if (datetime >= datetime1)
            {
                return true;
            }
            else
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Metoda koja provjerava je li sadrži privremeni objekt točan broj atributa
        /// </summary>
        /// <param name="inputObject">Objekt</param>
        /// <param name="number">Točan broj atr</param>
        /// <returns></returns>
        private static bool CheckAttributesNumber(string[] inputObject, int number)
        {
            if (inputObject.Length == number)
                return true;
            else
                return false;
        }

        private static bool CorrectNumber(string data)
        {
            if (!int.TryParse(data.Trim(), out int tempId))
                return false;
            else
                return true;
        }

        private static bool CorrectDecimal(string data)
        {
            string[] decimalPlaces = data.Split(',');
            if (data.Trim().Length == 0)
                return false;
            else if (float.Parse(decimalPlaces[1]) > 99)
                return false;
            else
                return true;
        }

        private static bool CorrectStringData(string data)
        {
            if (data.Trim().Length == 0)
                return false;
            else
                return true;
        }

        public static List<Vehicle> GetCorrectVehicles(string[] args, string[] seriesLinesVehicle, char split)
        {
            List<Vehicle> outputList = new List<Vehicle>();
            string[] headerAttributes = seriesLinesVehicle[0].Split(split);
            for (int i = 1; i < seriesLinesVehicle.Length; i++)
            {
                int countError = 0;
                string[] tempObject = seriesLinesVehicle[i].Split(split);
                if (!CheckAttributesNumber(tempObject, headerAttributes.Length))
                    countError++;
                else if (!CorrectNumber(tempObject[0]))
                    countError++;
                else if (!CorrectStringData(tempObject[1]))
                    countError++;
                else if (!CorrectNumber(tempObject[2]))
                    countError++;
                else if (!CorrectNumber(tempObject[3]))
                    countError++;

                if (countError != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" - {0}. redak nije u ispravnom formatu postavljen | Preskačem >>> {1}", i, seriesLinesVehicle[i]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (args.Contains("-i") && args.Contains("-s"))
                    {
                        string outputX = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
                        for (int k = 0; k < seriesLinesVehicle[i].Length; k++)
                        {
                            outputX += "x";
                        }
                        Console.WriteLine(outputX);
                    }
                }
                else
                {
                    outputList.Add(CreateVehicle(tempObject));
                    tempVehIds.Add(int.Parse(tempObject[0]));
                }
            }
            return outputList;
        }

        public static List<Location> GetCorrectLocations(string[] args, string[] seriesLinesLocation, char split)
        {
            List<Location> outputList = new List<Location>();
            string[] headerAttributes = seriesLinesLocation[0].Split(split);
            for (int i = 1; i < seriesLinesLocation.Length; i++)
            {
                int countError = 0;
                string[] tempObject = seriesLinesLocation[i].Split(split);
                if (!CheckAttributesNumber(tempObject, headerAttributes.Length))
                    countError++;
                else if (!CorrectNumber(tempObject[0]))
                    countError++;
                else if (!CorrectStringData(tempObject[1]))
                    countError++;
                else if (!CorrectStringData(tempObject[2]))
                    countError++;
                else if (!CorrectStringData(tempObject[3]))
                    countError++;

                if (countError != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" - {0}. redak nije u ispravnom formatu postavljen | Preskačem >>> {1}", i, seriesLinesLocation[i]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (args.Contains("-i") && args.Contains("-s"))
                    {
                        string outputX = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
                        for (int k = 0; k < seriesLinesLocation[i].Length; k++)
                        {
                            outputX += "x";
                        }
                        Console.WriteLine(outputX);
                    }
                }
                else
                {
                    outputList.Add(CreateLocation(tempObject));
                }
            }
            return outputList;
        }

        public static List<LocationCapacity> GetCorrectLocationCapacity(string[] args, string[] seriesLinesLocationCapacity, char split)
        {
            List<LocationCapacity> outputList = new List<LocationCapacity>();
            string[] headerAttributes = seriesLinesLocationCapacity[0].Split(split);
            for (int i = 1; i < seriesLinesLocationCapacity.Length; i++)
            {
                int countError = 0;
                string[] tempObject = seriesLinesLocationCapacity[i].Split(split);
                if (!CheckAttributesNumber(tempObject, headerAttributes.Length))
                    countError++;
                else if (!CorrectNumber(tempObject[0]))
                    countError++;
                else if (!CorrectNumber(tempObject[1]))
                    countError++;
                else if (!CorrectNumber(tempObject[2]))
                    countError++;
                else if (!CorrectNumber(tempObject[3]))
                    countError++;

                if (countError != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" - {0}. redak nije u ispravnom formatu postavljen | Preskačem >>> {1}", i, seriesLinesLocationCapacity[i]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (args.Contains("-i") && args.Contains("-s"))
                    {
                        string outputX = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
                        for (int k = 0; k < seriesLinesLocationCapacity[i].Length; k++)
                        {
                            outputX += "x";
                        }
                        Console.WriteLine(outputX);
                    }
                }
                else
                {
                    outputList.Add(CreateLocationCapacity(tempObject));
                }
            }
            return outputList;
        }

        public static List<Person> GetCorrectPersons(string[] args, string[] seriesLinesPerson, char split)
        {
            List<Person> outputList = new List<Person>();
            string[] headerAttributes = seriesLinesPerson[0].Split(split);
            for (int i = 1; i < seriesLinesPerson.Length; i++)
            {
                int countError = 0;
                string[] tempObject = seriesLinesPerson[i].Split(split);
                if (!CheckAttributesNumber(tempObject, headerAttributes.Length))
                    countError++;
                else if (!CorrectNumber(tempObject[0]))
                    countError++;
                else if (!CorrectStringData(tempObject[1]))
                    countError++;
                else if (!CorrectNumber(tempObject[2]))
                    countError++;

                if (countError != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" - {0}. redak nije u ispravnom formatu postavljen | Preskačem >>> {1}", i, seriesLinesPerson[i]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (args.Contains("-i") && args.Contains("-s"))
                    {
                        string outputX = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
                        for (int k = 0; k < seriesLinesPerson[i].Length; k++)
                        {
                            outputX += "x";
                        }
                        Console.WriteLine(outputX);
                    }
                }
                else
                {
                    outputList.Add(CreatePerson(tempObject));
                }
            }
            return outputList;
        }

        public static List<PriceList> GetCorrectPriceList(string[] args, string[] seriesLinesPriceList, char split)
        {
            List<PriceList> outputList = new List<PriceList>();
            string[] headerAttributes = seriesLinesPriceList[0].Split(split);
            for (int i = 1; i < seriesLinesPriceList.Length; i++)
            {
                int countError = 0;
                string[] tempObject = seriesLinesPriceList[i].Split(split);
                if (!CheckAttributesNumber(tempObject, headerAttributes.Length))
                    countError++;
                else if (!CorrectNumber(tempObject[0]))
                    countError++;
                else if (!CorrectDecimal(tempObject[1]))
                    countError++;
                else if (!CorrectDecimal(tempObject[2]))
                    countError++;
                else if (!CorrectDecimal(tempObject[3]))
                    countError++;

                if (countError != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" - {0}. redak nije u ispravnom formatu postavljen | Preskačem >>> {1}", i, seriesLinesPriceList[i]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (args.Contains("-i") && args.Contains("-s"))
                    {
                        string outputX = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
                        for (int k = 0; k < seriesLinesPriceList[i].Length; k++)
                        {
                            outputX += "x";
                        }
                        Console.WriteLine(outputX);
                    }
                }
                else
                {
                    outputList.Add(CreatePriceList(tempObject));
                }
            }
            return outputList;
        }

        public static List<Activity> GetCorrectActivity(string[] args, string[] seriesLinesActivity, char split)
        {
            List<Activity> outputList = new List<Activity>();
            string[] headerAttributes = seriesLinesActivity[0].Split(split);
            for (int i = 1; i < seriesLinesActivity.Length; i++)
            {
                int countError = 0;
                string[] tempObject = seriesLinesActivity[i].Split(split);
                if (tempObject.Length == 5)
                {
                    if (!CorrectNumber(tempObject[0]))
                        countError++;
                    else if (!CorrectStringData(tempObject[1]) || !(CheckValueVirtualTimeDatetimeWithS(args, tempObject[1])))
                        countError++;
                    else if (!CorrectNumber(tempObject[2]))
                        countError++;
                    else if (!CorrectNumber(tempObject[3]))
                        countError++;
                    else if (!CorrectNumber(tempObject[4]) || !tempVehIds.Contains(int.Parse(tempObject[4])))
                        countError++;
                }
                else if (tempObject.Length == 6)
                {
                    if (!CorrectNumber(tempObject[0]))
                        countError++;
                    else if (!CorrectStringData(tempObject[1]) || !(CheckValueVirtualTimeDatetimeWithS(args, tempObject[1])))
                        countError++;
                    else if (!CorrectNumber(tempObject[2]))
                        countError++;
                    else if (!CorrectNumber(tempObject[3]))
                        countError++;
                    else if (!CorrectNumber(tempObject[4]) || !tempVehIds.Contains(int.Parse(tempObject[4])))
                        countError++;
                    else if (!CorrectNumber(tempObject[5]))
                        countError++;
                }
                else if (tempObject.Length == 7)
                {
                    if (!CorrectNumber(tempObject[0]))
                        countError++;
                    else if (!CorrectStringData(tempObject[1]) || !(CheckValueVirtualTimeDatetimeWithS(args, tempObject[1])))
                        countError++;
                    else if (!CorrectNumber(tempObject[2]))
                        countError++;
                    else if (!CorrectNumber(tempObject[3]))
                        countError++;
                    else if (!CorrectNumber(tempObject[4]) || !tempVehIds.Contains(int.Parse(tempObject[4])))
                        countError++;
                    else if (!CorrectNumber(tempObject[5]))
                        countError++;
                    else if (!CorrectStringData(tempObject[6]))
                        countError++;
                }
                else if (tempObject.Length == 2)
                {
                    if (tempObject[0] != "10") {
                        if (!CorrectNumber(tempObject[0]))
                            countError++;
                        else if (!CorrectStringData(tempObject[1]))
                            countError++;
                    }
                    else
                    {
                        string[] splitDates = tempObject[1].Split(' ');
                        DateTime? insertDate = DateTime.TryParseExact(splitDates[2].Trim(), dateTimeFormatActivity, CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out DateTime datetime)
                                        ? datetime
                                        : (DateTime?)null;
                        DateTime? insertDate1 = DateTime.TryParseExact(splitDates[3].Trim(), dateTimeFormatActivity, CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out DateTime datetime1)
                                        ? datetime1
                                        : (DateTime?)null;

                        if (!CorrectNumber(tempObject[0]))
                            countError++;
                        else if (datetime == DateTime.MinValue || datetime1 == DateTime.MinValue)
                            countError++;
                    }
                }
                else if (tempObject.Length == 1)
                {
                    if (!CorrectNumber(tempObject[0]))
                        countError++;
                }

                if (countError != 0)
                    Console.WriteLine(" - {0}. redak nije u ispravnom formatu postavljen | Preskačem >>> {1}", i, seriesLinesActivity[i]);
                else
                {
                    outputList.Add(CreateActivity(tempObject));
                }
            }
            return outputList;
        }

        public static List<Company> GetCorrectCompany(string[] args, string[] seriesLinesCompany, char split)
        {
            List<Company> outputList = new List<Company>();
            string[] headerAttributes = seriesLinesCompany[0].Split(split);
            for (int i = 1; i < seriesLinesCompany.Length; i++)
            {
                int countError = 0;
                string[] tempObject = seriesLinesCompany[i].Split(split);
                if (!CorrectNumber(tempObject[0]))
                    countError++;
                else if (!CorrectStringData(tempObject[1]))
                    countError++;
                else if (!CorrectNumber(tempObject[2]) && tempObject[2] == null)
                    countError++;
                else if (!CorrectStringData(tempObject[3]) && tempObject[3] == null)
                    countError++;

                if (countError != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" - {0}. redak nije u ispravnom formatu postavljen | Preskačem >>> {1}", i, seriesLinesCompany[i]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (args.Contains("-i") && args.Contains("-s"))
                    {
                        string outputX = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
                        for (int k = 0; k < seriesLinesCompany[i].Length; k++)
                        {
                            outputX += "x";
                        }
                        Console.WriteLine(outputX);
                    }
                }
                else
                {
                    var company = CreateCompany(tempObject);
                    if (company != null)
                        outputList.Add(company);
                }
            }
            return outputList;
        }

        private static Vehicle CreateVehicle(string[] line)
        {
            int tempId = int.Parse(line[0].Trim());
            string tempName = line[1].Trim();
            int tempChargingTime = int.Parse(line[2].Trim());
            int tempRange = int.Parse(line[3].Trim());

            IVehicleBuilder builder = new VehicleConcreteBuilder();
            VehicleBuildDirector director = new VehicleBuildDirector(builder);

            return director.Construct(tempId, tempName, tempChargingTime, tempRange);
        }

        private static Location CreateLocation(string[] line)
        {
            Location tempLocation = new Location();
            tempLocation.SetId(int.Parse(line[0].Trim()));
            tempLocation.SetLocationName(line[1].Trim());
            tempLocation.SetLocationAddress(line[2].Trim());
            tempLocation.SetGps(line[3].Trim());
            return tempLocation;
        }

        private static LocationCapacity CreateLocationCapacity(string[] line)
        {
            LocationCapacity tempLocationCapacity = new LocationCapacity();
            tempLocationCapacity.SetLocationId(int.Parse(line[0].Trim()));
            tempLocationCapacity.SetVehicleId(int.Parse(line[1].Trim()));
            tempLocationCapacity.SetSeat(int.Parse(line[2].Trim()));
            tempLocationCapacity.SetAvailableVehicles(int.Parse(line[3].Trim()));
            return tempLocationCapacity;
        }

        private static Person CreatePerson(string[] line)
        {
            Person tempPerson = new Person();
            tempPerson.SetId(int.Parse(line[0].Trim()));
            tempPerson.SetFirstLastName(line[1].Trim());
            tempPerson.SetContract(int.Parse(line[2].Trim()));
            return tempPerson;
        }

        private static PriceList CreatePriceList(string[] line)
        {
            PriceList tempPriceList = new PriceList();
            tempPriceList.SetVehicleId(int.Parse(line[0].Trim()));
            tempPriceList.SetRent(line[1].Trim());
            tempPriceList.SetByHour(line[2].Trim());
            tempPriceList.SetByKilometers(line[3].Trim());
            return tempPriceList;
        }

        private static Activity CreateActivity(string[] line)
        {
            Activity tempActivity = new Activity();
            if (line.Length == 5)
            {
                tempActivity.SetId(int.Parse(line[0].Trim()));
                tempActivity.SetDatetime(line[1].Trim());
                tempActivity.SetPersonId(int.Parse(line[2].Trim()));
                tempActivity.SetLocationId(int.Parse(line[3].Trim()));
                tempActivity.SetVehicleId(int.Parse(line[4].Trim()));
            }
            else if (line.Length == 6)
            {
                tempActivity.SetId(int.Parse(line[0].Trim()));
                tempActivity.SetDatetime(line[1].Trim());
                tempActivity.SetPersonId(int.Parse(line[2].Trim()));
                tempActivity.SetLocationId(int.Parse(line[3].Trim()));
                tempActivity.SetVehicleId(int.Parse(line[4].Trim()));
                tempActivity.SetNumberKilometers(int.Parse(line[5].Trim()));
            }
            else if (line.Length == 7)
            {
                tempActivity.SetId(int.Parse(line[0].Trim()));
                tempActivity.SetDatetime(line[1].Trim());
                tempActivity.SetPersonId(int.Parse(line[2].Trim()));
                tempActivity.SetLocationId(int.Parse(line[3].Trim()));
                tempActivity.SetVehicleId(int.Parse(line[4].Trim()));
                tempActivity.SetNumberKilometers(int.Parse(line[5].Trim()));
                tempActivity.SetDescriptionProblem(line[6].Trim());
            }
            else if (line.Length == 2 && line[1].Trim().StartsWith("„") == true)
            {
                tempActivity.SetId(int.Parse(line[0].Trim()));
                tempActivity.SetDatetime(line[1].Trim());
            }
            else if (line.Length == 2 && line[1].Trim().StartsWith("„") == false)
            {
                tempActivity.SetId(int.Parse(line[0].Trim()));
                tempActivity.SetCustomActivitiy(line[1].Trim());
            }
            else if (line.Length == 1)
            {
                tempActivity.SetId(int.Parse(line[0].Trim()));
            }
            return tempActivity;
        }
        private static Company CreateCompany(string[] line)
        {
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();

            Company tempCompany = new Company();
            tempCompany.SetMyId(int.Parse(line[0].Trim()));
            tempCompany.SetMyName(line[1].Trim());
            if (line[3].Trim() != null && line[3].Trim() != "")
            {
                string[] locationId = line[3].Trim().Split(',');
                List<int> ListLocationIds = new List<int>();
                for (int i = 0; i < locationId.Length; i++)
                {
                    var location = vehicle.FindLocation(int.Parse(locationId[i]));
                    if (location != null)
                    {
                        tempCompany.Locations.Add(location);
                    }
                }
            }
            if (line[2].Trim() == "")
            {
                tempCompany.SetSuperiorUnit(null);
                vehicle.GetCompositeCompany().AddChild(tempCompany);
            }
            else
            {
                var superior = vehicle.GetCompositeCompany().FindCompany(int.Parse(line[2].Trim())) as Company;
                tempCompany.SetSuperiorUnit(superior);
                superior.AddChild(tempCompany);
            }
            return tempCompany;
        }
    }
}
