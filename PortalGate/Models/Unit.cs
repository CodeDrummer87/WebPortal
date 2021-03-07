using System.ComponentModel.DataAnnotations;

namespace PortalGate.Models
{
    public class Unit
    {
        [Key]
        public int Id { get; set; }
        public int Railroad { get; set; }
        public int Industry { get; set; }
        public string ShortTitle { get; set; }
        public string FullTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
    }
}
