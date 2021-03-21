using System.ComponentModel.DataAnnotations;

namespace RailroadPortalClassLibrary
{
    public class EmailConfirmModel
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string HashForCheck { get; set; }
    }
}
