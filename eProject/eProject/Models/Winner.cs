using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.Models
{
    [Table("Winner")]
    public class Winner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]      
        public int AuctionId { get; set; }
        public int WinnerId { get; set; } 
        public bool IsCheckOut { get; set; }
    }
}
