using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using Dhobi.Repository.Interface.Base;
using MongoDB.Driver;

namespace Dhobi.Repository.Implementation.Base
{
    public class Repository<T> : DhobiContext, IRepository<T> where T : class
    {
        public IMongoCollection<T> Collection { get; private set; }

        public Repository()
        {
            var service = PluralizationService.CreateService(CultureInfo.CurrentCulture);
            Collection = Database.GetCollection<T>(service.Pluralize(typeof(T).Name.ToLower()));
        }
        public bool Add(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public void UpdateEntity(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
