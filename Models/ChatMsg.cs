using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasty_Treat_be.Models
{
    [Table("Chat_msg")]
    public class ChatMsg
    {
        [Key]
        [Column("msg_id")]
        public int MsgId { get; set; }

        [Column("sender_id")]
        public int SenderId { get; set; }

        [Required]
        [Column("msg_txt")]
        public string MsgTxt { get; set; } = string.Empty;

        [Column("is_read")]
        public bool IsRead { get; set; } = false;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("SenderId")]
        public virtual User? Sender { get; set; }
    }
}
