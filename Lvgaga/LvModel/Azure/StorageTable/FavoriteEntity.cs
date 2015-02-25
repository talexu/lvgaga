using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvModel.Azure.StorageTable
{
    public class FavoriteEntity : TableEntity
    {
        public FavoriteEntity(string userId, string tumblrId)
        {
            PartitionKey = userId;
            RowKey = tumblrId;
        }

        public bool State { get; set; }
    }
}
