using System.ComponentModel.DataAnnotations;

namespace RailroadPortalClassLibrary
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
