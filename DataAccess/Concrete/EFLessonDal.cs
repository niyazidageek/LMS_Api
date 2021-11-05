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

                    var fileName = await FileHelper.AddFile(file);

                    lessonMaterial.FileName = fileName;
                    lessonMaterial.LessonId = lesson.Id;

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
                var lessonMaterialsDb = await Context.LessonMaterials
                    .Where(lm => lm.LessonId == lesson.Id)
                    .ToListAsync();

                foreach (var lessonMaterial in lessonMaterialsDb)
                {
                    FileHelper.DeleteFile(lessonMaterial.FileName);
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

        public async Task<bool> UpdateWithoutFilesAsync(Lesson lesson)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                var existingFiles = await Context.LessonMaterials
                   .Where(lm => lm.LessonId == lesson.Id)
                   .ToListAsync();

                if (existingFiles is not null && existingFiles.Count is not 0)
                {
                    var deleteableFiles = existingFiles
                    .Where(ef => !lesson.LessonMaterials.Any(lm => lm.FileName == ef.FileName))
                    .ToList();

                    if (deleteableFiles is not null)
                    {
                        foreach (var deleteableFile in deleteableFiles)
                        {
                            FileHelper.DeleteFile(deleteableFile.FileName);
                        }
                    }

                    foreach (var existingFile in existingFiles)
                    {
                        Context.LessonMaterials.Remove(existingFile);
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

        public async Task<bool> UpdateWithFilesAsync(Lesson lesson)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                var existingFiles = await Context.LessonMaterials
                   .Where(lm => lm.LessonId == lesson.Id)
                   .ToListAsync();

                if(existingFiles is not null)
                {
                    var deleteableFiles = existingFiles
                    .Where(ef => !lesson.LessonMaterials.Any(lm => lm.FileName == ef.FileName))
                    .ToList();

                    if(deleteableFiles is not null)
                    {
                        foreach (var deleteableFile in deleteableFiles)
                        {
                            FileHelper.DeleteFile(deleteableFile.FileName);
                            Context.LessonMaterials.Remove(deleteableFile);
                        }

                    }
                    
                }

                List<LessonMaterial> lessonMaterials = new();

                foreach (var file in lesson.Files)
                {
                    var lessonMaterial = new LessonMaterial();

                    var fileName = await FileHelper.AddFile(file);

                    lessonMaterial.FileName = fileName;
                    lessonMaterial.LessonId = lesson.Id;

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

        public async Task<List<Lesson>> GetAllAsync()
        {
            return await Context.Lessons.AsNoTracking()
                .Include(l => l.Group)
                .ToListAsync();
        }

        public async Task<Lesson> GetAsync(int id)
        {
            return await Context.Lessons.AsNoTracking()
                .Include(l=>l.LessonMaterials)
                .Include(l => l.Group)
                .FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}
