using System.Threading.Tasks;
using APITestDotNet.Data.Models;

namespace APITestDotNet.Interfaces
{
    public interface IProductService
    {
        Task<string> AddPRoduct(string userid, Product item);
        Task<string> EditProduct(string userid, Product item);
          Task<List<Product>> GetProducts(string type ,string? orderby);
    }
}
