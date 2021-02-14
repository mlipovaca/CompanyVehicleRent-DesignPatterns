using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3
{
    public class Activity
    {
        public int Id;
        public string Datetime;
        public int PersonId;
        public int LocationId;
        public int VehicleId;
        public int NumberKilometers;
        public string DescriptionProblem;
        public DateTime CustomDate;
        public string CustomActivity;

        public void SetId(int id)
        {
            Id = id;
        }
        public int GetId()
        {
            return Id;
        }

        public void SetDatetime(string dateTime)
        {
            Datetime = dateTime;
        }
        public string GetDatetime()
        {
            return Datetime;
        }

        public void SetCustomDatetime(DateTime customDate)
        {
            CustomDate = customDate;
        }
        public DateTime GetCustomDatetime()
        {
            return CustomDate;
        }

        public void SetPersonId(int personId)
        {
            PersonId = personId;
        }
        public int GetPersonId()
        {
            return PersonId;
        }

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

        public void SetNumberKilometers(int numberKilometers)
        {
            NumberKilometers = numberKilometers;
        }
        public int GetNumberKilometers()
        {
            return NumberKilometers;
        }
        public void SetDescriptionProblem(string descriptionProblem)
        {
            DescriptionProblem = descriptionProblem;
        }

        public string GetDescriptionProblem()
        {
            return DescriptionProblem;
        }

        public void SetCustomActivitiy(string customActivity)
        {
            CustomActivity = customActivity;
        }

        public string GetCustomActivity()
        {
            return CustomActivity;
        }
    }
}
