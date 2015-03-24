﻿using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Factories.Azure.Storage
{
    public interface ITableEntityFactory
    {
        ITableEntity CreateTumblrEntity(dynamic p);
    }
}