using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.DTOs;
using Entities.Models;
using LMS_Api.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class EFLessonDal: EFRepositoryBase<Lesson, AppDbContext>, ILessonDal
    {
        public EFLessonDal(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> AddWithFilesAsync(Lesson lesson)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                List<LessonMaterial> lessonMaterials = new();

                foreach (var file in lesson.Files)
                {
                    var lessonMaterial = new LessonMaterial();

                    var materialDb = new Material();

                    var fileName = FileHelper.AddFile(file);

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

        public async Task<bool> DeleteWithFilesAsync(Lesson lesson)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                var lessonMaterialsDb = await Context.LessonMaterials.Where(lm => lm.LessonId == lesson.Id)
                    .Include(lm => lm.Material)
                    .ToListAsync();

                foreach (var lessonMaterial in lessonMaterialsDb)
                {
                    FileHelper.DeleteFile(lessonMaterial.Material.FileName);
                    Context.Materials.Remove(lessonMaterial.Material);
                }


                Context.Lessons.Remove(lesson);
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

        public async Task<bool> EditAsync(Lesson lesson)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                if (lesson.ExistingFileNames is null || lesson.ExistingFileNames.Count == 0)
                {
                    var lessonMaterials = await Context.LessonMaterials.Where(lm => lm.LessonId == lesson.Id)
                        .Include(lm => lm.Material).ToListAsync();

                    foreach (var lessonMaterial in lessonMaterials)
                    {
                        FileHelper.DeleteFile(lessonMaterial.Material.FileName);
                        Context.Materials.Remove(lessonMaterial.Material);
                    }

                }
                else
                {
                    foreach (var existingFileName in lesson.ExistingFileNames)
                    {
                        var lessonMaterial = await Context.LessonMaterials
                            .Include(lm => lm.Material)
                            .FirstOrDefaultAsync(lm => lm.LessonId == lesson.Id &&
                            lm.Material.FileName != existingFileName);

                        var materialDb = await Context.Materials.FirstOrDefaultAsync(m => m.Id == lessonMaterial.Material.Id);
                        FileHelper.DeleteFile(materialDb.FileName);

                        Context.Materials.Remove(materialDb);
                    }
                }


                Context.Update(lesson);
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

        public async Task<bool> EditWithFilesAsync(Lesson lesson)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                if (lesson.ExistingFileNames is null || lesson.ExistingFileNames.Count == 0)
                {
                    var existingLessonMaterials = await Context.LessonMaterials.Where(lm => lm.LessonId == lesson.Id)
                        .Include(lm=>lm.Material).ToListAsync();

                    foreach (var lessonMaterial in existingLessonMaterials)
                    {
                        FileHelper.DeleteFile(lessonMaterial.Material.FileName);
                        Context.Materials.Remove(lessonMaterial.Material);
                    }

                }
                else
                {
                    foreach (var existingFileName in lesson.ExistingFileNames)
                    {
                        var lessonMaterial = await Context.LessonMaterials
                            .Include(lm => lm.Material)
                            .FirstOrDefaultAsync(lm => lm.LessonId == lesson.Id &&
                            lm.Material.FileName != existingFileName);

                        var materialDb = await Context.Materials.FirstOrDefaultAsync(m => m.Id == lessonMaterial.Material.Id);
                        FileHelper.DeleteFile(materialDb.FileName);

                        Context.Materials.Remove(materialDb);
                    }
                }


                List<LessonMaterial> lessonMaterials = new();

                foreach (var file in lesson.Files)
                {
                    var lessonMaterial = new LessonMaterial();

                    var materialDb = new Material();

                    var fileName = FileHelper.AddFile(file);

                    materialDb.FileName = fileName;
                    await Context.Materials.AddAsync(materialDb);
                    await Context.SaveChangesAsync();

                    lessonMaterial.Material = materialDb;
                    lessonMaterial.Lesson = lesson;

                    lessonMaterials.Add(lessonMaterial);
                }

                lesson.LessonMaterials = lessonMaterials;

                Context.Update(lesson);
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
