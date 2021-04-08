using System;
using System.ComponentModel.DataAnnotations;

namespace RailroadPortalClassLibrary
{
    public class Telegram
    {
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public byte IsActual { get; set; }
    }
}
