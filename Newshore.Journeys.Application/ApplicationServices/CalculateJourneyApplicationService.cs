using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newshore.Journeys.Application._Messages;
using Newshore.Journeys.Application.Response;
using Newshore.Journeys.Domain.Entities;
using Newshore.Journeys.Domain.Exceptions;
using Newshore.Journeys.Domain.Repository;
using Newshore.Journeys.Infrastructure.InfrastructureService;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Newshore.Journeys.Application.ApplicationServices
{
    public class CalculateJourneyApplicationService : ICalculateJourneyApplicationService
    {
        private readonly IRepository<Journey> repository;

        private INewshoreFlightsService NewshoreFlightsService { get; set; }

        private int MaxNumberOfFlightsPerJourney { get; set; }

        private readonly ILogger<CalculateJourneyApplicationService> @Logger;

        public CalculateJourneyApplicationService(IRepository<Journey> repository, INewshoreFlightsService newshoreFlightsService,
            ILogger<CalculateJourneyApplicationService> @logger)
            => (this.repository, NewshoreFlightsService, @Logger) = (repository, newshoreFlightsService, @logger);
        public JourneysResponse<IEnumerable<Journey>> CalculateOneWayJourneys(string origin, string destination, int maxNumberOfFlights)
        {
            MaxNumberOfFlightsPerJourney = maxNumberOfFlights;
            return JourneysResponse<IEnumerable<Journey>>.Create(() =>
            {
                IJourneyRepository journeyRepository = (IJourneyRepository)this.repository;
                List<Journey> journeys = journeyRepository.GetByOriginAndDestination(origin, destination);
                if (journeys == null || journeys.Count() == 0)
                {
                    List<Flight> initialFlights = NewshoreFlightsService.GetByOriginAndDestination(origin, destination);
                    if (initialFlights == null || initialFlights.Count() == 0)
                    {
                        initialFlights = NewshoreFlightsService.GetByOrigin(origin);
                        journeys = InitializeJourneys(origin, destination, initialFlights);
                        journeys = BuildPosibbleJourneys(journeys, destination, journeyRepository);
                    }
                    else
                    {
                        journeys = InitializeJourneys(origin, destination, initialFlights);
                        journeys = CalculateJourneysPrice(journeys);
                    }
                    SaveJourneys(journeys);
                }
                return journeys;

            }, ResponseMessage.JourneysCalculatedSuccessfully, @Logger).Execute();

        }

        public List<Journey> BuildPosibbleJourneys(List<Journey> journeys,
            string destination, IJourneyRepository journeyRepository)
        {   if (journeys.Count() == 0 || journeys.Any(it => it.Flights == null || it.Flights.Count() == 0))
                throw new DomainException(UseCaseMessage.CantCalculateJourneys);
            if (journeys.Any(itJ => itJ.Flights.Count() >= MaxNumberOfFlightsPerJourney
            || itJ.Flights.Any(itF => itF.Destination == destination)))
                return FilterCompatibleJourneys(journeys, destination);
            List<Journey> journeysLeveledUp = new List<Journey>();
            foreach (Journey journey in journeys)
            {
                journeysLeveledUp.AddRange(AddNextFlightToJourneys(journey));
            }
            return BuildPosibbleJourneys(journeysLeveledUp, destination, journeyRepository);

        }

        public JourneysResponse<IEnumerable<Journey>> CalculateRoundTripJourneys(string origin, string destination, int maxNumberOfFlights)
        {
            return JourneysResponse<IEnumerable<Journey>>.Create(() =>
            {
                List<Journey> journeys = CalculateOneWayJourneys(origin, destination, maxNumberOfFlights).Result.ToList();
                journeys.AddRange(CalculateOneWayJourneys(destination, origin, maxNumberOfFlights).Result.ToList());
                return journeys;
            }, ResponseMessage.JourneysCalculatedSuccessfully, @Logger).Execute();

        }

        public List<Journey> InitializeJourneys(string origin, string destination, List<Flight> flights)
        {
            List<Journey> journeys = new List<Journey>();
            foreach (Flight flight in flights)
            {
                journeys.Add(Journey.Create(origin, destination, new List<Flight>() { flight }));
            }
            return journeys;
        }

        public List<Journey> AddNextFlightToJourneys(Journey journey)
        {
            Journey journeyLeveledUp;
            List<Flight> foundedFlights = NewshoreFlightsService.GetByOrigin(journey.Flights.Last().Destination);
            if (foundedFlights == null || foundedFlights.Count() == 0)
                throw new DomainException(UseCaseMessage.FlightsDidNotFound);
            List<Journey> journeysLeveledUp = new List<Journey>();
            foreach (Flight flight in foundedFlights)
            {
                journeyLeveledUp = Journey.Create(journey.Origin, journey.Destination, journey.Flights);
                journeyLeveledUp.AddFlight(flight);
                journeyLeveledUp.CalculatePrice();
                journeysLeveledUp.Add(journeyLeveledUp);
            }
            return journeysLeveledUp;
        }

        public List<Journey> FilterCompatibleJourneys(List<Journey> journeys, string destination)
        {
            return journeys.Where(it => it.Flights.Last().Destination == destination).ToList();
        }

        public List<Journey> CalculateJourneysPrice(List<Journey> journeys)
        {
            foreach (Journey journey in journeys)
                journey.CalculatePrice();
            return journeys;
        }

        public void SaveJourneys(List<Journey> journeys)
        {
            if (journeys.Count() == 0)
                throw new DomainException(UseCaseMessage.CantCalculateJourneys);
            foreach(Journey journey in journeys)
            {
                repository.Post(journey);
            }
        }
    }
}
