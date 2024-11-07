using System.ComponentModel.DataAnnotations;

namespace Vehicle_World.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        public string SenderId { get; set; }  // User who sends the message
        public string ReceiverId { get; set; }  // User who receives the message
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
