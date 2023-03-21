using Newshore.Journeys.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Newshore.Journeys.Domain.Repository
{
    public interface IJourneyRepository
    {
        public List<Journey> GetByOriginAndDestination(string origin, string destination);
    }
}
