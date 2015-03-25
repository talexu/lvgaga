﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LvModel.View.Tumblr;

namespace LvService.Services
{
    public interface ITumblrService
    {
        Task<List<TumblrModel>> GetTumblrModelsAsync(TumblrCategory category, int takeCount);
    }
}