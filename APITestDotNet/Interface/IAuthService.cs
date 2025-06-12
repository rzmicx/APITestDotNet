using System.Threading.Tasks;
using APITestDotNet.Data.Models;

namespace APITestDotNet.Interfaces
{
    public interface IAuthService
    {
        string Login(string username, string password);
        Task<string> Register(string username, string password);
        string GenerateJwtToken(string username);
    }
}
