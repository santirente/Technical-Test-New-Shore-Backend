using System;
using System.Collections.Generic;
using System.Text;

namespace Newshore.Journeys.Domain.Entities
{
    public class Journey
    {
        public List<Flight> Flights { get; private set; }

        public string Origin { get; private set; }

        public string Destination { get; private set; }

        public double Price { get; private set; }

        public Journey() { }

        private Journey(string origin, string destination) => (this.Origin, this.Destination, this.Flights) 
            = (origin, destination, new List<Flight>());

        private Journey(string origin, string destination, List<Flight> flights)
        {
            this.Origin = origin;
            this.Destination = destination;
            this.Flights = new List<Flight>();
            this.Flights.AddRange(flights);
        }

        public static Journey Create(string origin, string destionation)
        {
            return new Journey(origin, destionation);
        }

        public static Journey Create(string origin, string destionation, List<Flight> flights)
        {
            return new Journey(origin, destionation, flights);
        }

        public void CalculatePrice()
        {
            this.Price = 0;
            foreach(Flight flight in Flights)
            {
                this.Price += flight.Price;
            }
        }

        public void AddFlight(Flight flight)
        {
            this.Flights.Add(flight);
        }

        public void AddFlights(IEnumerable<Flight> flights)
        {
            this.Flights.AddRange(flights);
        }
    }
}
