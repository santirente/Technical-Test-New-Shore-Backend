using System;
using System.Collections.Generic;
using System.Text;

namespace Newshore.Journeys.Domain.Entities
{
    public class Flight
    {
        public Transport Transport { get; private set; }

        public string Origin { get; private set; }

        public string Destination { get; private set; }

        public double Price { get; private set; }

        public Flight() { }
        private Flight(Transport transport, string origin, string destination, double price)
            => (Transport, Origin, Destination, Price) = (transport, origin, destination, price);

        public static Flight Create(Transport transport, string origin, string destination, double price)
        {
            return new Flight(transport, origin, destination, price);
        }

    }
}
