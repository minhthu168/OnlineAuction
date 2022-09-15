using eProject.Models;
using eProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.Service
{
    public class UserServices : Repository.IUserServices
    {
        private Data.DatabaseContext context;
        public UserServices(Data.DatabaseContext _context)
        {
            context = _context;
        }

        public bool Add(User newUser)
        {           
                newUser.Role = "user";
                newUser.Status = true;
                context.Users.Add(newUser);
                context.SaveChanges();
                return true;            
        }

        public User CheckLogin(string user, string pass)
        {
            var account = context.Users.SingleOrDefault(a => a.Email.Equals(user) && a.Password.Equals(pass) && a.Status==true);
            if (account != null)
            {
                return account;
            }
            else
            {
                return null;
            }
        }

        public bool Delete(int id)
        {
            User user = context.Users.SingleOrDefault(a => a.UserId.Equals(id));
            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Edit(User user)
        {
            var acc = context.Users.SingleOrDefault(a => a.UserId.Equals(user.UserId));
            if (acc != null)
            {
                acc.Username = user.Username;            
                acc.Phone = user.Phone;
                acc.Address = user.Address;
                acc.Email = user.Email;
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EditPass(ResetPassword resetPassword)
        {       
            var acc = context.Users.SingleOrDefault(a => a.Email.Equals(resetPassword.Email));
            if (acc != null)
            {
                acc.Password = resetPassword.Password;              
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public  bool EditRole(int id)
        {
            User account = context.Users.SingleOrDefault(a => a.UserId.Equals(id));
            if (account != null)
            {
                account.Role = "admin";
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public User GetUser(int id)
        {
            User account = context.Users.SingleOrDefault(a => a.UserId.Equals(id) && a.Status==true);
            if (account != null)
            {
                return account;
            }
            else
            {
                return null;
            }
        }

        public List<User> GetUsers()
        {
            return context.Users.Where(a=>a.Status==true).ToList();
        }

        public bool AccountLock(int id)
        {
            User account = context.Users.SingleOrDefault(a => a.UserId.Equals(id));
            if (account != null)
            {
                account.Status = false;
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<User> LockedAccount()
        {
            return context.Users.Where(a => a.Status == false).ToList();
        }

        public bool unLock(int id)
        {
            User account = context.Users.SingleOrDefault(a => a.UserId.Equals(id));
            if (account != null)
            {
                account.Status = true;
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
