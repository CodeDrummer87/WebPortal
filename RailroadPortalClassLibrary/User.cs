using System.ComponentModel.DataAnnotations;

namespace RailroadPortalClassLibrary
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte[] Salt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int PositionId { get; set; }
        public int? RoleId { get; set; }
        public byte ConfirmedEmail { get; set; }
        public byte IsActual { get; set; }
    }
}
