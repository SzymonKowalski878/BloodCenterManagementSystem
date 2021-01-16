using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodCenterManagementSystem.DataAccess
{
    public class Repository<T>:IRepository<T> where T:BaseModel
    {
        private readonly Lazy<DataContext> _dataContext;
        protected DataContext DataContext => _dataContext.Value;


        public Repository(Lazy<DataContext> dataContext)
        {
            _dataContext = dataContext;
        }

        public virtual void Add(T model)
        {
            DataContext.Set<T>().Add(model);
        }

        public virtual void SaveChanges()
        {
            DataContext.SaveChanges();
        }

        public virtual T GetById(int id)
        {
            return DataContext.Set<T>().FirstOrDefault(m => m.Id == id);
        }
    }
}
