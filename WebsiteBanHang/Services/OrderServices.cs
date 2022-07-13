using Microsoft.EntityFrameworkCore;
using MilkTeaShop.Helper;
using MilkTeaShop.Models;

namespace MilkTeaShop.Services
{
    public interface IOrder
    {
        public Task<DataResult<IEnumerable<Order>>> GetList();
        public Task<DataResult<Order>> Get(int? id = null);
        public Task<DataResult<IEnumerable<Order>>> Delete(int[]? ids);
        public Task<DataResult<IEnumerable<Order>>> Update(IEnumerable<Order> data);
        public Task<DataResult<IEnumerable<Order>>> Add(IEnumerable<Order> data);
    }
    public class OrderServices : IOrder
    {
        private readonly MilkTeaShopContext _dbContext;
        public OrderServices(MilkTeaShopContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<DataResult<IEnumerable<Order>>> GetList()
        {
            var result = new DataResult<IEnumerable<Order>>();
            try
            {
                result.Data = await _dbContext.Orders
                                    .IgnoreAutoIncludes()
                                    .ToListAsync();
                if (result.Data.Count() > 0)
                {
                    result.Status = true;
                    result.Message = "get Order list successfully";
                }
                else
                {
                    result.Status = false;
                    result.Message = "Order table is empty";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "get Order list failed, see detail in errors";
                result.Errors = ex.ToString();
            }
            return result;
        }
        public async Task<DataResult<Order>> Get(int? id = null)
        {
            try
            {
                var query = _dbContext.Orders.AsQueryable();

                if (id != null)
                    query = query.Where(x => x.Id == id).IgnoreAutoIncludes();
                var response = await query
                            //.Include(x => x.OrderItems)
                            .FirstOrDefaultAsync();
                return response != null ?
                 new DataResult<Order>("get Order by id  successully", true, response) :
                 new DataResult<Order>("get Order width id = " + id + " failed", false, null);
            }
            catch (Exception ex)
            {
                return new DataResult<Order>("get Order width id = " + id + " failed, see detail in error list", false, null, ex.ToString());
            }

        }
        public async Task<DataResult<IEnumerable<Order>>> Delete(int[]? ids)
        {
            var returns = new DataResult<IEnumerable<Order>>();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (ids != null && ids.Length > 0)
                    {
                        var data = await _dbContext.Orders.Where(x => ids.Contains(x.Id)).ToListAsync();
                        if (data != null && data.Count > 0)
                        {
                            _dbContext.RemoveRange(data);
                            await _dbContext.SaveChangesAsync();
                            tran.Commit();
                            returns.Data = data;
                            returns.Message = "delete Order by id successfully";
                            returns.Status = true;
                        }
                        else
                        {
                            tran.Rollback();
                            returns.Message = "get list Order for delete failed";
                            returns.Status = false;
                        }

                        return returns;
                    }

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    returns.Message = "delete Order width id failed, see detail in error";
                    returns.Status = false;
                    returns.Errors = ex.ToString();
                }
            }
            return returns;
        }
        public async Task<DataResult<IEnumerable<Order>>> Add(IEnumerable<Order> data)
        {
            var returns = new DataResult<IEnumerable<Order>>();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (data != null)
                    {

                        _dbContext.AddRange(data);
                        var result = await _dbContext.SaveChangesAsync();
                        if (result > 0)
                        {
                            tran.Commit();
                            returns.Message = "add Orders successfully";
                            returns.Status = true;
                        }
                        else
                        {
                            tran.Rollback();
                            returns.Message = "failed";
                            returns.Status = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    returns.Message = "add Order failed, see detail in error";
                    returns.Status = false;
                    returns.Errors = ex.ToString();
                }
            }
            returns.Data = data;
            return returns;
        }
        public async Task<DataResult<IEnumerable<Order>>> Update(IEnumerable<Order> data)
        {
            var returns = new DataResult<IEnumerable<Order>>();
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
                            returns.Message = "modify Orders successfully";
                            returns.Status = true;
                        }
                        else
                        {
                            tran.Rollback();
                            returns.Message = "modify Orders failed";
                            returns.Status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        returns.Message = "modify Orders failed, see detail in errors";
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


