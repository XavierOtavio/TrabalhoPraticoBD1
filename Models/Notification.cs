using System;

namespace TrabalhoFinal3.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime? SentDate { get; set; }
        public bool Read { get; set; }
        public DateTime? ReadDate { get; set; }
    }
}
