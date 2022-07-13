using MilkTeaShop.Helper;
using MilkTeaShop.Models;
namespace MilkTeaShop.Services;
public interface IUser
{
    public Task<DataResult<IEnumerable<User>>> GetList();
    public Task<DataResult<User>> Get( int? id = null, string username = "", string email = "");
    public Task<DataResult<IEnumerable<User>>> Delete( int[]? ids);
    public Task<DataResult<IEnumerable<User>>> Update(IEnumerable<User> data);
    public Task<DataResult<IEnumerable<User>>> Add(IEnumerable<User> data);
}
