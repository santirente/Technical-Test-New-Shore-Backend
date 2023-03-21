using System;
using System.Collections.Generic;
using System.Text;

namespace Newshore.Journeys.Domain.Entities
{
    public class Transport
    {
        public string FlightCarrier { get; private set; }

        public string FlightNumber { get; private set; }

        public Transport() { }

        private Transport(string flightCarrier, string flightNumber) => (FlightCarrier, FlightNumber) = (flightCarrier, flightNumber);
     

        public static Transport Create(string flightCarrier, string flightNumber)
        {
            return new Transport(flightCarrier, flightNumber);
        }

    }
}
