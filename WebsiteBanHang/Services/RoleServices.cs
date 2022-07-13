using Microsoft.EntityFrameworkCore;
using MilkTeaShop.Helper;
using MilkTeaShop.Models;

namespace MilkTeaShop.Services
{
    public interface IRole
    {
        public Task<DataResult<IEnumerable<Role>>> GetList();
        public Task<DataResult<Role>> Get(int? id = null);
        public Task<DataResult<IEnumerable<Role>>> Delete(int[]? ids);
        public Task<DataResult<IEnumerable<Role>>> Update(IEnumerable<Role> data);
        public Task<DataResult<IEnumerable<Role>>> Add(IEnumerable<Role> data);
    }
    public class RoleServices : IRole
    {
        private readonly MilkTeaShopContext _dbContext;
        public RoleServices(MilkTeaShopContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<DataResult<IEnumerable<Role>>> GetList()
        {
            var result = new DataResult<IEnumerable<Role>>();
            try
            {
                result.Data = await _dbContext.Roles.ToListAsync();
                if (result.Data.Count() > 0)
                {
                    result.Status = true;
                    result.Message = "get Role list successfully";
                }
                else
                {
                    result.Status = false;
                    result.Message = "Role table is empty";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "get Role list failed, see detail in errors";
                result.Errors = ex.ToString();
            }
            return result;
        }
        public async Task<DataResult<Role>> Get(int? id = null)
        {
            try
            {
                var query = _dbContext.Roles.AsQueryable();

                if (id != null)
                    query = query.Where(x => x.RoleId == id);
                var response = await query.FirstOrDefaultAsync();
                return response != null ?
                 new DataResult<Role>("get Role by id  successully", true, response) :
                 new DataResult<Role>("get Role width id = " + id + " failed", false, null);
            }
            catch (Exception ex)
            {
                return new DataResult<Role>("get Role width id = " + id + " failed, see detail in error list", false, null, ex.ToString());
            }

        }
        public async Task<DataResult<IEnumerable<Role>>> Delete(int[]? ids)
        {
            var returns = new DataResult<IEnumerable<Role>>();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (ids != null && ids.Length > 0)
                    {
                        var data = await _dbContext.Roles.Where(x => ids.Contains(x.RoleId)).ToListAsync();
                        if (data != null && data.Count > 0)
                        {
                            _dbContext.RemoveRange(data);
                            await _dbContext.SaveChangesAsync();
                            tran.Commit();
                            returns.Data = data;
                            returns.Message = "delete Role by id successfully";
                            returns.Status = true;
                        }
                        else
                        {
                            returns.Message = "get list Role for delete failed";
                            returns.Status = false;
                        }

                        return returns;
                    }

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    returns.Message = "delete Role width id failed, see detail in error";
                    returns.Status = false;
                    returns.Errors = ex.ToString();
                }
            }
            return returns;
        }
        public async Task<DataResult<IEnumerable<Role>>> Add(IEnumerable<Role> data)
        {
            var returns = new DataResult<IEnumerable<Role>>();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (data != null)
                    {

                        _dbContext.UpdateRange(data);
                        var result = await _dbContext.SaveChangesAsync();
                        if (result > 0)
                        {
                            tran.Commit();
                            returns.Message = "add Roles successfully";
                            returns.Status = true;
                        }
                        else
                        {
                            returns.Message = "failed";
                            returns.Status = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    returns.Message = "add Role failed, see detail in error";
                    returns.Status = false;
                    returns.Errors = ex.ToString();
                }
            }
            returns.Data = data;
            return returns;
        }
        public async Task<DataResult<IEnumerable<Role>>> Update(IEnumerable<Role> data)
        {
            var returns = new DataResult<IEnumerable<Role>>();
            if (data != null)
            {

                using (var tran = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        await _dbContext.AddRangeAsync(data);
                        var result = await _dbContext.SaveChangesAsync();
                        if (result > 0)
                        {
                            tran.Commit();
                            returns.Message = "modify Roles successfully";
                            returns.Status = true;
                        }
                        else
                        {
                            tran.Rollback();
                            returns.Message = "modify Roles failed";
                            returns.Status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        returns.Message = "modify Roles failed, see detail in errors";
                        returns.Errors = ex.ToString();
                        returns.Status = false;
                    }
                }
            }
            returns.Data = data;
            return returns;
        }

    }

}


