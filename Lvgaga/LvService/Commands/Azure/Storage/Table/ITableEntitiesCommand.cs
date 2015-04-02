﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public interface ITableEntitiesCommand
    {
        bool CanExecute(dynamic p);
        Task<List<T>> ExecuteAsync<T>(dynamic p) where T : ITableEntity, new();
    }
}