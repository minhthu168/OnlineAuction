using eProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }
        public virtual DbSet<User> Users { get; set; }
        public  DbSet<Category> Categories { get; set; }      
        public virtual DbSet<Auction> Auctions { get; set; }
        public virtual DbSet<Winner> Winners { get; set; }
        public virtual DbSet<AuctionBid> AuctionBids { get; set; }
        public virtual DbSet<Message> Messages{ get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }

    }
}
