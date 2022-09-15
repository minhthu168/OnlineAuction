using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eProject.Models
{
    [Table("Rating")]
    public class Rating
    {    
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RatingId { get; set; }       
        public int AuctionId { get; set; }//fk        
        public int ReceiverId { get; set; }
        public int ReviewerId { get; set; }
        [Required]
        public int Star { get; set; }
        public string Comment { get; set; }
        public DateTime CreateAt { get; set; }

        
    }
}
