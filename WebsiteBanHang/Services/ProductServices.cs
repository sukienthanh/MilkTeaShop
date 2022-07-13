
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MilkTeaShop.Helper;
using MilkTeaShop.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MilkTeaShop.Services
{
    public interface IProduct
    {
        public Task<DataResult<IEnumerable<Product>>> GetList();
        public Task<DataResult<Product>> Get(int? id = null);
        public Task<DataResult<IEnumerable<Product>>> Delete(int[]? ids);
        public Task<DataResult<IEnumerable<Product>>> Update(IEnumerable<Product> data);
        public Task<DataResult<IEnumerable<Product>>> Add(IEnumerable<Product> data);
    }
    public class ProductServices : IProduct
    {
        private readonly MilkTeaShopContext _dbContext;
        public ProductServices(MilkTeaShopContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<DataResult<IEnumerable<Product>>> GetList()
        {
            var result = new DataResult<IEnumerable<Product>>();
            try
            {
                result.Data = await _dbContext.Products
                    .IgnoreAutoIncludes()
                    .ToListAsync();
                if (result.Data.Count() > 0)
                {
                    result.Status = true;
                    result.Message = "get product list successfully";
                }
                else
                {
                    result.Status = false;
                    result.Message = "product table is empty";
                }
            }catch (Exception ex)
            {
                result.Status = false;
                result.Message = "get product list failed, see detail in errors";
                result.Errors = ex.ToString(); 
            }
            return result;
        }
        public async Task<DataResult<Product>> Get(int? id = null)
        {
            try
            {
                var query = _dbContext.Products.AsQueryable();

                if (id != null)
                    query = query.Where(x => x.ProductId == id).IgnoreAutoIncludes();
                var response = await query.FirstOrDefaultAsync();
                return response != null ?
                 new DataResult<Product>("get product by id  successully", true, response) :
                 new DataResult<Product>("get product width id = " + id + " failed", false, null);
            }
            catch (Exception ex)
            {
                return new DataResult<Product>("get product width id = " + id + " failed, see detail in error list", false,null, ex.ToString());
            }
            
        }
        public async Task<DataResult<IEnumerable<Product>>> Delete(int[]? ids)
        {
            var returns = new DataResult<IEnumerable<Product>>();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (ids != null && ids.Length > 0)
                    {
                        var data = await _dbContext.Products.Where(x => ids.Contains(x.ProductId)).ToListAsync();
                        if (data != null && data.Count > 0)
                        {
                            _dbContext.RemoveRange(data);
                            await _dbContext.SaveChangesAsync();
                            tran.Commit();
                            returns.Data = data;
                            returns.Message = "delete product by id successfully";
                            returns.Status = true;
                        }
                        else
                        {
                            returns.Message = "get list product for delete failed";
                            returns.Status = false;
                        }

                        return returns;
                    }

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    returns.Message = "delete prduct width id failed, see detail in error";
                    returns.Status = false;
                    returns.Errors = ex.ToString();
                }
            }
            return returns;
        }
        public async Task<DataResult<IEnumerable<Product>>> Add(IEnumerable<Product> data)
        {
            var returns = new DataResult<IEnumerable<Product>>();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (data != null){

                        _dbContext.UpdateRange(data);
                        var result = await _dbContext.SaveChangesAsync();
                        if (result > 0)
                        {                            
                            tran.Commit();
                            returns.Message = "add products successfully";
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
                    returns.Message = "add product failed, see detail in error";
                    returns.Status = false;
                    returns.Errors = ex.ToString();
                }
            }
            returns.Data = data;
            return returns;
        }
        public async Task<DataResult<IEnumerable<Product>>> Update(IEnumerable<Product> data)
        {
            var returns = new DataResult<IEnumerable<Product>>();
            if (data != null)            {
                
                using (var tran = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                         _dbContext.UpdateRange(data);
                        var result = await _dbContext.SaveChangesAsync();
                        if (result > 0)
                        {
                            tran.Commit();
                            returns.Message = "modify products successfully";
                            returns.Status = true;
                        }
                        else
                        {
                            tran.Rollback();
                            returns.Message = "modify products failed";
                            returns.Status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        returns.Message = "modify products failed, see detail in errors";
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


