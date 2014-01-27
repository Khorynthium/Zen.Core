using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Abstractions.Commands;
using Raven.Client;

namespace Zen.DataStore.Raven
{
    public class BasicRavenRepositoryWithGuid<TEntity> : BasicRavenRepository<TEntity>,
                                                         IRepositoryWithGuid<TEntity>
        where TEntity : IHasGuidId
    {
        public BasicRavenRepositoryWithGuid(IDocumentSession session, IBasicRavenRepositoryConfiguration configuration) : base(session, configuration)
        {
        }

        public IQueryable<TEntity> Find(IEnumerable<Guid> guids)
        {
            //return Session.Query<TEntity>().Where(x => x.Guid.In<Guid>(guids));
            return Session
                .Load<TEntity>(guids.Select(guid => typeof (TEntity).Name + "s/" + guid))
                .ToArray()
                .AsQueryable();
        }

        /// <summary>
        ///     ����� ������ �� GUID
        /// </summary>
        /// <param name="guid">���������� �� �������</param>
        /// <returns></returns>
        public TEntity Find(Guid guid)
        {
            return Session.Load<TEntity>(typeof (TEntity).Name + "s/" + guid);
        }

        public void Clone(TEntity entity)
        {
            Session.Advanced.Evict(entity);
            entity.Guid = Guid.NewGuid();
            Session.Store(entity, typeof (TEntity).Name + "s/" + entity.Guid);
        }
    }
}