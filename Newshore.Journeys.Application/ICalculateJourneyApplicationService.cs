using Newshore.Journeys.Application.Response;
using Newshore.Journeys.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Newshore.Journeys.Application
{
    public interface ICalculateJourneyApplicationService
    {
        public JourneysResponse<IEnumerable<Journey>> CalculateOneWayJourneys(string origin, string destination, int maxNumberOfFlights);
            
        public JourneysResponse<IEnumerable<Journey>> CalculateRoundTripJourneys(string origin, string destination, int maxNumberOfFlights);
    }
}
