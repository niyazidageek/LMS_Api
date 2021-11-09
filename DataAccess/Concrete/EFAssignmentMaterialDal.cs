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
    public class EFAssignmentMaterialDal : EFRepositoryBase<AssignmentMaterial, AppDbContext>, IAssignmentMaterialDal
    {
        public EFAssignmentMaterialDal(AppDbContext context) : base(context)
        {
        }

        public async Task<List<AssignmentMaterial>> GetAllByAssignmentIdAsync(int assignmentId)
        {
            return await Context.AssignmentMaterials
                .AsNoTracking()
                .Where(am => am.AssignmentId == assignmentId)
                .ToListAsync();
        }

        public async Task<bool> CreateAssignmentMaterialsAsync(List<AssignmentMaterial> assignmentMaterials)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                foreach (var assignmentMaterial in assignmentMaterials)
                {
                    var fileName = await FileHelper.AddFile(assignmentMaterial.File);

                    AssignmentMaterial assignmentMaterialDb = new()
                    {
                        AssignmentId = assignmentMaterial.AssignmentId,
                        FileName = fileName
                    };

                    await Context.AssignmentMaterials.AddAsync(assignmentMaterialDb);                  
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

        public async Task<bool> DeleteAssignmentMaterialssAsync(List<AssignmentMaterial> assignmentMaterials)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                foreach (var assignmentMaterial in assignmentMaterials)
                {
                    FileHelper.DeleteFile(assignmentMaterial.FileName);
                    var assignmentMaterialDb = await Context.AssignmentMaterials
                        .FirstOrDefaultAsync(am => am.FileName == assignmentMaterial.FileName);
                    Context.AssignmentMaterials.Remove(assignmentMaterialDb);
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
