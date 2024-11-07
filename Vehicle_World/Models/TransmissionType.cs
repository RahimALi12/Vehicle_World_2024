using System.ComponentModel.DataAnnotations;

namespace Vehicle_World.Models
{
    public class TransmissionType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}