using Pattern.Adapter.Model;
using System.Collections.Generic;

namespace Pattern.Adapter.DataBaseLayer
{
    interface IDataBase
    {
        void CreateDatabaseAndTable();
        void SetNewRecord(ResponceObject newRecord);
        List<ResponceObject> GetAllProducts();
    }
}
