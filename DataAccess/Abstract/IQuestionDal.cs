using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IQuestionDal: IRepository<Question>
    {
        public Task<bool> AddAsync(Question question, List<Option> options);

        public Task<bool> UpdateAsync(Question question, List<Option> options);

    }
}
