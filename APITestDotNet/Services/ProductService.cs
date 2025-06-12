using System;
using System.Linq;
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using APITestDotNet.Data.Models;
using APITestDotNet.Data.Common;
using APITestDotNet.Interfaces;
using APITestDotNet.Services;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace APITestDotNet.Services
{
    public class ProductService : IProductService
    {
        private readonly TestDotNetContext _db;
        private readonly IConfiguration _configuration;

        public ProductService(TestDotNetContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }


        public async Task<string> AddPRoduct(string userid,Product item)
        {
            var transaksi = new Product
            {
                Name = item.Name,
                Description = item.Description,
                Price= item.Price,
                CreatedAt =DateTime.Now,
                CreateBy=userid
            };
              await  _db.Products.AddAsync(transaksi);
              await  _db.SaveChangesAsync();
            
            return "ok";
        }
        public async Task<string> EditProduct(string userid, Product item)
        {
            var data = await _db.Products.Where(x=>x.Id==item.Id).FirstOrDefaultAsync();
            if (data == null)
            {
                return "data tidak ditemukan";
            }
            if (data.CreateBy != userid) {
                return "the data your Edit Not Authorize";
            }

            data.Name = item.Name;
                 data.Description = item.Description;
                 data.Price = item.Price;
  
            await _db.SaveChangesAsync();

            return "ok";
        }

        public async Task<List<Product>> GetProducts(string type,string? orderby)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                using var connection = new SqlConnection(connectionString);

                var result = await connection.QueryAsync<Product>(
                    "sp_GridData",
                    new { type, orderby },
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(nameof(GetProducts), ex);
                throw new Exception("tolong hubungin administrator");
            }
        }






    }
}
