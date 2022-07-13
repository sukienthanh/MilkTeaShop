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
    public class UserServices : IUser
    {
        private readonly MilkTeaShopContext _dbContext;
        public UserServices(MilkTeaShopContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<DataResult<IEnumerable<User>>> GetList()
        {
            var result = new DataResult<IEnumerable<User>>();           
            
            result.Data = await _dbContext.Users.ToListAsync();
            if (result.Data.Count() > 0)
            {
                result.Status = true;
                result.Message = "successfully";
            }
            else
            {
                result.Status = false;
                result.Message = "failed";
            }
            return result;
        }
        public async Task<DataResult<User>> Get(int? id = null, string? username = "", string? email = "")
        {
            var query = _dbContext.Users.AsQueryable();
           
            if (id != null)
                 query   = query.Where(x => x.UserId == id);
            if (!String.IsNullOrEmpty(username))
                query = query.Where(x => x.UserName != null && x.UserName.Contains(username));
            if (!String.IsNullOrEmpty(email))
                query = query.Where(x => x.Email != null && x.Email.Contains(email));
            var response = await query.Include(x => x.Role).ToListAsync();
               
            return response.Count() > 0 ?
                 new DataResult<User>("get user successully", true, response.FirstOrDefault()) :
                 new DataResult<User>("get user failed", false, null);
        }
        public async Task<DataResult<IEnumerable<User>>> Delete(int[]? ids)
        {
            var returns = new DataResult<IEnumerable<User>>();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {                                        
                    if (ids != null && ids.Length > 0){                                               
                        var data = await _dbContext.Users.Where(x => ids.Contains(x.UserId)).ToListAsync();
                        if(data != null && data.Count > 0)
                        {
                            _dbContext.RemoveRange(data);
                            await _dbContext.SaveChangesAsync();
                            tran.Commit();
                            returns.Data = data;
                            returns.Message = "successfully";
                            returns.Status = true;
                        }
                        else
                        {
                            returns.Message = "Get list user for delete failed";
                            returns.Status = false;
                        }
                        
                        return returns;
                    }

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    returns.Message = ex.ToString();
                    returns.Status = false;
                }
            }
            return returns;
        }
        public async Task<DataResult<IEnumerable<User>>> Add(IEnumerable<User> data)
        {
            var returns = new DataResult<IEnumerable<User>>();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (data != null)
                    {
                        var length = data.Count();
                        string[] userNames  = new string[length];
                        foreach(var item in data)
                        {
                            var i = 0;
                            userNames[i++] = item.UserName;
                        }
                        
                        _dbContext.UpdateRange(data);
                        var result = await _dbContext.SaveChangesAsync();
                        if(result > 0)
                        {
                            
                            returns.Data = await _dbContext.Users                                
                                .Where(x => userNames.Contains(x.UserName))
                                .Include( x => x.Role)
                                .ToListAsync();
                            tran.Commit();
                            returns.Message = "successfully";
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
                    returns.Message = ex.ToString();
                    returns.Status = false;
                }
            }
            returns.Data = data;
            return returns;
        }
        public async Task<DataResult<IEnumerable<User>>> Update (IEnumerable<User> data)
        {
            var returns = new DataResult<IEnumerable<User>>();
            if (data != null)            
            {   
                using (var tran = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var length = data.Count();
                        string[] userNames = new string[length];
                        foreach (var item in data)
                        {
                            var i = 0;
                            userNames[i++] = item.UserName;
                        }
                        _dbContext.UpdateRange(data);
                        var result = await _dbContext.SaveChangesAsync();
                        if (result > 0)
                        {
                            returns.Data = await _dbContext.Users
                                .Where(x => userNames.Contains(x.UserName))
                                .Include(x => x.Role)
                                .ToListAsync();
                            tran.Commit();
                            returns.Message = "successfully";
                            returns.Status = true;
                        }
                        else { 
                            tran.Rollback();
                            returns.Message = "failed";
                            returns.Status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        returns.Message = ex.ToString();
                        returns.Status = false;
                    }
                }
            }            
            return returns;
        }       
    }  
}
    

