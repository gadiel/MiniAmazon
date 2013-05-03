using System;
using System.Linq;
using System.Linq.Expressions;
using MiniAmazon.Domain;
using MiniAmazon.Domain.Entities;
using NHibernate;
using NHibernate.Linq;

namespace MiniAmazon.Data
{
    public class Repository : IRepository
    {
        private readonly ISession _session;

        public Repository(ISession session)
        {
            _session = session;
        }

        public T First<T>(Expression<Func<T, bool>> query) where T : class, IEntity
        {
            T firstOrDefault = _session.Query<T>().FirstOrDefault(query);
            return firstOrDefault;
        }

        public T GetById<T>(long id) where T : class, IEntity
        {
            var item = _session.Get<T>(id);
            return item;
        }

        public T Create<T>(T itemToCreate) where T : class, IEntity
        {
            _session.Save(itemToCreate);
            return itemToCreate;
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> expression) where T : class, IEntity
        {
            return _session.Query<T>().Where(expression);
        }

        public T Update<T>(T itemToUpdate) where T : class, IEntity
        {
            _session.Clear();
            _session.Update(itemToUpdate);
            
            _session.Flush();
            return itemToUpdate;
        }

        public void Delete<T>(T itemToDelete)
        {
            _session.Delete(itemToDelete);
        }
    }
}