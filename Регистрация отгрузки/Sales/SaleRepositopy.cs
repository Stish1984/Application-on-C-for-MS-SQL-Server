using System;
using System.Collections.Generic;

namespace Sales_registration.Sales
{
    public class SaleRepositopy : ISale
    {
        private Context db;

        public SaleRepositopy()
        {
            this.db = new Context();
        }

        public IEnumerable<Sale> GetList()
        {
            return db.Sales;
        }

        public Sale GetSale(DateTime Date)
        {
            return db.Sales.Find(Date);
        }

        public void Create(Sale sale)
        {
            db.Sales.Add(sale);
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
