namespace Tasty_Treat_be.DTOs
{
    public class ChatMsgDto
    {
        public int MsgId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public int? RecipientId { get; set; }
        public string MsgTxt { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateChatMsgDto
    {
        public int SenderId { get; set; }
        public int? RecipientId { get; set; }
        public string MsgTxt { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
    }

    public class UpdateChatMsgDto
    {
        public string? MsgTxt { get; set; }
        public bool? IsRead { get; set; }
    }

    public class ConversationUserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastMessage { get; set; } = string.Empty;
        public DateTime LastMessageAt { get; set; }
        public int UnreadCount { get; set; }
    }
}
