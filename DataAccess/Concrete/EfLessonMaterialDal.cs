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
    public class EfLessonMaterialDal: EFRepositoryBase<LessonMaterial, AppDbContext>, ILessonMaterialDal
    {
        public EfLessonMaterialDal(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> AddAsync(List<LessonMaterial> lessonMaterials)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                foreach (var lessonMaterial in lessonMaterials)
                {
                    var fileName = await FileHelper.AddFile(lessonMaterial.File);

                    LessonMaterial lessonMaterialdb = new()
                    {
                        LessonId = lessonMaterial.LessonId,
                        FileName = fileName
                    };

                    await Context.LessonMaterials.AddAsync(lessonMaterialdb);
                    
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

        public async Task<bool> DeleteAsync(List<LessonMaterial> lessonMaterials)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                foreach (var lessonMaterial in lessonMaterials)
                {
                    FileHelper.DeleteFile(lessonMaterial.FileName);
                    var lessonMaterialDb = await Context.LessonMaterials
                        .FirstOrDefaultAsync(lm=>lm.FileName == lessonMaterial.FileName);
                    Context.LessonMaterials.Remove(lessonMaterialDb);
                    
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

        public async Task<List<LessonMaterial>> GetAllByLessonIdAsync(int lessonId)
        {
            return await Context.LessonMaterials
                .AsNoTracking()
                .Where(lm => lm.LessonId == lessonId)
                .ToListAsync();
        }
    }
}
