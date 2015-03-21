using System;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Factories
{
    public class TableEntityFactory : ITableEntityFactory
    {
        public ITableEntity CreateTumblrEntity(dynamic p)
        {
            var now = DateTime.UtcNow;
            return new TumblrEntity(Constants.MediaTypeImage, DateTimeHelper.GetInvertedTicks(now))
            {
                Uri = Guid.NewGuid() + ".jpg",
                Text = p.Text,
                CreateTime = now,
                State = TumblrState.Active
            };
        }
    }
}