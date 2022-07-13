using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkTeaShop.Helper;
using System.Text.Json;
using WebClient.ApiClient;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class CartController : Controller
    {
        private readonly IProduct _proService;
        private readonly ISession _session;
        private const string key = "cart";
        public CartController(IProduct _proService, IHttpContextAccessor httpContextAccessor)
        {
            this._proService = _proService;
            _session = httpContextAccessor.HttpContext.Session;

        }
        // GET: CartController/Details/5
        [HttpGet]
        public IActionResult GetList()
        {
            try
            {
                var result = new DataResult<List<Cart>>()
                {
                    Data = GetListMethod(),
                    Message = "get list session cart successfully",
                    Status = true
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new DataResult<List<Cart>>()
                {
                    Data = GetListMethod(),
                    Message = "get list session cart failed",
                    Status = false,
                    Errors = ex.Message
                });
            }

        }
        public List<Cart> GetListMethod()
        {
            var listCart = _session.GetString(key);
            if (listCart != null)
            {
                return JsonSerializer.Deserialize<List<Cart>>(listCart);
            }
            return new List<Cart>();            
        }

        // GET: CartController/Create
        [HttpPost]
        public ActionResult Modify([FromBody]List<Cart> data)
        {
            if (data != null && data.Count>0)
            {
                try
                {
                    var listCart = GetListMethod();

                    //copy list cart 
                    var listCopy = new List<Cart>();
                    foreach(var cart in listCart)
                    {
                        listCopy.Add(cart);
                    }
                    if (listCart.Count > 0)
                    {
                        var isExisted = false;
                        foreach (var product in data)
                        {                            
                            if (product.Quantity > 0)
                            {                               
                                foreach (var item in listCopy)
                                {
                                    if (product.Product != null && item.Product != null)
                                    {
                                        if (item.Product.ProductId == product.Product.ProductId)
                                        { //already exsited                                                                                      
                                            item.Quantity = product.Quantity;
                                            isExisted = true;
                                        }
                                    }
                                }
                                if(!isExisted) 
                                    listCart.Add(product); //add new
                            }
                            else listCart.Remove(product);
                        }
                    }                    
                    else
                    {//add new
                        foreach(var item in data)
                        {
                            if (item.Quantity != 0)
                                listCart.Add(item);
                        }                        
                    }
                    //change to json and save to session
                    SaveToSession(listCart);

                    var result = new DataResult<List<Cart>>()
                    {
                        Data = listCart,
                        Message = "update session cart successfully",
                        Status = true
                    };
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return Ok(new DataResult<List<Cart>>()
                    {
                        Data = data,
                        Message = "update session cart failed",
                        Status = false,
                        Errors = ex.Message
                    });
                }
            }
            return BadRequest();
        }
        //save new product to session card
        public void SaveToSession(List<Cart> data)
        {            
            _session.SetString(key,JsonSerializer.Serialize(data));           
            
        }
        [HttpDelete]
        public IActionResult Delete()
        {
            try
            {
                _session.Remove(key);
                return Ok(new DataResult<string>()
                {
                    Data = null,
                    Message = "remove session cart successfully",
                    Status = true                    
                });
            }
            catch(Exception ex)
            {
                return Ok(new DataResult<string>()
                {
                    Data = null,
                    Message = "remove session cart failed",
                    Status = false,
                    Errors = ex.Message
                });
            }
        }
    }
}
