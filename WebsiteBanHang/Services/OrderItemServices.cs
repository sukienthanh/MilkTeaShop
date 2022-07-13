using Microsoft.EntityFrameworkCore;
using MilkTeaShop.Helper;
using MilkTeaShop.Models;

namespace MilkTeaShop.Services
{
    public interface IOrderItem
    {
        public Task<DataResult<IEnumerable<OrderItem>>> GetList();
        public Task<DataResult<OrderItem>> Get(int? id = null);
        public Task<DataResult<IEnumerable<OrderItem>>> Delete(int[]? ids);
        public Task<DataResult<IEnumerable<OrderItem>>> Update(IEnumerable<OrderItem> data);
        public Task<DataResult<IEnumerable<OrderItem>>> Add(IEnumerable<OrderItem> data);
    }
    public class OrderItemServices : IOrderItem
    {
        private readonly MilkTeaShopContext _dbContext;
        public OrderItemServices(MilkTeaShopContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<DataResult<IEnumerable<OrderItem>>> GetList()
        {
            var result = new DataResult<IEnumerable<OrderItem>>();
            try
            {
                result.Data = (IEnumerable<OrderItem>)await _dbContext.OrderItems
                    .Include(x=>x.Product)
                    .Include(x=>x.Order)
                    .ToListAsync();
                        
                if (result.Data.Count() > 0)
                {
                    result.Status = true;
                    result.Message = "get OrderItem list successfully";
                }
                else
                {
                    result.Status = false;
                    result.Message = "OrderItem table is empty";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "get OrderItem list failed, see detail in errors";
                result.Errors = ex.ToString();
            }
            return result;
        }
        public async Task<DataResult<OrderItem>> Get(int? id = null)
        {
            try
            {
                var query = _dbContext.OrderItems.AsQueryable();

                if (id != null)
                    query = query.Where(x => x.Id == id)
                            .Include(x => x.Product)
                            .Include(x => x.Order);
                var response = await query                            
                            .FirstOrDefaultAsync();
                return response != null ?
                 new DataResult<OrderItem>("get OrderItem by id  successully", true, response) :
                 new DataResult<OrderItem>("get OrderItem width id = " + id + " failed", false, null);
            }
            catch (Exception ex)
            {
                return new DataResult<OrderItem>("get OrderItem width id = " + id + " failed, see detail in error list", false, null, ex.ToString());
            }

        }
        public async Task<DataResult<IEnumerable<OrderItem>>> Delete(int[]? ids)
        {
            var returns = new DataResult<IEnumerable<OrderItem>>();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (ids != null && ids.Length > 0)
                    {
                        var data = await _dbContext.OrderItems
                                .Where(x => ids.Contains(x.Id))
                                .ToListAsync();
                        if (data != null && data.Count > 0)
                        {
                            _dbContext.RemoveRange(data);
                            await _dbContext.SaveChangesAsync();
                            tran.Commit();
                            returns.Data = data;
                            returns.Message = "delete OrderItem by id successfully";
                            returns.Status = true;
                        }
                        else
                        {
                            tran.Rollback();
                            returns.Message = "get list OrderItem for delete failed";
                            returns.Status = false;
                        }

                        return returns;
                    }

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    returns.Message = "delete OrderItem width id failed, see detail in error";
                    returns.Status = false;
                    returns.Errors = ex.ToString();
                }
            }
            return returns;
        }
        public async Task<DataResult<IEnumerable<OrderItem>>> Add(IEnumerable<OrderItem> data)
        {
            var returns = new DataResult<IEnumerable<OrderItem>>();
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
                            returns.Message = "add OrderItems successfully";
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
                    returns.Message = "add OrderItem failed, see detail in error";
                    returns.Status = false;
                    returns.Errors = ex.ToString();
                }
            }
            returns.Data = data;
            return returns;
        }
        public async Task<DataResult<IEnumerable<OrderItem>>> Update(IEnumerable<OrderItem> data)
        {
            var returns = new DataResult<IEnumerable<OrderItem>>();
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
                            returns.Message = "modify OrderItems successfully";
                            returns.Status = true;
                        }
                        else
                        {
                            tran.Rollback();
                            returns.Message = "modify OrderItems failed";
                            returns.Status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        returns.Message = "modify OrderItems failed, see detail in errors";
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


