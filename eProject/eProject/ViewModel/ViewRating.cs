using eProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.ViewModel
{
    public class ViewRating
    {
        public Auction Auction { get; set; }
        public Rating Rating { get; set; }
        public User User { get; set; }
    }
}
