using System.ComponentModel.DataAnnotations;

namespace Vehicle_World.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\+?[0-9]{10,11}$", ErrorMessage = "Please enter a valid contact number.")]
        public string ConNum { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]

        public string Message { get; set; }
    }

}
