using mlipovaca_zadaca_3.Composite;
using mlipovaca_zadaca_3.Decorator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3.ChainofResponsibility
{
    public class BillStatHandler : AbstractStatisticHandler
    {
        public BillStatHandler(int option)
        {
            Option = option;
        }

        public override void Calculate(Person person)
        {
            person = new Person();
            VehiclesSingleton vehicle = VehiclesSingleton.GetVehiclesInstance();
            var listPersons = vehicle.ListPersons.ToList();
            List<string> listPersonsBills = new List<string>();
            List<int> counterListPersonBills = new List<int>();

            foreach (var per in listPersons)
            {
                if (!listPersonsBills.Contains(person.GetFirstLastName()))
                {
                    listPersonsBills.Add(person.GetFirstLastName());
                    counterListPersonBills.Add(1);
                }
                else
                {
                    counterListPersonBills[listPersonsBills.IndexOf(person.GetFirstLastName())] += 1;
                }
            }
        }

        public static void OutputStatisticTable(DateTime dateFrom, DateTime dateTo, int personId)
        {
            VehiclesSingleton vehicles = VehiclesSingleton.GetVehiclesInstance();

            Dictionary<int, Tuple<int, string, DateTime, string, string, string>> listBillPersons = new Dictionary<int, Tuple<int, string, DateTime, string, string, string>>();
            int outputId = 0;
            string outputStatus = "";
            string outputBill = "";
            string outputVehicle = "";
            string outputRentLocation = "";
            DateTime outputBillDate = DateTime.Now;

            var person = vehicles.ListPersons.Where(x => x.Id == personId).ToList()[0];
            var rentAllBills = OutputHelper.rentAllBills.Where(x => x.Value.Item2 == person.GetFirstLastName()).Where(x => x.Value.Item1 >= dateFrom)
                                .Where(x => x.Value.Item1 <= dateTo).ToList();
            var rentBills = OutputHelper.rentBills.Where(x => x.Value.Item2 == person.GetFirstLastName()).ToList();

            OutputHelper.Counter = 0;
            IRowTable rowTable =
                new TextDecorator(
                    new TextDecorator(
                        new TextDecorator(
                            new TextDecorator(
                               new TextDecorator(
                                   new TextDecorator(
                                new ConcreteRow()))))));
            string format = rowTable.MakeRow();

            foreach (var bill in rentAllBills)
            {
                outputId = bill.Key + 1;
                outputBill = bill.Value.Item7;
                outputBillDate = bill.Value.Item1;
                for (int i = 0; i < rentBills.Count; i++)
                {
                    if (bill.Key == rentBills[i].Key)
                    {
                        outputStatus = "Dug";
                        break;
                    }
                    else
                    {
                        outputStatus = "Plaćen";
                    }
                }

                var vehRent = OutputHelper.rentVehs.Where(x => x.Value.Item4 == personId).Where(x => x.Value.Item2 == bill.Value.Item3).ToList()[0];

                var veh1 = vehicles.ListVehicles.Where(x => x.Id == vehRent.Value.Item3).ToList()[0];
                outputVehicle = veh1.GetVehicleName();

                outputRentLocation = bill.Value.Item4;

                listBillPersons.Add(bill.Key, new Tuple<int, string, DateTime, string, string, string>(outputId, outputBill, outputBillDate, outputStatus, outputVehicle, outputRentLocation));
            }

            foreach (var listBill in listBillPersons.OrderBy(x => x.Value.Item4).ToList())
            {
                string output = String.Format(format, listBill.Value.Item6, listBill.Value.Item5, listBill.Value.Item4, listBill.Value.Item3, listBill.Value.Item2, listBill.Value.Item1);
                Console.WriteLine(output);
                Console.WriteLine(new String('_', 180));
            }
        }
    }
}
