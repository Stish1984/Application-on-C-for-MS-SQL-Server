using System;
using System.Collections.Generic;


namespace Sales_registration.Sales
{
    public interface ISale:IDisposable
    {
        IEnumerable<Sale> GetList();
        void Create(Sale  sale);
        Sale GetSale(DateTime Date);
        void Save();
    }
}
