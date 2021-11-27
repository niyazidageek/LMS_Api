using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;

namespace Business.Concrete
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectDal _conext;

        public SubjectService(ISubjectDal conext)
        {
            _conext = conext;
        }

        public async Task<bool> AddSubjectAsync(Subject subject)
        {
            await _conext.AddAsync(subject);

            return true;
        }

        public async Task<bool> DeleteSubjectAsync(int id)
        {
            await _conext.DeleteAsync(new Subject { Id = id });

            return true;
        }

        public async Task<bool> EditSubjectAsync(Subject subject)
        {
            await _conext.UpdateAsync(subject);

            return true;
        }

        public async Task<Subject> GetSubjectByIdAsync(int id)
        {
            return await _conext.GetAsync(s => s.Id == id);
        }

        public async Task<List<Subject>> GetSubjectsAsync()
        {
            return await _conext.GetAllAsync();
        }

        public async Task<List<Subject>> GetSubjectsByPageAndSizeAsync(int page, int size)
        {
            return await _conext.GetByPageAndSizeAsync(page, size);
        }

        public async Task<int> GetSubjectsCountAsync()
        {
            return await _conext.GetSubjectsCountAsync();
        }
    }
}
