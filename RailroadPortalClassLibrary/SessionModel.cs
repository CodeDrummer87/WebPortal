using System;
using System.ComponentModel.DataAnnotations;

namespace RailroadPortalClassLibrary
{
    public class SessionModel
    {
        [Key]
        public int Id { get; set; }
        public string SessionId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expired { get; set; }
    }
}
