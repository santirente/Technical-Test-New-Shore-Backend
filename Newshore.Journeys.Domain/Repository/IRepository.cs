using System.Collections.Generic;

namespace Newshore.Journeys.Domain.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public void Post(TEntity entity);
    }
}
