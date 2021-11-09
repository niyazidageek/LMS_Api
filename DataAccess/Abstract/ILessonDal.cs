﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;

namespace DataAccess.Abstract
{
    public interface ILessonDal:IRepository<Lesson>
    {
        Task<List<Lesson>> GetAllAsync();

        //Task<bool> AddWithFilesAsync(Lesson lesson);

        //Task<bool> UpdateWithFilesAsync(Lesson lesson);

        //Task<bool> UpdateWithoutFilesAsync(Lesson lesson);

        //Task<bool> DeleteWithFilesAsync(Lesson lesson);

        Task<List<Lesson>> GetAllByGroupIdAsync(int groupId);

        Task<Lesson> GetAsync(int id);

        Task<List<Lesson>> GetAllByGroupIdAsync(int groupId, int skip = 0, int take = 2);
    }
}
