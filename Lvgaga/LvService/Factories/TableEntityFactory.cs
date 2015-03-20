using LvModel.Azure.StorageTable;
using LvModel.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Factories
{
    public class TableEntityFactory : ITableEntityFactory
    {

        public ITableEntity CreateTumblrEntity(dynamic p)
        {

            return new TumblrEntity(MediaType.Image.ToString("G"), "");
        }
    }
}