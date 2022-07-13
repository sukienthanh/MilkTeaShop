using Microsoft.EntityFrameworkCore;
using MilkTeaShop.Helper;
using MilkTeaShop.Models;

namespace MilkTeaShop.Services
{
    public interface ICatalog
    {
        public Task<DataResult<IEnumerable<Catalog>>> GetList();
        public Task<DataResult<Catalog>> Get(int? id = null);
        public Task<DataResult<IEnumerable<Catalog>>> Delete(int[]? id);
        public Task<DataResult<IEnumerable<Catalog>>> Update(IEnumerable<Catalog> data);
        public Task<DataResult<IEnumerable<Catalog>>> Add(IEnumerable<Catalog> data);
    }
    public class CatalogServices : ICatalog
    {
        private readonly MilkTeaShopContext _dbContext;
        public CatalogServices(MilkTeaShopContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<DataResult<IEnumerable<Catalog>>> GetList()
        {
            var result = new DataResult<IEnumerable<Catalog>>();
            
             try
             {
                result.Data = await _dbContext.Catalogs.ToListAsync();
                if (result.Data.Count() > 0)
                {
                    result.Status = true;
                    result.Message = "get catalog list successfully";
                }
                else
                {
                    result.Status = false;
                    result.Message = "catalog table is empty";
                }
             }
             catch (Exception ex)
             {
                result.Status = false;
                result.Message = "get catalog list failed, see detail in errors";
                result.Errors = ex.ToString();
             }
             return result;
        }
        public async Task<DataResult<Catalog>> Get(int? id = null)
        {
            try
            {
                var query = _dbContext.Catalogs.AsQueryable();

                if (id != null)
                    query = query.Where(x => x.CataId == id);
                var response = await query.FirstOrDefaultAsync();
                return response != null ?
                 new DataResult<Catalog>("get catalog by id  successully", true, response) :
                 new DataResult<Catalog>("get Catalog width id = " + id + " failed", false, null);
            }
            catch (Exception ex)
            {
                return new DataResult<Catalog>("get catalog width id = " + id + " failed, see detail in error list", false, null, ex.ToString());
            }

        }
        public async Task<DataResult<IEnumerable<Catalog>>> Delete(int[]? id)
        {
            var returns = new DataResult<IEnumerable<Catalog>>();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (id != null)
                    {
                        var data = await  _dbContext.Catalogs.Where(x => id.Contains(x.CataId)).ToListAsync();

                        if (data != null)
                        {                           
                            _dbContext.RemoveRange(data);
                            await _dbContext.SaveChangesAsync();
                            tran.Commit();
                            returns.Data = data;
                            returns.Message = "delete catalog by id successfully";
                            returns.Status = true;
                        }
                        else
                        {                            
                            returns.Message = "get list catalog for delete failed";
                            returns.Status = false;
                        }

                        return returns;
                    }

                }
                catch (Exception ex)
                {
                    tran.Rollback();

                    returns.Message = "delete catalog width id failed, see detail in error";
                    returns.Status = false;
                    returns.Errors = ex.ToString();
                }
            }
            
            return returns;
        }
        public async Task<DataResult<IEnumerable<Catalog>>> Add(IEnumerable<Catalog> data)
        {
            var returns = new DataResult<IEnumerable<Catalog>>();
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
                            returns.Message = "add Catalogs successfully";
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
                    returns.Message = "add Catalog failed, see detail in error";
                    returns.Status = false;
                    returns.Errors = ex.ToString();
                }
            }
            returns.Data = data;
            return returns;
        }
        public async Task<DataResult<IEnumerable<Catalog>>> Update(IEnumerable<Catalog> data)
        {
            var returns = new DataResult<IEnumerable<Catalog>>();
            if (data != null)
            {

                using (var tran = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                         _dbContext.UpdateRange(data);                       
                                               
                        var result = await _dbContext.SaveChangesAsync();                       

                        if (result > 0)
                        {
                            tran.Commit();
                            returns.Message = "modify Catalogs successfully";
                            returns.Status = true;
                        }
                        else
                        {
                            tran.Rollback();
                            returns.Message = "modify Catalogs failed";
                            returns.Status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        returns.Message = "modify Catalogs failed, see detail in errors";
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


