using System;
using System.Collections.Generic;
using System.Data.Entity;


namespace Sales_registration.Managers
{
    public class SQLRepository : IRepository
    {
        private Context db;

        public SQLRepository()
        {
            this.db = new Context();
        }

        public IEnumerable<Manager> GetList()
        {
            return db.Managers;

        }

        public Manager GetManager(int ID)
        {
            return db.Managers.Find(ID);
        }

        public void Create(Manager manager)
        {
            db.Managers.Add(manager);
        }

        public void Delete(int ID)
        {
            Manager manager = db.Managers.Find(ID);
            if (manager != null)
                db.Managers.Remove(manager);
        }

        public void Update(Manager manager)
        {
            db.Entry(manager).State = EntityState.Modified;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
