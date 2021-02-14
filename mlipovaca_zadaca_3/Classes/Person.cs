using mlipovaca_zadaca_3.Composite;
using mlipovaca_zadaca_3.Decorator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3
{
    public class Person
    {
        public int Id;
        public string FirstLastName;
        public int Contract;
        private List<IComponentCompany> mChildList = new List<IComponentCompany>();

        public void AddChild(IComponentCompany child)
        {
            mChildList.Add(child);
        }

        public void SetId(int id)
        {
            Id = id;
        }
        public int GetId()
        {
            return Id;
        }

        public void SetFirstLastName(string firstLastName)
        {
            FirstLastName = firstLastName;
        }
        public string GetFirstLastName()
        {
            return FirstLastName;
        }

        public void SetContract(int contract)
        {
            Contract = contract;
        }

        public int GetContract()
        {
            return Contract;
        }

        public void ShowData(int activityId)
        {
            VehiclesSingleton vehicles = VehiclesSingleton.GetVehiclesInstance();

            if (activityId == 9)
            {
                List<int> countPersonIds = new List<int>();
                int counterBill = 0;
                int outputId = 0;
                int sumBill = 0;
                string outputFirstLastName = "";
                string outputBill = "";
                DateTime outputRentDate = DateTime.Now;

                var rentVeh = OutputHelper.rentVehs.ToList();

                OutputHelper.Counter = 0;
                IRowTable rowTable =
                    new TextDecorator(
                        new TextDecorator(
                            new TextDecorator(
                                new TextDecorator(
                                    new ConcreteRow()))));
                string format = rowTable.MakeRow();

                foreach (var rent in rentVeh)
                {
                    var person = vehicles.ListPersons.Where(x => x.Id == rent.Value.Item4).ToList()[0];
                    var rentBills = OutputHelper.rentBills.Where(x => x.Value.Item2 == person.GetFirstLastName()).ToList();

                    if (!countPersonIds.Contains(rent.Value.Item4))
                    {
                        foreach (var bill in rentBills)
                        {
                            if (counterBill < rentBills.Count)
                            {
                                if (person.Contract != 0)
                                {
                                    string[] splitBill = bill.Value.Item7.Split(' ');
                                    sumBill += int.Parse(splitBill[12]);
                                    outputBill = sumBill + "kn";
                                }
                                else
                                {
                                    outputBill = 0.ToString() + "kn";
                                }
                                counterBill++;
                            }
                        }
                        outputId = rent.Value.Item4;
                        outputFirstLastName = person.FirstLastName;

                        var lastRentDate = OutputHelper.rentVehs.Where(x => x.Value.Item4 == outputId).LastOrDefault();
                        outputRentDate = lastRentDate.Value.Item1;

                        string output = String.Format(format, outputRentDate, outputBill, outputFirstLastName, outputId);
                        Console.WriteLine(output);
                        Console.WriteLine(new String('_', 120));

                        countPersonIds.Add(outputId);
                        sumBill = 0;
                        counterBill = 0;
                    }
                }
            }
        }
    }
}
