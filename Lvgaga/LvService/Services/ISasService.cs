﻿using System.Threading.Tasks;

namespace LvService.Services
{
    public interface ISasService
    {
        Task<string> GetSasForTable(string tableName);
        Task<string> GetSasForTable(string tableName, string partitionKey);
    }
}