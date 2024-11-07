using Microsoft.DotNet.Scaffolding.Shared.Project;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vehicle_World.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        public string Buyer_Id { get; set; }

        [ForeignKey("Buyer_Id")]
        public virtual AppUser Buyer { get; set; }

        //public int CarId { get; set; }

        //[ForeignKey("CarId")]
        //public virtual CarDetail Car { get; set; }

        [Required]
        public int Rating { get; set; } // 1 to 5

        [Required]
        public string Comment { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }
    }

}
