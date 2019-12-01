using System;
using System.Collections.Generic;
using System.Threading;

namespace MediatorPattern
{
    // Mediator
    interface IAirportControl
    {
        void Register(Airline airLine);
        void SuggestWay(string fligthNumber, string way);
    }

    // Concrete Mediator
    class OsloControl : IAirportControl
    {
        // Concrete Colleague object instances are stored in this collection.
        private readonly Dictionary<string, Airline> _planes;

        public OsloControl()
        {
            _planes = new Dictionary<string, Airline>();
        }

        #region IAirportControl Members

        // The Register method is used to register the surrounding planes to the control tower. This method can take any Concrete Colleague object instance derived from the Colleague as a parameter.
        public void Register(Airline airLine)
        {
            if (!_planes.ContainsValue(airLine))
                _planes[airLine.FlightNumber] = airLine;

            // In order for the airline to request a new route from the tower, the Concrete Colleague object instance, Mediator reference must be declared.
            airLine.Airport = this;
        }

        // A method used by Concrete Colleague object instances to request a new route. This method makes use of the location information of the other airplanes which it has stored due to the current conditions and draws conclusions. Thus, instead of handling n combinations by each aircraft, all these combinations can be evaluated in the Mediator by reducing them to a smaller number.
        public void SuggestWay(string fligthNumber, string way)
        {
            // Symbolically a new route is being set. The notification is made by calling the GetWay method of the Concrete Colleague object instance following the route.
            Thread.Sleep(250);
            Random rnd = new Random();
            _planes[fligthNumber].GetWay(String.Format("{0}:{1}E;{2}:{3}W", rnd.Next(1, 100).ToString(), rnd.Next(1, 100).ToString(), rnd.Next(1, 100).ToString(), rnd.Next(1, 100).ToString()));
        }

        #endregion
    }

    // Colleague
    abstract class Airline
    {
        public IAirportControl Airport { get; set; }
        public string FlightNumber { get; set; }
        public string From { get; set; }

        // The method used when requesting a new route from the Mediator.
        public void RequestNewWay(string myWay)
        {
            // As it will be noted, the call is made towards the object reference of the Mediator type.
            Airport.SuggestWay(FlightNumber, myWay);
        }

        // The GetaWay method of which the mediator type calls. The parameter content of this method comes from the concrete mediator.
        public virtual void GetWay(string messageFromAirport)
        {
            Console.WriteLine("{0} route", messageFromAirport);
        }
    }

    // Concrete Colleague
    class Flight1 : Airline
    {
        public override void GetWay(string messageFromAirport)
        {
            Console.WriteLine("1, Flight {0} : ", FlightNumber);
            base.GetWay(messageFromAirport);
        }
    }

    // Concrete Colleague
    class Flight2 : Airline
    {
        public override void GetWay(string messageFromAirport)
        {
            Console.WriteLine("2, Flight {0} : ", FlightNumber);
            base.GetWay(messageFromAirport);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Concrete Mediator
            OsloControl osloControl = new OsloControl();

            Flight1 oh101 = new Flight1 { Airport = osloControl, FlightNumber = "oh101", From = "London" };
            osloControl.Register(oh101);
            Flight1 oh132 = new Flight1 { Airport = osloControl, FlightNumber = "oh132", From = "Roma" };
            osloControl.Register(oh132);
            Flight2 zy99 = new Flight2 { Airport = osloControl, FlightNumber = "zy99", From = "Berlin" };
            osloControl.Register(zy99);

            // Airplanes demand new routes.
            zy99.RequestNewWay("34:43E;41:41W");

            oh101.RequestNewWay("34:43E;41:41W");
        }
    }
}