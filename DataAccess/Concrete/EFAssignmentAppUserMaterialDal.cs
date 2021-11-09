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
    public class EFAssingnmentAppUserMaterial : EFRepositoryBase<AssignmentAppUserMaterial, AppDbContext>, IAssignmentAppUserMaterialDal
    {
        public EFAssingnmentAppUserMaterial(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> CreateAssignmentAppUserMaterialAsync(List<AssignmentAppUserMaterial> assignmentAppUserMaterials)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                foreach (var assignmentAppUserMaterial in assignmentAppUserMaterials)
                {
                    var fileName = await FileHelper.AddFile(assignmentAppUserMaterial.File);

                    AssignmentAppUserMaterial assignmentAppUserMaterialDb = new()
                    {
                        AssignmentAppUserId = assignmentAppUserMaterial.AssignmentAppUserId,
                        FileName = fileName
                    };

                    await Context.AssignmentAppUserMaterials.AddAsync(assignmentAppUserMaterial);
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
