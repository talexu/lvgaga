using System;
using LvModel.Common;
using LvService.Commands.Azure.Storage.Table;
using LvService.DbContexts;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Tests.Commands.Azure.Storage
{
    public class AzureStorageFixture
    {
        public IAzureStorage AzureStorage { get; private set; }

        // Table
        public DeleteTableCommand DeleteTableCommand = new DeleteTableCommand();

        // Entity
        public CreateTableEntityCommand CreateTableEntityCommand = new CreateTableEntityCommand();
        public CreateTableEntitiesCommand CreateTableEntitiesCommand = new CreateTableEntitiesCommand();
        public ReadTableEntityCommand ReadTableEntityCommand = new ReadTableEntityCommand();
        public ReadTableEntitiesCommand ReadTableEntitiesCommand = new ReadTableEntitiesCommand();
        public DeleteTableEntityCommand DeleteTableEntityCommand = new DeleteTableEntityCommand();
        public DeleteTableEntitiesCommand DeleteTableEntitiesCommand = new DeleteTableEntitiesCommand();

        public AzureStorageFixture()
        {
            //AzureStorage = new AzureStorageDb();
            AzureStorage = new AzureStoragePool(new AzureStorageDb());
        }

        public string GetRandomTableName()
        {
            return String.Format("t{0}", Guid.NewGuid().ToString().Replace("-", ""));
        }

        public string GetTableFilterByPartitionKey(string partitionKey)
        {
            return TableQuery.GenerateFilterCondition(Constants.PartitionKey, QueryComparisons.Equal, partitionKey);
        }
    }
}