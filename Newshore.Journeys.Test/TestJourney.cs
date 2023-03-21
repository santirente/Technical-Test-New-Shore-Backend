using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newshore.Journeys.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Newshore.Journeys.Test
{
    [TestClass]
    public class TestJourney
    {
        [TestMethod]
        public void TestAddFlight()
        {
            Journey journey = Journey.Create("MZL", "BOG");
            Flight firstFlight = Flight.Create(Transport.Create("CO", "5566"), "MZL", "BOG", 130000);
            journey.AddFlight(firstFlight);
            Flight current = journey.Flights.First();
            Assert.AreEqual(firstFlight, current);
        }

        [TestMethod]
        public void TestAddFlights()
        {
            Journey journey = Journey.Create("MZL", "BOG");
            Flight firstFlight = Flight.Create(Transport.Create("CO", "5566"), "MZL", "BOG", 130000);
            Flight secondFlight = Flight.Create(Transport.Create("CO", "7862"), "MDE", "BOG", 100000);
            List<Flight> flights = new() { firstFlight, secondFlight };
            journey.AddFlights(flights);
            for(int i = 0; i < journey.Flights.Count; i++)
            {
                Assert.AreEqual(flights[i], journey.Flights[i]);
            }
        }

        [TestMethod]
        public void TestCalculatePrice()
        {
            Journey journey = Journey.Create("MZL", "BOG");
            Flight firstFlight = Flight.Create(Transport.Create("CO", "5566"), "MZL", "BOG", 130000);
            Flight secondFlight = Flight.Create(Transport.Create("CO", "7862"), "MDE", "BOG", 100000);
            List<Flight> flights = new() { firstFlight, secondFlight };
            journey.AddFlights(flights);
            journey.CalculatePrice();
            Assert.AreEqual(journey.Price, 230000);
        }
    }
}
