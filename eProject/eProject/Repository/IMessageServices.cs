using eProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.Repository
{
   public interface IMessageServices
    {

        void SendMail(Message message, string ToEmail);
        void AddMail(Message message);
    }
}
