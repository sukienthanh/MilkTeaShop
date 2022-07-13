using Microsoft.EntityFrameworkCore;
using MilkTeaShop.Helper;
using MilkTeaShop.Models;

namespace MilkTeaShop.Services
{
    public interface IAbout
    {
        public Task<DataResult<IEnumerable<About>>> GetList();
        public Task<DataResult<About>> Get(int? id = null);
        public Task<DataResult<IEnumerable<About>>> Delete(int[]? ids);
        public Task<DataResult<IEnumerable<About>>> Update(IEnumerable<About> data);
        public Task<DataResult<IEnumerable<About>>> Add(IEnumerable<About> data);
    }
    public class AboutServices : IAbout
    {
        private readonly MilkTeaShopContext _dbContext;
        public AboutServices(MilkTeaShopContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<DataResult<IEnumerable<About>>> GetList()
        {
            var result = new DataResult<IEnumerable<About>>();
            try
            {
                result.Data = await _dbContext.Abouts.ToListAsync();
                if (result.Data.Count() > 0)
                {
                    result.Status = true;
                    result.Message = "get About list successfully";
                }
                else
                {
                    result.Status = false;
                    result.Message = "About table is empty";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "get About list failed, see detail in errors";
                result.Errors = ex.ToString();
            }
            return result;
        }
        public async Task<DataResult<About>> Get(int? id = null)
        {
            try
            {
                var query = _dbContext.Abouts.AsQueryable();

                if (id != null)
                    query = query.Where(x => x.Id == id);
                var response = await query.FirstOrDefaultAsync();
                return response != null ?
                 new DataResult<About>("get About by id  successully", true, response) :
                 new DataResult<About>("get About width id = " + id + " failed", false, null);
            }
            catch (Exception ex)
            {
                return new DataResult<About>("get About width id = " + id + " failed, see detail in error list", false, null, ex.ToString());
            }

        }
        public async Task<DataResult<IEnumerable<About>>> Delete(int[]? ids)
        {
            var returns = new DataResult<IEnumerable<About>>();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (ids != null && ids.Length > 0)
                    {
                        var data = await _dbContext.Abouts.Where(x => ids.Contains(x.Id)).ToListAsync();
                        if (data != null && data.Count > 0)
                        {
                            _dbContext.RemoveRange(data);
                            await _dbContext.SaveChangesAsync();
                            tran.Commit();
                            returns.Data = data;
                            returns.Message = "delete About by id successfully";
                            returns.Status = true;
                        }
                        else
                        {
                            returns.Message = "get list About for delete failed";
                            returns.Status = false;
                        }

                        return returns;
                    }

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    returns.Message = "delete About width id failed, see detail in error";
                    returns.Status = false;
                    returns.Errors = ex.ToString();
                }
            }
            return returns;
        }
        public async Task<DataResult<IEnumerable<About>>> Add(IEnumerable<About> data)
        {
            var returns = new DataResult<IEnumerable<About>>();
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
                            returns.Message = "add Abouts successfully";
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
                    returns.Message = "add About failed, see detail in error";
                    returns.Status = false;
                    returns.Errors = ex.ToString();
                }
            }
            returns.Data = data;
            return returns;
        }
        public async Task<DataResult<IEnumerable<About>>> Update(IEnumerable<About> data)
        {
            var returns = new DataResult<IEnumerable<About>>();
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
                            returns.Message = "modify Abouts successfully";
                            returns.Status = true;
                        }
                        else
                        {
                            tran.Rollback();
                            returns.Message = "modify Abouts failed";
                            returns.Status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        returns.Message = "modify Abouts failed, see detail in errors";
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


