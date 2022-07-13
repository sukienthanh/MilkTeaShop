using Microsoft.EntityFrameworkCore;
using MilkTeaShop.Helper;
using MilkTeaShop.Models;

namespace MilkTeaShop.Services
{
    public interface IPayment
    {
        public Task<DataResult<IEnumerable<Payment>>> GetList();
        public Task<DataResult<Payment>> Get(int? id = null);
        public Task<DataResult<IEnumerable<Payment>>> Delete(int[]? ids);
        public Task<DataResult<IEnumerable<Payment>>> Update(IEnumerable<Payment> data);
        public Task<DataResult<IEnumerable<Payment>>> Add(IEnumerable<Payment> data);
    }
    public class PaymentServices : IPayment
    {
        private readonly MilkTeaShopContext _dbContext;
        public PaymentServices(MilkTeaShopContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<DataResult<IEnumerable<Payment>>> GetList()
        {
            var result = new DataResult<IEnumerable<Payment>>();
            try
            {
                result.Data = await _dbContext.Payments.ToListAsync();
                if (result.Data.Count() > 0)
                {
                    result.Status = true;
                    result.Message = "get Payment list successfully";
                }
                else
                {
                    result.Status = false;
                    result.Message = "Payment table is empty";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "get Payment list failed, see detail in errors";
                result.Errors = ex.ToString();
            }
            return result;
        }
        public async Task<DataResult<Payment>> Get(int? id = null)
        {
            try
            {
                var query = _dbContext.Payments.AsQueryable();

                if (id != null)
                    query = query.Where(x => x.Id == id);
                var response = await query.FirstOrDefaultAsync();
                return response != null ?
                 new DataResult<Payment>("get Payment by id  successully", true, response) :
                 new DataResult<Payment>("get Payment width id = " + id + " failed", false, null);
            }
            catch (Exception ex)
            {
                return new DataResult<Payment>("get Payment width id = " + id + " failed, see detail in error list", false, null, ex.ToString());
            }

        }
        public async Task<DataResult<IEnumerable<Payment>>> Delete(int[]? ids)
        {
            var returns = new DataResult<IEnumerable<Payment>>();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (ids != null && ids.Length > 0)
                    {
                        var data = await _dbContext.Payments.Where(x => ids.Contains(x.Id)).ToListAsync();
                        if (data != null && data.Count > 0)
                        {
                            _dbContext.RemoveRange(data);
                            await _dbContext.SaveChangesAsync();
                            tran.Commit();
                            returns.Data = data;
                            returns.Message = "delete Payment by id successfully";
                            returns.Status = true;
                        }
                        else
                        {
                            returns.Message = "get list Payment for delete failed";
                            returns.Status = false;
                        }

                        return returns;
                    }

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    returns.Message = "delete Payment width id failed, see detail in error";
                    returns.Status = false;
                    returns.Errors = ex.ToString();
                }
            }
            return returns;
        }
        public async Task<DataResult<IEnumerable<Payment>>> Add(IEnumerable<Payment> data)
        {
            var returns = new DataResult<IEnumerable<Payment>>();
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
                            returns.Message = "add Payments successfully";
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
                    returns.Message = "add Payment failed, see detail in error";
                    returns.Status = false;
                    returns.Errors = ex.ToString();
                }
            }
            returns.Data = data;
            return returns;
        }
        public async Task<DataResult<IEnumerable<Payment>>> Update(IEnumerable<Payment> data)
        {
            var returns = new DataResult<IEnumerable<Payment>>();
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
                            returns.Message = "modify Payments successfully";
                            returns.Status = true;
                        }
                        else
                        {
                            tran.Rollback();
                            returns.Message = "modify Payments failed";
                            returns.Status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        returns.Message = "modify Payments failed, see detail in errors";
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


