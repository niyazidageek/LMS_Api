using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;

namespace DataAccess.Concrete
{
    public class EFLessonDal: EFRepositoryBase<Lesson, AppDbContext>, ILessonDal
    {
        public EFLessonDal(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> AddWithFilesAsync(Lesson lesson, List<string> fileNames)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                List<LessonMaterial> lessonMaterials = new();

                foreach (var fileName in fileNames)
                {
                    var lessonMaterial = new LessonMaterial();

                    var materialDb = new Material();

                    materialDb.FileName = fileName;
                    await Context.Materials.AddAsync(materialDb);
                    await Context.SaveChangesAsync();

                    lessonMaterial.Material = materialDb;
                    lessonMaterial.Lesson = lesson;

                    lessonMaterials.Add(lessonMaterial);
                }

                lesson.LessonMaterials = lessonMaterials;

                await Context.Lessons.AddAsync(lesson);
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
