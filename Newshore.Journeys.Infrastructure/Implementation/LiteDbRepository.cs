using LiteDB;
using Microsoft.Extensions.Configuration;
using Newshore.Journeys.Domain.Entities;
using Newshore.Journeys.Domain.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Newshore.Journeys.Infrastructure.Implementation
{
    public class LiteDbRepository<TEntity> : IJourneyRepository, IRepository<TEntity> where TEntity : class
    {
        private readonly IConfiguration _config;

        public LiteDbRepository(IConfiguration config)
        {
            _config = config;
        }
        public List<Journey> GetByOriginAndDestination(string origin, string destination)
        {
            using (var db = new LiteDatabase(_config.GetSection("LiteDbUrl").Value))
            {
                var journeysCollection = db.GetCollection<Journey>(typeof(TEntity).ToString().Split(".").Last());
                return journeysCollection.FindAll().Where(it => it.Origin == origin && it.Destination == destination
                && it.Flights.First().Origin == origin && it.Flights.Last().Destination == destination).ToList();
            }
        }

        public void Post(TEntity entity)
        {
            using (var db = new LiteDatabase(_config.GetSection("LiteDbUrl").Value))
            {
                var journeysCollection = db.GetCollection<TEntity>(typeof(TEntity).ToString().Split(".").Last());
                journeysCollection.Insert(entity);
            }
        }

    }
}
