using System.ComponentModel.DataAnnotations;

namespace Vehicle_World.Models
{
    public class ConditionType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
