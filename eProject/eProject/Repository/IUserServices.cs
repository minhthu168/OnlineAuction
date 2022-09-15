using eProject.Models;
using eProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.Repository
{
   public interface IUserServices
    {
        //list of all user except locked account
        List<User> GetUsers();
        ////list of all locked account
        List<User> LockedAccount();
        //Login
        User CheckLogin(string user, string pass);
       //find user by id
        User GetUser(int id);
        //Register
        bool Add(User newUser);
        //edit information user
        bool Edit(User user);
        //change password
        bool EditPass(ResetPassword resetPassword);
        //Approval from user to admin
        bool EditRole(int id);
        //lock account
        bool AccountLock(int id);
        //unlock account
        bool unLock(int id);
        //delete user
        bool Delete(int id);
    }
}
