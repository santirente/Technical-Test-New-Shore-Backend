using LiteDB;
using Microsoft.Extensions.Configuration;
using Newshore.Journeys.Domain.Entities;
using Newshore.Journeys.Infrastructure.DataModels;
using Newshore.Journeys.Infrastructure.InfrastructureService;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Newshore.Journeys.Infrastructure.Implementation
{
    public class NewshoreFlightsService : INewshoreFlightsService
    {
        private List<Flight> FlightsInCache;

        private readonly HttpClient Client;

        private readonly IConfiguration _config;

        public NewshoreFlightsService(IConfiguration config)
        {
            _config = config;
            FlightsInCache = new List<Flight>();
            Client = new HttpClient();
        }
        public void DoInitialCharge()
        {
            string apiUrl = _config.GetSection("NewshoreRecruitingApiUrl").Value;
            var responseTask = Client.GetAsync(apiUrl);
            responseTask.Wait();
            HttpResponseMessage response = responseTask.Result;
            string responseBody;
            if (response.IsSuccessStatusCode)
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                responseBody = task.Result;
                IEnumerable<FlightDataModel> flights = JsonConvert.DeserializeObject<IEnumerable<FlightDataModel>>(responseBody);
                foreach (FlightDataModel flight in flights)
                {
                    FlightsInCache.Add(Flight.Create(Transport.Create(flight.FlightCarrier, flight.FlightNumber),
                        flight.DepartureStation, flight.ArrivalStation, flight.Price));
                }
            }
        }

        public List<Flight> GetByOriginAndDestination(string origin, string destination)
        {
            return FlightsInCache.Where(it => it.Origin == origin && it.Destination == destination).ToList();
        }

        public List<Flight> GetByOrigin(string origin)
        {
            return FlightsInCache.Where(it => it.Origin == origin).ToList();
        }
    }
}
