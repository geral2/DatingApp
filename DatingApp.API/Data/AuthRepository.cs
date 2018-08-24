using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.UserName == username);

            if(user == null)
              return null;

            if(!VerifyPaswordHash(user, password))
             return null;

             return user;
        }

        private bool VerifyPaswordHash(User user, string password)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt))
            {
                byte[] ComputedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for(int i = 0; i < ComputedHash.Length; i++)
                {
                    if(ComputedHash[i] != user.PasswordHash[i])
                     return false;
                }
                
                return true;
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] PasswordSalt, PasswordHash;
            GeneratePasswordHash(password, out PasswordSalt, out PasswordHash);

            user.PasswordHash = PasswordHash;
            user.PasswordSalt = PasswordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void GeneratePasswordHash(string password, out byte[] passwordSalt, out byte[] passwordHash)
        {
           using(var hmac = new System.Security.Cryptography.HMACSHA512())
           {
             passwordSalt = hmac.Key;
             passwordHash = hmac.ComputeHash(System.Text.ASCIIEncoding.UTF8.GetBytes(password));
           }
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(d=>d.UserName == username))
                return true;

            return false;
        }
    }
}