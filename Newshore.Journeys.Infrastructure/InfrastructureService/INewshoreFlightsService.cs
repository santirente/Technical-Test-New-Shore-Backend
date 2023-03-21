using Newshore.Journeys.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Newshore.Journeys.Infrastructure.InfrastructureService
{
    public interface INewshoreFlightsService
    {
        public void DoInitialCharge();

        public List<Flight> GetByOriginAndDestination(string origin, string destination);

        public List<Flight> GetByOrigin(string origin);
    }
}
