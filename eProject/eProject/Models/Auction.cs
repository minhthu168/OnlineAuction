using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.Models
{
    [Table("Auction")]
    public class Auction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuctionId { get; set; } 
        [Required]
        public string Title { get; set; } //ten sp 
        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }
        public string Image { get; set; } //anh sp
        public string Document { get; set; } //tai lieu dinh kem
        public string Condition { get; set; }//rare/used/new
        public int CategoryId { get; set; }//loai sp
        public int UserId { get; set; }//nguoi ban(nguoi tao)
        [Required]
        public DateTime StartDate { get; set; } //ngay bat dau
         [Required]                                       
        public DateTime EndDate { get; set; }    //ngay ket thuc dau thau
          [Required]                                      
        public int MinimumBid { get; set; } //gia dau thau ban dau

        public int SalePrice { get; set; } //gia ban
        [Required]
        public int BidIncremenent { get; set; } //muc tang gia thau
        public string Status { get; set; } //Active or Inactive      
    }
}
