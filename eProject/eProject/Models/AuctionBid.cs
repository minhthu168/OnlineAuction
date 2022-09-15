using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eProject.Models
{
    [Table("AuctionBid")]
    public class AuctionBid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Bid ID")]
        public int BidId { get; set; }
        public int BidAmount { get; set; }
        public DateTime Time { get; set; }
        public int AuctionId { get; set; }
        public int UserId { get; set; }
        
    }
}
