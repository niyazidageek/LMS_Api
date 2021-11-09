using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using LMS_Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class EfLessonAssignmentDal : EFRepositoryBase<LessonAssignment, AppDbContext>, ILessonAssignmentDal
    {
        public EfLessonAssignmentDal(AppDbContext context) : base(context)
        {
        }

        public async Task<List<LessonAssignment>> GetAllByLessonIdAsync(int lessonId)
        {
            return await Context.LessonAssignments
                .AsNoTracking()
                .Where(la => la.LessonId == lessonId)
                .ToListAsync();
        }

        public async Task<bool> CreateLessonAssignmentsAsync(List<LessonAssignment> lessonAssignments)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                foreach (var lessonAssignment in lessonAssignments)
                {
                    var fileName = await FileHelper.AddFile(lessonAssignment.File);

                    LessonAssignment lessonAssignmentDb = new()
                    {
                        LessonId = lessonAssignment.LessonId,
                        FileName = fileName
                    };

                    await Context.LessonAssignments.AddAsync(lessonAssignmentDb);                  
                }

                await Context.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteLessonAssignmentsAsync(List<LessonAssignment> lessonAssignments)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                foreach (var lessonAssignment in lessonAssignments)
                {
                    FileHelper.DeleteFile(lessonAssignment.FileName);
                    var lessonAssignmentDb = await Context.LessonAssignments
                        .FirstOrDefaultAsync(la => la.FileName == lessonAssignment.FileName);
                    Context.LessonAssignments.Remove(lessonAssignmentDb);
                }

                await Context.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync();
                throw;
            }
        }
    }
}
