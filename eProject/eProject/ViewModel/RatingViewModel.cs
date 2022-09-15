using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using eProject.Models;
namespace eProject.ViewModel
{
    public class RatingViewModel
    {
        public User Receiver { get; set; }
        public Auction Auction { get; set; }
        public Rating Rating { get; set; }
        public User Reviewer { get; set; }
    }
}
