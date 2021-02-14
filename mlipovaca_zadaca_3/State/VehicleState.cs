using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3.State
{
    public class VehicleState
    {
        public IVehicleStatusState State { get; set; }
        public VehicleState (IVehicleStatusState state)
        {
            State = state;
        }

        public void Request()
        {
            State.ChangeVehicleStatus(this);
        }

        public void OutputAvailable(string vehStatus)
        {
            Console.WriteLine("Trenutno stanje vozila: " + vehStatus + "!");
        }
    }
}
