using System.ComponentModel.DataAnnotations;

namespace PortalGate.Models
{
    public class Railroad
    {
        [Key]
        public int Id { get; set; }
        public string FullTitle { get; set; }
        public string Abbreviation { get; set; }
        public string Code { get; set; }
    }
}
