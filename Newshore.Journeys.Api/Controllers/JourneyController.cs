using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newshore.Journeys.Application;
using Newshore.Journeys.Application.Response;
using Newshore.Journeys.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Newshore.Journeys.Api
{
    [ApiController]
    [Route("api/journey")]
    public class JourneyController : ControllerBase
    {
        private readonly ICalculateJourneyApplicationService CalculateJourneyApplicationService;

        public JourneyController(ICalculateJourneyApplicationService calculateJourneyApplicationService)
        {
            this.CalculateJourneyApplicationService = calculateJourneyApplicationService;

        }
        [HttpGet]
        [Route("/get_one_way_journeys/{origin}/{destination}/{maxNumberOfFlights}")]
        public JourneysResponse<IEnumerable<Journey>> GetOneWayJourneys(string origin, string destination, int maxNumberOfFlights)
        {
            return this.CalculateJourneyApplicationService.CalculateOneWayJourneys(origin, destination, maxNumberOfFlights);
        }
        [HttpGet]
        [Route("/get_round_trip_journeys/{origin}/{destination}/{maxNumberOfFlights}")]
        public JourneysResponse<IEnumerable<Journey>> GetRoundTripJourneys(string origin, string destination, int maxNumberOfFlights)
        {
            return this.CalculateJourneyApplicationService.CalculateRoundTripJourneys(origin, destination, maxNumberOfFlights);
        }
    }
}
