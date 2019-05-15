using System;
using System.Collections.Generic;


namespace Sales_registration.Managers
{
    public interface IRepository:IDisposable
    {
        IEnumerable<Manager> GetList();
        Manager GetManager(int ID);
        void Create(Manager manager);
        void Delete(int ID);
        void Update(Manager manager);
        void Save();
    }
}
