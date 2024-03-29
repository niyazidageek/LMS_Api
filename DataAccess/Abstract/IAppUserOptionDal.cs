﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IAppUserOptionDal: IRepository<AppUserOption>
    {
        Task<List<AppUserOption>> GetAllByQuizIdAndUserIdAsync(int quizId, string userId);
    }
}
