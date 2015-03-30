using System;
using System.Collections.Generic;
using System.Dynamic;
using LvModel.Azure.StorageTable;
using LvModel.View.Tumblr;
using LvService.Factories.Azure.Storage;
using LvService.Factories.Uri;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Tests.Utilities
{
    public class TestDataGenerator
    {
        private static readonly TableEntityFactory TableEntityFactory = new TableEntityFactory(new UriFactory());

        #region ITableEntity
        public static ITableEntity GetTableEntity(string partitionKey = null, string rowKey = null)
        {
            var pk = partitionKey ?? Guid.NewGuid().ToString();
            var rk = rowKey ?? Guid.NewGuid().ToString();
            return new TableEntity(pk, rk);
        }

        public static List<ITableEntity> GetTableEntities(int count = 20, string partitionKey = null)
        {
            partitionKey = String.IsNullOrEmpty(partitionKey) ? Guid.NewGuid().ToString() : partitionKey;

            var entities = new List<ITableEntity>(count);
            for (var i = 0; i < count; i++)
            {
                entities.Add(GetTableEntity(partitionKey, Guid.NewGuid().ToString()));
            }
            return entities;
        }

        #endregion

        #region TumblrText

        public static TumblrText GetTestTumblrText(TumblrCategory category = TumblrCategory.C1)
        {
            return new TumblrText
            {
                Text = Guid.NewGuid().ToString(),
                Category = category
            };
        }

        public static List<TumblrText> GetTestTumblrTexts(int count)
        {
            var results = new List<TumblrText>(count);
            for (var i = 0; i < count; i++)
            {
                results.Add(GetTestTumblrText());
            }
            return results;
        }
        #endregion

        #region TumblrEntity

        public static TumblrEntity GetTumblrEntity(string partitionKey = null)
        {
            dynamic p = new ExpandoObject();
            p.PartitionKey = partitionKey ?? Guid.NewGuid().ToString();
            p.MediaUri = String.Format("http://www.caoliu.com/{0}.jpg", Guid.NewGuid());
            p.TumblrText = GetTestTumblrText();
            return TableEntityFactory.CreateTumblrEntity(p);
        }

        public static List<TumblrEntity> GetTumblrEntities(int count = 20, string partitionKey = null)
        {
            partitionKey = String.IsNullOrEmpty(partitionKey) ? Guid.NewGuid().ToString() : partitionKey;

            var entities = new List<TumblrEntity>(count);
            for (var i = 0; i < count; i++)
            {
                entities.Add(GetTumblrEntity(partitionKey));
            }
            return entities;
        }
        #endregion
    }
}