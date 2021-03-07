using System.ComponentModel.DataAnnotations;

namespace PortalGate.Models
{
    public class UnitStartPageURI
    {
        [Key]
        public int Id { get; set; }
        public int Railroad { get; set; }
        public int Industry { get; set; }
        public int Unit { get; set; }
        public string URI { get; set; }
    }
}
