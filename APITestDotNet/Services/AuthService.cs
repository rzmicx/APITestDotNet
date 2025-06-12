using APITestDotNet.Data.Common;
using APITestDotNet.Data.Models;
using  APITestDotNet.Interfaces;
using APITestDotNet.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace TestMandiri.Services
{
    public class AuthService : IAuthService
    {
        private readonly TestDotNetContext _db;
        private readonly IConnectionMultiplexer _redis;
        private readonly IConfiguration _config;

        public AuthService(TestDotNetContext db, IConnectionMultiplexer redis, IConfiguration config)
        {
            _db = db;
            _redis = redis;
            _config = config;
        }

        public string Login(string username, string password)
        {
            try { 
            var user = _db.Msusers.Where(u => u.Username == username).FirstOrDefault();

            if (user == null)
                return "User tidak ditemukan.";

            if (!user.Active.Value)
                return "User nonaktif.";

            var redisDb = _redis.GetDatabase();
            var redisKey = $"loginAttempts:{username}";

            int loginAttempts = (int)redisDb.StringGet(redisKey);

            if (loginAttempts >= 3)
            {
                user.Active = false;
                _db.SaveChanges();
                return "Akun Anda dinonaktifkan setelah 3 percobaan login gagal.";
            }

            var decryptor = AESHelper.Decrypt(user.Passcode);

            if (! password.SequenceEqual(decryptor))
            {
                redisDb.StringIncrement(redisKey);
                return "Username atau password salah.";
            }

            redisDb.KeyDelete(redisKey);  
            return "Login berhasil.";
        }catch (Exception ex)
            {
                LoggerHelper.LogError(nameof(Login), ex);
                return "login gagal tolong hubungi support";
            }
        }

        public async Task<string> Register(string username, string password)
        {
           
                var exists = _db.Msusers.Any(u => u.Username == username);
                if (exists)
                   return ("Username sudah digunakan.");

                var encrypted = AESHelper.Encrypt(password);

                var user = new Msuser
                {
                    Username = username,
                    Passcode = encrypted,
                    Active = true
                };

                _db.Msusers.Add(user);
             await   _db.SaveChangesAsync();
            return "ok";
            }

        public string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }




    }
}
