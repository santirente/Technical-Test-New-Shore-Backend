using System;
using System.Collections.Generic;
using System.Text;

namespace Newshore.Journeys.Infrastructure.DataModels
{
    public class FlightDataModel
    {
        public TransportDataModel Transport { get; set; }

        public string DepartureStation { get; set; }

        public string ArrivalStation { get; set; }

        public string FlightCarrier { get; set; }

        public string FlightNumber { get; set; }

        public double Price { get; set; }
    }
}
